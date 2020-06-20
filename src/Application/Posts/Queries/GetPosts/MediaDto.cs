using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.Enums;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class MediaDto : IMapFrom<StoredMedia>
    {
        public int Id { get; set; }
        public string[] Content { get; set; } = {};
        public MediaType MediaType { get; set; }
        public int PostId { get; set; }
    }
}