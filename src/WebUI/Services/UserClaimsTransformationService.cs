using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SharedPhotoAlbum.Application.Common.Exceptions;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.WebUI.Services
{
    public class UserClaimsTransformationService : IClaimsTransformation
    {
        private const string Issuer = "Claims.Transformation.Service.Authority";
        private readonly UserManager<ApplicationUser> _userManager;
        private ClaimsIdentity _identity;
        private ApplicationUser _user;
        private List<Claim> _claims;

        public UserClaimsTransformationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                return await BuildNewPrincipalWithStoredClaims(principal);
            }
            catch (NotFoundException e)
            {
                return principal;
            }
        }

        private async Task<ClaimsPrincipal> BuildNewPrincipalWithStoredClaims(ClaimsPrincipal principal)
        {
            _identity = principal.Identities.FirstOrDefault();
            await GetUserByClaimsPrincipal();
            await PopulateClaimsList();

            var claimsIdentity = new ClaimsIdentity(_claims, _identity.AuthenticationType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        private async Task GetUserByClaimsPrincipal()
        {
            var identifier = _identity?.FindFirst(ClaimTypes.NameIdentifier);
            Guid.TryParse(identifier?.Value, out var id);
            _user = await _userManager.FindByIdAsync(id.ToString());
            if (_user == null)
            {
                throw new NotFoundException();
            }
        }

        private async Task PopulateClaimsList()
        {
            var userClaims = await _userManager.GetClaimsAsync(_user);

            if (!userClaims.Any())
            {
                throw new Exception();
            }

            _claims = userClaims.Where(FilterDuplicateClaims())
                .Select(c => new Claim(c.Type, c.Value, c.ValueType, GetIssuer(c)))
                .ToList();
            
            _claims.AddRange(_identity.Claims);
        }

        private Func<Claim, bool> FilterDuplicateClaims()
        {
            return storedClaim =>
                _identity.Claims.All(identityClaim => identityClaim.Type != storedClaim.Type);
        }

        private static string GetIssuer(Claim claim)
        {
            return string.IsNullOrWhiteSpace(claim.Issuer) ? Issuer : claim.Issuer;
        }
    }
}