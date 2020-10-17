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

        public TokenController(IConfiguration configuration, IUserStore<ApplicationUser> userStore)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }
        
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var userId = User.FindFirst(JwtClaimTypes.Subject)?.Value;
            var user = await _userStore.FindByIdAsync(userId, CancellationToken.None);
            if (user == null)
            {
                return NotFound();
            }
            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:SigningKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}