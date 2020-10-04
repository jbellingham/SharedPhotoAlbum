using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed;
using SharedPhotoAlbum.Application.Posts.Commands.CreatePost;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Posts.Commands
{
    using static Testing;
    
    public class CreatePost_TestHarness
    {
        private string _postText;
        private Guid? _userId;
        private Post _post;

        private CreatePostCommand _command;

        public CreatePost_TestHarness WithPostText(string text)
        {
            _postText = text;
            return this;
        }

        public async Task<CreatePost_TestHarness> Build()
        {
            var feedCommand = new CreateFeedCommand
            {
                Description = "Some feed",
                Name = "Some feed"
            };
            var feedId = await SendAsync(feedCommand);
            
            _command = new CreatePostCommand
            {
                Text = _postText,
                FeedId = feedId
            };

            var createPostCommandResponse = await SendAsync(_command);
            _post = await FindAsync<Post>(createPostCommandResponse.PostId.Value);
            return this;
        }

        public CreatePost_TestHarness WithUser(Guid? userId)
        {
            _userId = userId;
            return this;
        }

        public void Was_Successfully_Created_Based_Off_Command()
        {
            _post.Should().NotBeNull();
            _post.Text.Should().Be(_command.Text);
            _post.CreatedBy.Should().Be(_userId.Value);
            _post.Created.Should().BeCloseTo(DateTime.Now, 10000);
            // _post.LastModifiedBy.Should().BeNull();
            _post.LastModified.Should().BeNull();
        }
    }
}