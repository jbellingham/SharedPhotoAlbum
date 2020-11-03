using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure.Identity
{
    public interface ITokenHelper
    {
        Token GenerateJwtToken(ApplicationUser user);
        Task<Token> GenerateRefreshToken(ApplicationUser user);
        Task<bool> ValidateRefreshToken(string authToken, string refreshToken);
    }

    public class Token
    {
        public Token(string tokenString, DateTime? validTo)
        {
            TokenString = tokenString;
            ValidTo = validTo;
        }
        
        public string TokenString { get; }
        public DateTime? ValidTo { get; }
    }
    
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IUserAuthenticationTokenStore<ApplicationUser> _userAuthenticationTokenStore;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenHandler =  new JwtSecurityTokenHandler();
        }
        
        public async Task<Token> GenerateRefreshToken(ApplicationUser user)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber);
                await _userAuthenticationTokenStore.RemoveTokenAsync(user, "Api", "RefreshToken", CancellationToken.None);
                await _userAuthenticationTokenStore.SetTokenAsync(user, "Api", "RefreshToken", token, CancellationToken.None);
                return new Token(token, null);
            }
        }

        public async Task<bool> ValidateRefreshToken(string authToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(authToken);
            var userId = principal.FindFirst(JwtClaimTypes.Subject)?.Value;
            var user = await _userAuthenticationTokenStore.FindByIdAsync(userId, CancellationToken.None);
            var storedToken = await _userAuthenticationTokenStore.GetTokenAsync(user, "Api", "RefreshToken", CancellationToken.None);
            return storedToken != refreshToken;
        }

        public Token GenerateJwtToken(ApplicationUser user)
        {
            SecurityTokenDescriptor tokenDescriptor = CreateSecurityTokenDescriptor(user);
            SecurityToken token = _tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = _tokenHandler.WriteToken(token);
            return new Token(tokenString, tokenDescriptor.Expires);
        }

        private SecurityTokenDescriptor CreateSecurityTokenDescriptor(ApplicationUser user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:SigningKey"]);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }
        
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(
                token,
                TokenValidation.BuildParameters(_configuration),
                out securityToken);
            
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
    }
}