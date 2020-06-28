using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Feeds.Commands
{
    using static Testing;

    public class CreateFeed_TestHarness
    {
        private string _name;
        private string _userId;
        private CreateFeedCommand _command;
        private Feed _feed;


        public CreateFeed_TestHarness WithFeedName(string name)
        {
            _name = name;
            return this;
        }

        public CreateFeed_TestHarness WithUser(string userId)
        {
            _userId = userId;
            return this;
        }
        
        public async Task<CreateFeed_TestHarness> Build()
        {
            _command = new CreateFeedCommand
            {
                Name = _name
            };

            var feedId = await SendAsync(_command);
            _feed = await FindAsync<Feed>(feedId);
            return this;
        }

        public void Was_Successfully_Created_Based_Off_Command()
        {
            _feed.Should().NotBeNull();
            _feed.Name.Should().Be(_command.Name);
            _feed.CreatedBy.Should().Be(_userId);
            _feed.Created.Should().BeCloseTo(DateTime.Now, 10000);
            _feed.LastModifiedBy.Should().BeNull();
            _feed.LastModified.Should().BeNull();
        }
    }
}
