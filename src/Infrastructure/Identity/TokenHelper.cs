using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure.Identity
{
    public interface ITokenHelper
    {
        string GenerateJwtToken(ApplicationUser user);
        bool ValidateToken(string token);
    }

    public class Token
    {
        public string TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
    
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenHandler =  new JwtSecurityTokenHandler();
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            SecurityTokenDescriptor tokenDescriptor = CreateSecurityTokenDescriptor(user);
            SecurityToken token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor CreateSecurityTokenDescriptor(ApplicationUser user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:SigningKey"]);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),//DateTime.UtcNow.AddDays(7), // generate token that is valid for 7 days
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }

        public bool ValidateToken(string token)
        {
            try
            {
                _tokenHandler.ValidateToken(
                    token,
                    TokenValidation.BuildParameters(_configuration),
                    out SecurityToken _);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}