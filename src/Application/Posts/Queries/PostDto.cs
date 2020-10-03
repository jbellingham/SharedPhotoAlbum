using System;
using System.Collections.Generic;
using SharedPhotoAlbum.Application.Comments.Queries.GetComments;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Posts.Queries
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