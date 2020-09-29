using System;
using System.Collections.Generic;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class PostDto : IMapFrom<Post>
    {
        public Guid Id { get; set; }
        
        public string LinkUrl { get; set; }
        
        public string Text { get; set; }
        
        public IList<CommentDto> Comments { get; set; } = new List<CommentDto>();
        
        public IList<StoredMediaDto> StoredMedia { get; set; } = new List<StoredMediaDto>();
    }
}