using System;

namespace SharedPhotoAlbum.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
