using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.Application.IntegrationTests.Posts.Queries
{
    using static Testing;
    
    public class GetPostsTests : TestBase
    {
        [Test]
        public async Task ShouldGetAllPosts()
        {
            var query = new GetPostsQuery();

            var result = await SendAsync(query);
            result.Posts.Should().HaveCount(0);
        }
    }
}