using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Configurations
{
    public class StoredMediaConfiguration : IEntityTypeConfiguration<StoredMedia>
    {
        public void Configure(EntityTypeBuilder<StoredMedia> builder)
        {
            builder.OwnsOne<File>(_ => _.File);
            // builder.Property(m => m.File.DataUrl)
            //     .IsRequired();
            //
            // builder.Property(m => m.File.FileType)
            //     .IsRequired();
            //
            // builder.Property(m => m.File.MimeType)
            //     .IsRequired();
        }
    }
}