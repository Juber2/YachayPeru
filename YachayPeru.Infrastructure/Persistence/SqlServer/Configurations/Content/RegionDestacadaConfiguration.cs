using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class RegionDestacadaConfiguration : BaseEntityConfiguration<RegionDestacada>
    {
        public override void Configure(EntityTypeBuilder<RegionDestacada> builder)
        {
            base.Configure(builder);

            builder.ToTable("regiones_destacadas", "content");

            builder.Property(e => e.CourseId).IsRequired();
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
