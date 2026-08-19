using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Auth
{
    public class UserPasswordChangeConfiguration : IEntityTypeConfiguration<UserPasswordChange>
    {
        public  void Configure(EntityTypeBuilder<UserPasswordChange> builder)
        {
            builder.ToTable("user_password_changes", "auth");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Password).IsRequired().HasMaxLength(200);
            builder.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

        }

    }
}
