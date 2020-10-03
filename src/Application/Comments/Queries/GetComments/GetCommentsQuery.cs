using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.Application.Comments.Queries.GetComments
{
    public class GetCommentsQuery : IRequest<CommentsVm>
    {
        public Guid PostId { get; set; }
    }

    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, CommentsVm>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public GetCommentsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<CommentsVm> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            return new CommentsVm
            {
                Comments = await _dbContext.Comments
                    .Where(_ => _.PostId == request.PostId)
                    .OrderBy(_ => _.Created)
                    .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken) 
            };
        }
    }
}