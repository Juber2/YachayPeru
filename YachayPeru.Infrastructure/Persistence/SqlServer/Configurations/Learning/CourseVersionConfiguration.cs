using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Learning
{
    public class CourseVersionConfiguration : BaseEntityConfiguration<CourseVersion>
    {
        public override void Configure(EntityTypeBuilder<CourseVersion> builder)
        {
            base.Configure(builder);

            builder.ToTable("course_versions", "course");

            builder.Property(e => e.CourseId).IsRequired();
            builder.Property(e => e.VersionNumber).IsRequired();
            builder.Property(e => e.StatusCode).IsRequired().HasMaxLength(50);
            builder.Property(e => e.DurationHours).HasColumnType("decimal(6,2)");
            builder.Property(e => e.IsCurrent).IsRequired().HasDefaultValue(false).HasColumnName("is_current");
            builder.Property(e => e.PublishedAt);
            builder.Property(e => e.PublishedBy);

            // Máximo una version is_current=true por curso (filtrado: solo activos)
            builder.HasIndex(e => e.CourseId)
                   .IsUnique()
                   .HasDatabaseName("ux_course_versions_is_current")
                   .HasFilter("[is_current] = 1 AND [Deleted] = 0");

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MasterCode>()
                   .WithMany()
                   .HasForeignKey(e => e.StatusCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
