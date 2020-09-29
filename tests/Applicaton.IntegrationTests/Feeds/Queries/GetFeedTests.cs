using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Feeds.Queries.GetFeed;
using SharedPhotoAlbum.Application.IntegrationTests.Seeds;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.Application.IntegrationTests.Feeds.Queries
{
    using static Testing;

    public class GetFeedTests : TestBase
    {
        public class GetPostsTests : TestBase
        {
            // [Test]
            // public async Task ShouldGetCorrectFeed()
            // {
            //     using var scope = 
            //     var userId = await RunAsDefaultUserAsync();
            //     var feed = FeedSeed.UserFeeds.FirstOrDefault(_ => _.Key == userId).Value;
            //     var query = new GetFeedQuery
            //     {
            //         FeedId = feed.Id
            //     };
            //     var result = await SendAsync(query);
            //     result.Posts.Should().HaveCount(1);
            //     var post = result.Posts.First();
            //     post.Text.Should().Be(feed.Posts.First().Text);
            //
            //     var expectedComments =
            //         feed.Posts.First().Comments
            //             .OrderByDescending(_ => _.Created).ProjectTo<CommentDto>(); //post.Comments.OrderByDescending(_ => _.)
            // }
        }
    }
}