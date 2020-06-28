using System.Collections.Generic;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Feeds.Queries.GetFeed
{
    public class FeedVm : IMapFrom<Feed>
    {
        public string Name { get; set; }

        public IList<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
