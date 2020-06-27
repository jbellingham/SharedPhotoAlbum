using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class FeedsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<long>> Create(CreateCommentCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
