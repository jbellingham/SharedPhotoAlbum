using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Application.Posts.Commands.CreatePost;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    [Authorize]
    public class PostController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<long>> Create(CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<PostsVm>> Get()
        {
            return await Mediator.Send(new GetPostsQuery());
        }
    }
}