using SharedPhotoAlbum.Domain.Common;
using SharedPhotoAlbum.Domain.Enums;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class StoredMedia : AuditableEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public MediaType MediaType { get; set; }
        
        public byte[] Content { get; set; }
        
        public int PostId { get; set; }
        
        public Post Post { get; set; }
    }
}