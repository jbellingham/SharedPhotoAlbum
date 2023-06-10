using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;
using SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed;
using SharedPhotoAlbum.Application.Posts.Commands.CreatePost;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Comments.Commands
{
    using static Testing;

    public class CreateComment_TestHarness
    {
        private string _commentText;
        private Guid? _userId;
        private Comment _comment;
        private CreateCommentCommand _command;

        public CreateComment_TestHarness WithCommentText(string comment)
        {
            _commentText = comment;
            return this;
        }

        public CreateComment_TestHarness WithUser(Guid? userId)
        {
            _userId = userId;
            return this;
        }

        public async Task<CreateComment_TestHarness> Build()
        {
            var feedCommand = new CreateFeedCommand
            {
                Description = "Some feed",
                Name = "Some feed"
            };
            var feedId = await SendAsync(feedCommand);
            
            var postCommand = new CreatePostCommand
            {
                Text = "Some post",
                FeedId = feedId
            };
            var createPostCommandResponse = await SendAsync(postCommand);

            _command = new CreateCommentCommand
            {
                Text = _commentText,
                PostId = createPostCommandResponse.PostId.Value
            };

            var commentId = await SendAsync(_command);
            _comment = await FindAsync<Comment>(commentId);
            return this;
        }

        public void Was_Successfully_Created_Based_Off_Command()
        {
            _comment.Should().NotBeNull();
            _comment.Text.Should().Be(_command.Text);
            _comment.CreatedBy.Should().Be(_userId.Value);
            _comment.Created.Should().BeCloseTo(DateTime.Now, new TimeSpan(10000));
            // _comment.LastModifiedBy.Should().BeNull();
            _comment.LastModified.Should().BeNull();
        }
    }
}
