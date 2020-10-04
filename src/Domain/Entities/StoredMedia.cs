using System;
using SharedPhotoAlbum.Domain.Common;
using SharedPhotoAlbum.Domain.Enums;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class StoredMedia : AuditableEntity
    {
        public Guid Id { get; set; }

        public File File { get; set; }
        
        public Guid PostId { get; set; }
        
        public Post Post { get; set; }
    }
}