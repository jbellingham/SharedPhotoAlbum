using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }
        public async Task<string> ProfilePictureUrl()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                // do something
            }
            //
            // var identity = (ClaimsIdentity)this.User.Identity;
            // if (info.Principal.HasClaim(_ => _.Type == JwtClaimTypes.Picture))
            // {
            //     if (!identity.HasClaim(_ => _.Type == JwtClaimTypes.Picture))
            //     {
            //         
            //     }
            // }
            
            // add to onloginsuccess
            var authResult = await HttpContext.AuthenticateAsync();
            var identity = (ClaimsIdentity)this.User.Identity;
            var result = identity.Claims.SingleOrDefault(_ => _.Type == JwtClaimTypes.Picture)?.Value;
            return result;
        }
    }
}