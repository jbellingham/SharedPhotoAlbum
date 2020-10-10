using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class ExternalLogin : ApiController
    {
        [AllowAnonymous]
        [HttpPost(nameof(ExternalLogin))]
        public IActionResult ExternalLogin(ExternalLoginModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return null;
            }

            var properties = new AuthenticationProperties { RedirectUri = _authenticationAppSettings.External.RedirectUri };

            return Challenge(properties, model.Provider);
        }
        
        [AllowAnonymous]
        [HttpGet(nameof(ExternalLoginCallback))]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            //Here we can retrieve the claims
            var result = await HttpContext.AuthenticateAsync("YourCustomScheme");

            return null;
        }
    }
}