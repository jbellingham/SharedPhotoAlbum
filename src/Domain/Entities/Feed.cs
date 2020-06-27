using System;
using System.Collections.Generic;
using SharedPhotoAlbum.Domain.Common;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class Feed : AuditableEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<Post> Posts { get; set; } = new List<Post>();

        public ApplicationUser Owner { get; set; }

        public string OwnerId { get; set; }
    }
}
