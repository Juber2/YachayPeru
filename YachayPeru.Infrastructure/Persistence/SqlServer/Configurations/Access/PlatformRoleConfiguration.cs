using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Access;
using YachayPeru.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Access
{
    public class PlatformRoleConfiguration : BaseEntityConfiguration<PlatformRole>
    {
        public override void Configure(EntityTypeBuilder<PlatformRole> builder)
        {
            base.Configure(builder);

            builder.ToTable("platform_roles", "access");

            builder.Property(e => e.RoleCode)
                   .HasMaxLength(50);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            builder.HasIndex(e => e.RoleCode)
                   .IsUnique()
                   .HasFilter("[RoleCode] IS NOT NULL AND [Deleted] = 0");

        }
    }
}
