﻿using System;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.Enums;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class StoredMediaDto : IMapFrom<StoredMedia>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public MediaType MediaType { get; set; }
        
        public string Content { get; set; }
        
        public Guid PostId { get; set; }
    }
}