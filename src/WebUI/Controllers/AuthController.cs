using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure.Identity;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public Token AuthToken { get; set; }
        public Token RefreshToken { get; set; }
    }
    
    [AllowAnonymous]
    public class AuthController : ApiController
    {
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenHelper _tokenHelper;

        public AuthController(
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager,
            ITokenHelper tokenHelper)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenHelper = tokenHelper ?? throw new ArgumentNullException(nameof(tokenHelper));
        }
        
        [HttpGet]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate()
        {
            if (User.IsAuthenticated())
            {
                return Ok(new AuthenticationResponse
                {
                    IsAuthenticated = true
                });
            }
            
            string userId =
                await AddUserIdClaimIfEmpty() ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            ApplicationUser user = await _userStore.FindByIdAsync(userId, CancellationToken.None);
            if (user == null)
            {
                return NotFound();
            }

            var authToken = _tokenHelper.GenerateJwtToken(user);
            var refreshToken = await _tokenHelper.GenerateRefreshToken(user);
            
            return Ok(new AuthenticationResponse
            {
                AuthToken = authToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken(string authToken, string refreshToken)
        {
            var refreshTokenIsValid = await _tokenHelper.ValidateRefreshToken(authToken, refreshToken); 
            if (!refreshTokenIsValid)
            {
                return BadRequest("Invalid client request");
            }

            var userId = User.FindFirst(JwtClaimTypes.Subject)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var newAuthToken = _tokenHelper.GenerateJwtToken(user);
            var newRefreshToken = await _tokenHelper.GenerateRefreshToken(user);
            return Ok(new AuthenticationResponse
            {
                AuthToken = newAuthToken,
                RefreshToken = newRefreshToken
            });
        }

        private async Task<string> AddUserIdClaimIfEmpty()
        {
            if (User.HasClaim(_ => _.Type == ClaimTypes.NameIdentifier))
                return null;
            
            string userId = _userManager.GetUserId(User);
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, userId));

            return userId;
        }
    }
}