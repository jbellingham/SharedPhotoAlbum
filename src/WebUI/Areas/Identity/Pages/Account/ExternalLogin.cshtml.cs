using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.Exceptions;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SharedPhotoAlbum.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private ExternalLoginInfo _externalLoginInfo;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            string redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            try
            {
                await PopulateExternalLoginInfo();
                await SignInWithExternalLoginInfo();

                _logger.LogInformation(
                    "{Name} logged in with {LoginProvider} provider.",
                    _externalLoginInfo.Principal.Identity.Name,
                    _externalLoginInfo.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            catch (ExternalLoginInformationNotFoundException ex)
            {
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            catch (UserIsLockedOutException ex)
            {
                return RedirectToPage("./Lockout");
            }
            catch (LocalUserNotFoundForExternalLoginException ex)
            {
                PrepareRegistrationViewModel();
                return Page();
            }
        }

        private async Task PopulateExternalLoginInfo()
        {
            _externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (_externalLoginInfo == null)
            {
                ErrorMessage = "Error loading external login information.";
                throw new ExternalLoginInformationNotFoundException();
            }
        }

        private async Task SignInWithExternalLoginInfo()
        {
            SignInResult result = await _signInManager.ExternalLoginSignInAsync(
                _externalLoginInfo.LoginProvider,
                _externalLoginInfo.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (UserIsLockedOut(result))
            {
                throw new UserIsLockedOutException();
            }
                
            if (LocalUserNotFoundForExternalLogin(result))
            {
                throw new LocalUserNotFoundForExternalLoginException();
            }
        }

        private static bool UserIsLockedOut(SignInResult result)
        {
            return result.IsLockedOut;
        }

        private static bool LocalUserNotFoundForExternalLogin(SignInResult result)
        {
            return !result.Succeeded;
        }

        private void PrepareRegistrationViewModel()
        {
            ProviderDisplayName = _externalLoginInfo.ProviderDisplayName;
            if (_externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                Input = new InputModel
                {
                    Email = _externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email)
                };
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                IdentityResult result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        await AddProfilePictureClaimIfEmpty(info, user);

                        var userId = await _userManager.GetUserIdAsync(user);
                        await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Subject, userId));
                        await SendEmailConfirmationEmail(user, userId);

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private async Task AddProfilePictureClaimIfEmpty(ExternalLoginInfo info, ApplicationUser user)
        {
            if (info.Principal.HasClaim(_ => _.Type == JwtClaimTypes.Picture))
            {
                await _userManager.AddClaimAsync(user,
                    info.Principal.FindFirst(JwtClaimTypes.Picture));
            }
        }

        private async Task SendEmailConfirmationEmail(ApplicationUser user, string userId)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new {area = "Identity", userId = userId, code = code},
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
