using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;

namespace WebUI.Controllers
{
    public class CommentController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateCommentCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}