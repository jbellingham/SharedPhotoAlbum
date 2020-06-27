using System;
using SharedPhotoAlbum.Domain.Common;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public long Id { get; set; }

        public string Text { get; set; }
        
        public int Likes { get; set; }
        
        public long PostId { get; set; }
        
        public Post Post { get; set; }
    }
}