using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor; 
            
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Guid UserId
        {
            get
            {
                Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id);
                return id;
            }
        }
    }
}
