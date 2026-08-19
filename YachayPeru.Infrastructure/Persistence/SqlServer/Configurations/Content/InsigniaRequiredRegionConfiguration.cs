using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class InsigniaRequiredRegionConfiguration : BaseEntityConfiguration<InsigniaRequiredRegion>
    {
        public override void Configure(EntityTypeBuilder<InsigniaRequiredRegion> builder)
        {
            base.Configure(builder);

            builder.ToTable("insignia_required_regions", "content");

            builder.Property(e => e.InsigniaId).IsRequired();
            builder.Property(e => e.CourseId).IsRequired();

            builder.HasOne<Insignia>()
                   .WithMany()
                   .HasForeignKey(e => e.InsigniaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
