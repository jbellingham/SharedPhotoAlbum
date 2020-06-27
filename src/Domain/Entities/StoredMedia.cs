using System;
using SharedPhotoAlbum.Domain.Common;
using SharedPhotoAlbum.Domain.Enums;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class StoredMedia : AuditableEntity
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public MediaType MediaType { get; set; }
        
        public string Content { get; set; }
        
        public long PostId { get; set; }
        
        public Post Post { get; set; }
    }
}