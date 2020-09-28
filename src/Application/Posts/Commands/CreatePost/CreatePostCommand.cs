using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public IList<StoredMediaDto> StoredMedia { get; set; } = new List<StoredMediaDto>();
        
        public string Text { get; set; }

        public Guid FeedId { get; set; }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreatePostCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var media = request.StoredMedia.Select(_ => new StoredMedia
            {
                Content = _.Content,
                MediaType = _.MediaType
            }).ToList();
            
            var entity = new Post
            {
                FeedId = request.FeedId,
                Text = request.Text,
                StoredMedia = media
            };
            
            await _dbContext.Posts.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}