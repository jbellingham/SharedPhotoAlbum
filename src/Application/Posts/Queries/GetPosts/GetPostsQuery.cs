using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedPhotoAlbum.Application.Comments.Queries.GetComments;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.Application.Posts.Queries.GetPosts
{
    public class GetPostsQuery : IRequest<PostsVm>
    {
        public Guid? FeedId { get; set; }
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
                    .Where(_ => _.FeedId == request.FeedId)
                    .OrderByDescending(_ => _.Created)
                    .Select(_ => new PostDto
                    {
                        Id = _.Id,
                        LinkUrl = _.LinkUrl,
                        Text = _.Text,
                        Comments = _.Comments.OrderBy(c => c.Created)
                            .Select(c => new CommentDto
                            {
                                Id = c.Id,
                                Likes = c.Likes,
                                Text = c.Text,
                                PostId = c.PostId
                            })
                            .ToList(),
                        StoredMedia = _.StoredMedia.Select(s => new StoredMediaDto
                        {
                            Id = s.Id,
                            Content = s.Content,
                            MediaType = s.MediaType,
                            Name = s.Name,
                            PostId = s.PostId
                        }).ToList()
                    }).ToListAsync(cancellationToken: cancellationToken)
            };
        }
    }
}