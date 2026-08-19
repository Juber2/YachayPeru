using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class AprendizRegionExploredConfiguration : BaseEntityConfiguration<AprendizRegionExplored>
    {
        public override void Configure(EntityTypeBuilder<AprendizRegionExplored> builder)
        {
            base.Configure(builder);

            builder.ToTable("aprendiz_region_explored", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.RegionId).IsRequired();
            builder.Property(e => e.FirstViewedAt).IsRequired();

            builder.HasIndex(e => new { e.UserId, e.RegionId })
                   .IsUnique()
                   .HasDatabaseName("ux_aprendiz_region_explored_user_region")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.RegionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
