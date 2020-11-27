using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Infrastructure.Identity;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class UserController : ApiController
    {
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