using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;
using SharedPhotoAlbum.Application.Common.Exceptions;

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
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateComment()
        {
            var comment = await new CreateComment_TestHarness()
                .WithCommentText("New comment")
                .WithUser(await RunAsDefaultUserAsync())
                .Build();
            
            comment.Was_Successfully_Created_Based_Off_Command();
        }
    }
}