using System.Security.Claims;
using System.Threading.Tasks;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Common.Interfaces
{
    public interface IUserClaimsService
    {
        Task AddMissingUserClaims(ApplicationUser user, ClaimsPrincipal externalLoginClaimsPrincipal);
    }
}