using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.Enums;

namespace SharedPhotoAlbum.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<int>
    {
        public string[] Files { get; set; }
        public string Text { get; set; }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreatePostCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var media = request.Files.Select(_ => new StoredMedia
            {
                Content = Encoding.ASCII.GetBytes(_),
                MediaType = MediaType.Image
            }).ToList();
            
            var entity = new Post
            {
                Text = request.Text,
                StoredMedia = media
            };
            
            await _dbContext.Posts.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}