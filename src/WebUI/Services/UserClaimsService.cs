using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.WebUI.Services
{
    public class UserClaimsService : IUserClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;
        private ClaimsPrincipal _externalLoginPrincipal;
        private IEnumerable<Claim> _existingClaims;
        
        private readonly List<string> _claimsToAdd = new List<string> { JwtClaimTypes.Picture, JwtClaimTypes.GivenName, JwtClaimTypes.FamilyName };
        
        public UserClaimsService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        
        public async Task AddMissingUserClaims(ApplicationUser user, ClaimsPrincipal externalLoginClaimsPrincipal)
        {
            _user = user;
            _externalLoginPrincipal = externalLoginClaimsPrincipal;
            _existingClaims = await _userManager.GetClaimsAsync(user);

            foreach (string claimToAdd in _claimsToAdd)
            {
                await AddClaimIfMissing(claimToAdd);
            }
        }

        private async Task AddClaimIfMissing(string claimToAdd)
        {
            if (ShouldAddClaimOfType(claimToAdd))
            {
                await _userManager.AddClaimAsync(_user,
                _externalLoginPrincipal.FindFirst(claimToAdd));
            }
        }

        private bool ShouldAddClaimOfType(string claimType)
        {
            return _externalLoginPrincipal.HasClaim(_ => _.Type == claimType) &&
                   _existingClaims.All(_ => _.Type != claimType);
        }
    }
}