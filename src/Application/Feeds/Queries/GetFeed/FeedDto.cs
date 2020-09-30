using System;
using AutoMapper;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Feeds.Queries.GetFeed
{
    public class FeedDto : IMapFrom<Feed>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsOwner { get; set; }
        
        public bool IsSubscription { get; set; }
        
        public string ShortCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Feed, FeedDto>()
                .ForMember(dest => dest.IsOwner, opt => opt.MapFrom<IsOwnerMapper>())
                .ForMember(dest => dest.IsSubscription, opt => opt.MapFrom<IsSubscriptionMapper>());
        }
    }

    public class IsSubscriptionMapper : IValueResolver<Feed, FeedDto, bool>
    {
        public bool Resolve(Feed source, FeedDto destination, bool destMember, ResolutionContext context)
        {
            return false;
        }
    }

    public class IsOwnerMapper : IValueResolver<Feed, FeedDto, bool>
    {
        private Guid _userId;
        public IsOwnerMapper(ICurrentUserService currentUserService)
        {
            _userId = currentUserService.UserId;
        }
        
        public bool Resolve(Feed source, FeedDto destination, bool destMember, ResolutionContext context)
        {
            return source.Owner.Id == _userId;
        }
    }
}