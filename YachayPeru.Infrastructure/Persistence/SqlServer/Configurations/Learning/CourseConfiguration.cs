using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Learning
{
    public class CourseConfiguration : BaseEntityConfiguration<Course>
    {
        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            base.Configure(builder);

            builder.ToTable("courses", "course");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
            builder.Property(e => e.Description).HasMaxLength(2000);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.SourceTemplateId);
            builder.Property(e => e.CoverImageUrl).HasMaxLength(2000);
            builder.Property(e => e.ZoneCode).HasMaxLength(20);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.SourceTemplateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
