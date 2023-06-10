using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Common.Exceptions;
using SharedPhotoAlbum.Application.Posts.Commands.CreatePost;

namespace SharedPhotoAlbum.Application.IntegrationTests.Posts.Commands
{
    using static Testing;

    public class CreatePostTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreatePostCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldCreatePost()
        {
            var post = await new CreatePost_TestHarness()
                .WithPostText("New Post")
                .WithUser(await RunAsDefaultUserAsync())
                .Build();
            
            post.Was_Successfully_Created_Based_Off_Command();
        }
    }
}
