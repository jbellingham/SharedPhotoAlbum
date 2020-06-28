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

        public CreateFeedCommandHandler(IApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<long> Handle(CreateFeedCommand request, CancellationToken cancellationToken)
        {
            var feed = new Feed
            {
                Name = request.Name,
                Description = request.Description
            };

            await _db.Feeds.AddAsync(feed, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return feed.Id;
        }
    }
}
