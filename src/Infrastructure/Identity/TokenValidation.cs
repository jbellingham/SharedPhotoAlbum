using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SharedPhotoAlbum.Infrastructure.Identity
{
    public static class TokenValidation
    {
        public static TokenValidationParameters BuildParameters(IConfiguration configuration)
        {
            var validIssuer = configuration["JwtOptions:Issuer"];
            var validAudience = configuration["JwtOptions:Audience"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtOptions:SigningKey"]));
            
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = validIssuer,

                ValidateAudience = true,
                ValidAudience = validAudience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}