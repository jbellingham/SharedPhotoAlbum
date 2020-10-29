using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("./Login");
        }
    }
}