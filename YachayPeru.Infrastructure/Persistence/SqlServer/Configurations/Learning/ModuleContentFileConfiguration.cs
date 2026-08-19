using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Learning
{
    public class ModuleContentFileConfiguration : BaseEntityConfiguration<ModuleContentFile>
    {
        public override void Configure(EntityTypeBuilder<ModuleContentFile> builder)
        {
            base.Configure(builder);

            builder.ToTable("module_content_files", "course");

            builder.Property(e => e.ModuleContentId).IsRequired();
            builder.Property(e => e.FileTypeCode).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FileUrl).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            builder.Property(e => e.OrderIndex).IsRequired();

            builder.HasOne<ModuleContent>()
                   .WithMany()
                   .HasForeignKey(e => e.ModuleContentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MasterCode>()
                   .WithMany()
                   .HasForeignKey(e => e.FileTypeCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
