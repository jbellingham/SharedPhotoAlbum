using System;
using System.Threading.Tasks;
using FluentAssertions;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;
using SharedPhotoAlbum.Application.Posts.Commands.CreatePost;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Comments.Commands
{
    using static Testing;

    public class CreateComment_TestHarness
    {
        private string _commentText;
        private string _userId;
        private Comment _comment;
        private CreateCommentCommand _command;

        public CreateComment_TestHarness WithCommentText(string comment)
        {
            _commentText = comment;
            return this;
        }

        public CreateComment_TestHarness WithUser(string userId)
        {
            _userId = userId;
            return this;
        }

        public async Task<CreateComment_TestHarness> Build()
        {
            var postCommand = new CreatePostCommand
            {
                Text = "Some post"
            };
            var postId = await SendAsync(postCommand);

            _command = new CreateCommentCommand
            {
                Text = _commentText,
                PostId = postId
            };

            var commentId = await SendAsync(_command);
            _comment = await FindAsync<Comment>(commentId);
            return this;
        }

        public void Was_Successfully_Created_Based_Off_Command()
        {
            _comment.Should().NotBeNull();
            _comment.Text.Should().Be(_command.Text);
            _comment.CreatedBy.Should().Be(_userId);
            _comment.Created.Should().BeCloseTo(DateTime.Now, 10000);
            _comment.LastModifiedBy.Should().BeNull();
            _comment.LastModified.Should().BeNull();
        }
    }
}
