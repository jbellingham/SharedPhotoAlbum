using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Configurations
{
    public class StoredMediaConfiguration : IEntityTypeConfiguration<StoredMedia>
    {
        public void Configure(EntityTypeBuilder<StoredMedia> builder)
        {
            builder.Property(m => m.Content)
                .IsRequired();
        }
    }
}