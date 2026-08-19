using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Common;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Common
{
    public class MasterCodeConfiguration : IEntityTypeConfiguration<MasterCode>
    {
        public void Configure(EntityTypeBuilder<MasterCode> builder)
        {
            builder.ToTable("master_codes", "common");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(e => e.Code).IsUnique();

            builder.Property(e => e.ParentCode)
                   .HasMaxLength(50);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Value)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            builder.Property(e => e.OrderIndex)
                   .IsRequired();

            builder.Property(e => e.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasOne<MasterCode>()
                   .WithMany()
                   .HasForeignKey(e => e.ParentCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
