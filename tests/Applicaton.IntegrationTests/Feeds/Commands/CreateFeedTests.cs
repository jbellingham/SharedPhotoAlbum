using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Common.Exceptions;
using SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed;
using SharedPhotoAlbum.Application.IntegrationTests.Posts.Commands;

namespace SharedPhotoAlbum.Application.IntegrationTests.Feeds.Commands
{
    using static Testing;

    public class CreateFeedTests
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateFeedCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateFeed()
        {
            var feed = await new CreateFeed_TestHarness()
                .WithFeedName("New Feed")
                .WithUser(await RunAsDefaultUserAsync())
                .Build();

            feed.Was_Successfully_Created_Based_Off_Command();
        }
    }
}
