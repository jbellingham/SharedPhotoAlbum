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
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}
