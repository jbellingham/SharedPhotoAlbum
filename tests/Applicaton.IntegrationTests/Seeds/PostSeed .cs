using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Seeds
{
    public static class PostSeed
    {
        public static async Task Seed(IApplicationDbContext db, Guid userId)
        {
            var feed = await db.Feeds.FirstOrDefaultAsync(_ => _.OwnerId == userId);
            await db.Posts.AddRangeAsync(new List<Post>
            {
                new Post
                {
                    Text = "poop",
                    FeedId = feed.Id
                }
            });
            await db.SaveChangesAsync(CancellationToken.None);
        }
    }
}