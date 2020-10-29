using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;
using SharedPhotoAlbum.Application.Services.Cdn;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<CreatePostCommandResponse>
    {
        public IList<string> Files { get; set; } = new List<string>();
        
        public string Text { get; set; }

        public Guid FeedId { get; set; }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostCommandResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICdnService _cdn;

        public CreatePostCommandHandler(IApplicationDbContext dbContext, ICdnService cdn)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _cdn = cdn ?? throw new ArgumentNullException(nameof(cdn));
        }
        
        public async Task<CreatePostCommandResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var response = new CreatePostCommandResponse();
            var fileUploadResult = await UploadFiles(request);

            if (fileUploadResult.Success)
            {
                var entity = new Post
                {
                    FeedId = request.FeedId,
                    Text = request.Text,
                    StoredMedia = fileUploadResult.UploadResults.Select(_ => new StoredMedia
                    {
                        File = _.File
                    }).ToList()
                };
            
                await _dbContext.Posts.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                response.PostId = entity.Id;
                response.Success = true;
            }

            return response;
        }

        private async Task<FileUploadResult> UploadFiles(CreatePostCommand request)
        {
            var uploadRequest = new UploadRequest
            {
                FeedId = request.FeedId,
                Files = request.Files.Select(fileString => File.For(fileString)).ToList()
            };
            return await _cdn.UploadFiles(uploadRequest);
        }
    }

    public class CreatePostCommandResponse
    {
        public Guid? PostId { get; set; }
        public bool Success { get; set; }
    }
}