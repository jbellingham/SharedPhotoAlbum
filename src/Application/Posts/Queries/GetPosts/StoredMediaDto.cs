using System;
using AutoMapper;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Application.Feeds.Queries.GetFeed;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.Enums;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class StoredMediaDto : IMapFrom<StoredMedia>
    {
        public Guid Id { get; set; }
        
        public string PublicId { get; set; }
        
        public string MimeType { get; set; }
        
        public Guid PostId { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StoredMedia, StoredMediaDto>()
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.File.MimeType))
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => src.File.DataUrl));
        }
    }
}