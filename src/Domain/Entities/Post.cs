using System;
using System.Collections.Generic;
using SharedPhotoAlbum.Domain.Common;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class Post : AuditableEntity
    {
        public long Id { get; set; }
        
        public string Text { get; set; }
        
        public string LinkUrl { get; set; }
        
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        
        public IList<StoredMedia> StoredMedia { get; set; } = new List<StoredMedia>();

        public Feed Feed { get; set; }

        public long FeedId { get; set; }
    }
}