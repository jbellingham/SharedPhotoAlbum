using SharedPhotoAlbum.Application.Common.Interfaces;
using System;

namespace SharedPhotoAlbum.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
