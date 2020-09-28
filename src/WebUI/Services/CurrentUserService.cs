using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id);
            UserId = id;
        }

        public Guid UserId { get; }
    }
}
