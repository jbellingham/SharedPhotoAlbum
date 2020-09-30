using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.Application.Feeds.Queries.GetFeed
{
    public class GetFeedQuery : IRequest<FeedVm>
    {
        public Guid? FeedId { get; set; }
    }

    public class GetFeedCommandHandler : IRequestHandler<GetFeedQuery, FeedVm>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetFeedCommandHandler(IApplicationDbContext db, IMapper mapper, ICurrentUserService currentUserService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<FeedVm> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            var feeds = await _db.Feeds.OrderByDescending(_ => _.CreatedBy)
                    .Where(_ => !request.FeedId.HasValue && _.OwnerId == _currentUserService.UserId ||
                                _.Id == request.FeedId)
                    .ToListAsync(cancellationToken);
            

            return new FeedVm
            {
                Feeds = feeds.Select(feed => _mapper.Map<FeedDto>(feed)).ToList()
            };
        }
    }
}
