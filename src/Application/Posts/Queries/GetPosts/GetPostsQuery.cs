using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class GetPostsQuery : IRequest<PostsVm>
    {
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PostsVm>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public GetPostsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<PostsVm> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            return new PostsVm
            {
                Posts = await _dbContext.Posts
                    .OrderByDescending(_ => _.Created)
                    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}