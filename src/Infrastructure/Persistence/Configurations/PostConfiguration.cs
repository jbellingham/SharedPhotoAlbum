using SharedPhotoAlbum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(t => t.Text)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(_ => _.FeedId)
                .IsRequired();
        }
    }
}
