using System;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure.Identity;

namespace SharedPhotoAlbum.WebUI.Controllers
{
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
                ProfilePictureUrl = HttpContext.User.FindFirst(CustomClaimTypes.Facebook.Picture)?.Value,
                FirstName = HttpContext.User.FindFirst(CustomClaimTypes.Facebook.FirstName)?.Value,
                LastName = HttpContext.User.FindFirst(CustomClaimTypes.Facebook.LastName)?.Value,
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