using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.WebUI.Services
{
    public interface ITokenHelper
    {
        Token GenerateJwtToken(ApplicationUser user);
    }

    public class Token
    {
        public string TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
    
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _configuration;
        
        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public Token GenerateJwtToken(ApplicationUser user)
        {
            // var claims = new[]
            // {
            //     new Claim(JwtRegisteredClaimNames.Sub, userName),
            //     // new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            //     new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            //     identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Rol),
            //     identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
            // };
            
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtOptions:SigningKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return new Token
            {
                TokenString = tokenHandler.WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
}