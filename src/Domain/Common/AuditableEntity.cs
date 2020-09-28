using System;

namespace SharedPhotoAlbum.Domain.Common
{
    public abstract class AuditableEntity
    {
        public Guid CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public Guid LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
