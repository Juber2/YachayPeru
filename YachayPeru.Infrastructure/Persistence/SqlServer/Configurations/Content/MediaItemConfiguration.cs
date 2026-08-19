using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class MediaItemConfiguration : BaseEntityConfiguration<MediaItem>
    {
        public override void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            base.Configure(builder);

            builder.ToTable("media_items", "content");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
            builder.Property(e => e.MediaTypeCode).IsRequired().HasMaxLength(50);
            builder.Property(e => e.CourseId).IsRequired();
            builder.Property(e => e.ThumbnailUrl).HasMaxLength(2000);
            builder.Property(e => e.ExternalUrl).HasMaxLength(2000);
            builder.Property(e => e.LegendText).HasColumnType("nvarchar(max)");
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MasterCode>()
                   .WithMany()
                   .HasForeignKey(e => e.MediaTypeCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
