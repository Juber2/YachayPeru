using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class FestividadConfiguration : BaseEntityConfiguration<Festividad>
    {
        public override void Configure(EntityTypeBuilder<Festividad> builder)
        {
            base.Configure(builder);

            builder.ToTable("festividades", "content");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).HasMaxLength(2000);
            builder.Property(e => e.CourseId).IsRequired();
            builder.Property(e => e.Month).IsRequired();
            builder.Property(e => e.Day).IsRequired();
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
