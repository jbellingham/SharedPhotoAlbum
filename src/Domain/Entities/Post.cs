using SharedPhotoAlbum.Domain.Common;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class Post : AuditableEntity
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
    }
}