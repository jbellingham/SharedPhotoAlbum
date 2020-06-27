using MediatR;

namespace SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed
{
    public class CreateFeedCommand : IRequest<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
