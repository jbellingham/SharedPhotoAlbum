using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;
using SharedPhotoAlbum.Application.Common.Exceptions;
using SharedPhotoAlbum.Application.IntegrationTests.Posts.Commands;

namespace SharedPhotoAlbum.Application.IntegrationTests.Comments.Commands
{
    using static Testing;
    
    public class CreateCommentTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateCommentCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
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