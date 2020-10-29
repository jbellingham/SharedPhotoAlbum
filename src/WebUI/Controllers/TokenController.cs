using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class TokenController : ApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenController(
            IConfiguration configuration,
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
            SecurityTokenDescriptor tokenDescriptor = CreateSecurityTokenDescriptor(user);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor CreateSecurityTokenDescriptor(ApplicationUser user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:SigningKey"]);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7), // generate token that is valid for 7 days
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }
    }
}