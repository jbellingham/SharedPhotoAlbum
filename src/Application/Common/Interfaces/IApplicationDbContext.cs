using SharedPhotoAlbum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SharedPhotoAlbum.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Post> Posts { get; set; }
        
        DbSet<Comment> Comments { get; set; }
        
        DbSet<StoredMedia> StoredMedia { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
