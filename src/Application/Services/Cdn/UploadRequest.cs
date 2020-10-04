using System;
using System.Collections.Generic;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Application.Services.Cdn
{
    public class UploadRequest
    {
        public Guid FeedId { get; set; }
        public IList<File> Files { get; set; } = new List<File>();
    }
}