using System.Collections.Generic;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.Application.Feeds.Queries.GetFeed
{
    public class FeedVm
    {
        public IList<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
