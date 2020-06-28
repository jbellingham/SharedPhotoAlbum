using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Configurations
{
    public class FeedConfiguration : IEntityTypeConfiguration<Feed>
    {
        public void Configure(EntityTypeBuilder<Feed> builder)
        {
            builder.Property(_ => _.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(_ => _.Description)
                .HasMaxLength(500);

            builder.Property(_ => _.OwnerId)
                .IsRequired();
        }
    }
}
