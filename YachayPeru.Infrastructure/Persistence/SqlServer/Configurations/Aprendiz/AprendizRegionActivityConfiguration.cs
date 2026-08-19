using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class AprendizRegionActivityConfiguration : BaseEntityConfiguration<AprendizRegionActivity>
    {
        public override void Configure(EntityTypeBuilder<AprendizRegionActivity> builder)
        {
            base.Configure(builder);

            builder.ToTable("aprendiz_region_activities", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.RegionId).IsRequired();
            builder.Property(e => e.ModuleId).IsRequired();
            builder.Property(e => e.ViewedAt).IsRequired();

            builder.HasIndex(e => e.UserId).IsUnique().HasFilter("[Deleted] = 0");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.RegionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<CourseModule>()
                   .WithMany()
                   .HasForeignKey(e => e.ModuleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
