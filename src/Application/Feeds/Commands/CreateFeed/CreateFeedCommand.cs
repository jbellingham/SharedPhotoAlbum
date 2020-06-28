using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed
{
    public class CreateFeedCommand : IRequest<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class CreateFeedCommandHandler : IRequestHandler<CreateFeedCommand, long>
    {
        private readonly IApplicationDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public CreateFeedCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<long> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
        {
            var feed = new Feed
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = _currentUser.UserId
            };

            await _db.Feeds.AddAsync(feed, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return feed.Id;
        }
    }
}
