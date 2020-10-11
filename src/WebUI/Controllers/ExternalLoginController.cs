using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure;
using SharedPhotoAlbum.WebUI.Services;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class ExternalLoginController : ApiController
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ExternalLoginController(
            ITokenHelper tokenHelper,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _tokenHelper = tokenHelper ?? throw new ArgumentNullException(nameof(tokenHelper));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(DependencyInjection.CookieScheme);
            if (authenticateResult?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            // var info = new ExternalLoginInfo(ClaimsPrincipal.Current, "Facbook", );
            if (info == null)
            {
                throw new Exception("Error loading external login information.");
            }
            
            var signInResult =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, true);

            if (signInResult.Succeeded)
            {
                // Store the access token and resign in so the token is included in
                // in the cookie
                var loggedInUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                var props = new AuthenticationProperties { IsPersistent = true };
                props.StoreTokens(info.AuthenticationTokens);

                await _signInManager.SignInAsync(loggedInUser, props, info.LoginProvider);
                var token = _tokenHelper.GenerateJwtToken(loggedInUser);
                return Ok(token); //(returnUrl ?? "/", );
            }
            
            if (signInResult.IsLockedOut)
            {
                // return RedirectToPage("./Lockout");
            }

            var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
            
            var user = new ApplicationUser { UserName = email, Email = email };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    // If they exist, add claims to the user for:
                    //    Given (first) name
                    //    Locale
                    //    Picture
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                    {
                        await _userManager.AddClaimAsync(user, 
                            info.Principal.FindFirst(ClaimTypes.GivenName));
                    }

                    if (info.Principal.HasClaim(c => c.Type == JwtClaimTypes.Picture))
                    {
                        await _userManager.AddClaimAsync(user, 
                            info.Principal.FindFirst(JwtClaimTypes.Picture));
                    }
                    // _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);

                    // await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // If account confirmation is required, we need to show the link if we don't have a real email sender
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("./RegisterConfirmation", new { Email = email });
                    }

                    // Include the access token in the properties
                    var props = new AuthenticationProperties { IsPersistent = true };
                    props.StoreTokens(info.AuthenticationTokens);
                    await _signInManager.SignInAsync(user, props);

                    var token = _tokenHelper.GenerateJwtToken(user);
                    return Ok(token); //LocalRedirect(returnUrl);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return null;
        }
    }
}