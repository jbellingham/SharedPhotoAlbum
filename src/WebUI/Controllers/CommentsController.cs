using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;
using SharedPhotoAlbum.Application.Comments.Queries.GetComments;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class CommentsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateCommentCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<CommentsVm>> Get(Guid postId)
        {
            return await Mediator.Send(new GetCommentsQuery { PostId = postId });
        }
    }
}