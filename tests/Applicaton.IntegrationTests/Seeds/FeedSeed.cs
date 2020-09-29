using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Seeds
{
    public static class FeedSeed
    {
        public static Dictionary<Guid, Feed> UserFeeds = new Dictionary<Guid, Feed>();

        public static async Task Seed(IApplicationDbContext db, Guid userId)
        {
            var feed = new Feed
            {
                OwnerId = userId,
                Name = "poop",
                Description = "poop"
            };
            UserFeeds.Add(userId, feed);
            await db.Feeds.AddRangeAsync(new List<Feed>
            {
                feed
            });
            await db.SaveChangesAsync(CancellationToken.None);
        }
    }
}