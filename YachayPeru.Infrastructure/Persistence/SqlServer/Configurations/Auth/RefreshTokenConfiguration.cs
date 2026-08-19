using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Auth
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens", "auth");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Token).IsRequired().HasMaxLength(200);
            builder.Property(e => e.ExpiresAt).IsRequired();
            builder.Property(e => e.IsRevoked).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            // Rotation
            builder.Property(e => e.ReplacedByToken).HasMaxLength(200);

            // Contexto del login
            builder.Property(e => e.LoginIp).HasMaxLength(50);
            builder.Property(e => e.LoginUserAgent).HasMaxLength(500);

            // Aprobación de sesión sospechosa
            builder.Property(e => e.IsPendingApproval).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.ApprovalToken).HasMaxLength(100);
            builder.Property(e => e.ApprovalExpiresAt);
            builder.Property(e => e.SuspiciousIp).HasMaxLength(50);

            builder.HasIndex(e => e.Token).IsUnique();
            builder.HasIndex(e => e.ApprovalToken);

            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
