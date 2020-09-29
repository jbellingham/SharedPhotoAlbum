using System;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class CommentDto : IMapFrom<Comment>
    {
        public Guid Id { get; set; }
        
        public string Text { get; set; }
        
        public int Likes { get; set; }
        
        public Guid PostId { get; set; }
    }
}