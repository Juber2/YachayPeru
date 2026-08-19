using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Learning
{
    public class CourseModuleConfiguration : BaseEntityConfiguration<CourseModule>
    {
        public override void Configure(EntityTypeBuilder<CourseModule> builder)
        {
            base.Configure(builder);

            builder.ToTable("course_modules", "course");

            builder.Property(e => e.CourseVersionId).IsRequired();
            builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.OrderIndex).IsRequired();
            builder.Property(e => e.DurationHours).HasColumnType("decimal(6,2)");

            builder.HasOne<CourseVersion>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseVersionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
