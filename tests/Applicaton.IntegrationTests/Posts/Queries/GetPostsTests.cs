using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.IntegrationTests.Seeds;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.Application.IntegrationTests.Posts.Queries
{
    using static Testing;
    
    public class GetPostsTests : TestBase
    {
        private readonly IApplicationDbContext _db;
        private readonly ICurrentUserService _currentUserService;

        [Test]
        public async Task ShouldGetPostsForFeed()
        {
            var userId = await RunAsDefaultUserAsync();
            var feed = FeedSeed.UserFeeds.FirstOrDefault(_ => _.Key == userId).Value;
            var query = new GetPostsQuery
            {
                FeedId = feed.Id
            };
            var result = await SendAsync(query);
            result.Posts.Should().HaveCount(1);
            result.Posts.First().Text.Should().Be(feed.Posts.First().Text);
        }
    }
}