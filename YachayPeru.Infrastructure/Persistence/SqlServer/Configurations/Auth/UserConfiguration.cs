using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Access;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Infrastructure.Persistence.SqlServer.Configurations;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Auth
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("users", "auth");

            builder.Property(e => e.PersonId).IsRequired();

            builder.Property(e => e.UserTypeCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Username).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Password).IsRequired().HasMaxLength(255);
            builder.Property(e => e.LockedUntil);
            builder.Property(e => e.IsLocked).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.LockedReason).HasMaxLength(500);
            builder.Property(e => e.Email).HasMaxLength(200);
            builder.Property(e => e.RoleId);

            builder.HasIndex(e => e.Email).IsUnique().HasDatabaseName("ux_users_email").HasFilter("[Email] IS NOT NULL AND [Deleted] = 0");
            builder.HasIndex(e => e.Username).IsUnique().HasDatabaseName("ux_users_username").HasFilter("[Deleted] = 0");

            // FK: UserTypeCode → master_codes.code
            builder.HasOne(e => e.UserType)
                   .WithMany()
                   .HasForeignKey(e => e.UserTypeCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Person)
                   .WithMany()
                   .HasForeignKey(e => e.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK: RoleId → platform_roles.id (un usuario tiene un solo rol)
            builder.HasOne<PlatformRole>()
                   .WithMany()
                   .HasForeignKey(e => e.RoleId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
