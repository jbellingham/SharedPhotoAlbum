﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.Posts.Queries.GetPosts;

namespace SharedPhotoAlbum.Application.Feeds.Queries.GetFeed
{
    public class GetFeedQuery : IRequest<FeedVm>
    {
        public long? FeedId { get; set; }
    }

    public class GetFeedCommandHandler : IRequestHandler<GetFeedQuery, FeedVm>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetFeedCommandHandler(IApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<FeedVm> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            return new FeedVm
            {
                Posts = await _db.Posts
                    .OrderByDescending(_ => _.Created)
                    .Where(_ => request.FeedId == null ||
                                _.FeedId == request.FeedId)
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
