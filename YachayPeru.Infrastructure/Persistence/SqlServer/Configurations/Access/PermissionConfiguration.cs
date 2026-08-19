using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Access;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Access
{
    public class PermissionConfiguration : BaseEntityConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);

            builder.ToTable("permissions", "access");

            builder.Property(e => e.ResourceId).IsRequired();

            builder.Property(e => e.PermissionCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(e => e.Resource)
                   .WithMany()
                   .HasForeignKey(e => e.ResourceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PermissionMasterCode)
                   .WithMany()
                   .HasForeignKey(e => e.PermissionCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ResourceId, e.PermissionCode }).IsUnique().HasFilter("[Deleted] = 0");
        }
    }
}
