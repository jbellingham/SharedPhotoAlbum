using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<UserDetailsDto> Get()
        {
            return Ok(new UserDetailsDto
            {
                ProfilePictureUrl = HttpContext.User.FindFirst(JwtClaimTypes.Picture)?.Value,
                FirstName = HttpContext.User.FindFirst(JwtClaimTypes.GivenName)?.Value,
                LastName = HttpContext.User.FindFirst(JwtClaimTypes.FamilyName)?.Value,
            });
        }
    }

    public class UserDetailsDto
    {
        public string ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}