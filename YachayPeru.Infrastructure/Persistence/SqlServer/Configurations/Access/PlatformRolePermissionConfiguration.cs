using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Access;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Access
{
    public class PlatformRolePermissionConfiguration : BaseEntityConfiguration<PlatformRolePermission>
    {
        public override void Configure(EntityTypeBuilder<PlatformRolePermission> builder)
        {
            base.Configure(builder);

            builder.ToTable("platform_role_permissions", "access");

            builder.Property(e => e.PlatformRoleId).IsRequired();
            builder.Property(e => e.PermissionId).IsRequired();

            builder.HasOne(e => e.PlatformRole)
                   .WithMany()
                   .HasForeignKey(e => e.PlatformRoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Permission)
                   .WithMany()
                   .HasForeignKey(e => e.PermissionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.PlatformRoleId, e.PermissionId }).IsUnique().HasFilter("[Deleted] = 0");
        }
    }
}
