using System.Collections.Generic;

namespace SharedPhotoAlbum.Application.Comments.Queries.GetComments
{
    public class CommentsVm
    {
        public IList<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}