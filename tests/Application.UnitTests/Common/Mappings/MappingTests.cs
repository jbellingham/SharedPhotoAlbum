using AutoMapper;
using SharedPhotoAlbum.Application.Common.Mappings;
using SharedPhotoAlbum.Domain.Entities;
using NUnit.Framework;
using System;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Test]
        [TestCase(typeof(StoredMedia), typeof(MediaDto))]
        [TestCase(typeof(Post), typeof(PostDto))]
        [TestCase(typeof(Comment), typeof(CommentDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
