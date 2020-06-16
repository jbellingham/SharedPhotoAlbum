using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<int>
    {
        public string Text { get; set; }
        public int PostId { get; set; }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;
        
        public CreateCommentCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Comment
            {
                Text = request.Text,
                PostId = request.PostId
            };

            await _dbContext.Comments.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}