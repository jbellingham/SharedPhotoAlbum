using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure.Identity;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    [AllowAnonymous]
    public class TokenController : ApiController
    {
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenHelper _tokenHelper;

        public TokenController(
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager,
            ITokenHelper tokenHelper)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenHelper = tokenHelper ?? throw new ArgumentNullException(nameof(tokenHelper));
        }
        
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            string userId =
                await AddUserIdClaimIfEmpty() ??
                User.FindFirst(JwtClaimTypes.Subject)?.Value;
            
            ApplicationUser user = await _userStore.FindByIdAsync(userId, CancellationToken.None);
            if (user == null)
            {
                return NotFound();
            }
            string token = GenerateJwtToken(user);
            return Ok(token);
        }

        private async Task<string> AddUserIdClaimIfEmpty()
        {
            if (User.HasClaim(_ => _.Type == JwtClaimTypes.Subject))
                return null;
            
            string userId = _userManager.GetUserId(User);
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Subject, userId));

            return userId;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            return _tokenHelper.GenerateJwtToken(user);
        }
    }
}