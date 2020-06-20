using System.Collections.Generic;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class PostsVm
    {
        public IList<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}