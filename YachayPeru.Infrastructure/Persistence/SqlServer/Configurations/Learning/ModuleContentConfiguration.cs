using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Learning
{
    public class ModuleContentConfiguration : BaseEntityConfiguration<ModuleContent>
    {
        public override void Configure(EntityTypeBuilder<ModuleContent> builder)
        {
            base.Configure(builder);

            builder.ToTable("module_contents", "course");

            builder.Property(e => e.ModuleId).IsRequired();
            builder.Property(e => e.Text).HasColumnType("nvarchar(max)");
            builder.Property(e => e.DesignJson).HasColumnType("nvarchar(max)");
            builder.Property(e => e.OrderIndex).IsRequired();

            builder.HasOne<CourseModule>()
                   .WithMany()
                   .HasForeignKey(e => e.ModuleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
