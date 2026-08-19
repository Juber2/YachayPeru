using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class PremiumWaitlistEntryConfiguration : BaseEntityConfiguration<PremiumWaitlistEntry>
    {
        public override void Configure(EntityTypeBuilder<PremiumWaitlistEntry> builder)
        {
            base.Configure(builder);

            builder.ToTable("premium_waitlist_entries", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.PlanId).IsRequired();
            builder.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(30);
            builder.Property(e => e.ReceiptUrl).HasMaxLength(500);
            builder.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("pending");
            builder.Property(e => e.RejectionReason).HasMaxLength(500);
            builder.Property(e => e.ReviewSeen).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.JoinedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasIndex(e => e.UserId)
                   .IsUnique()
                   .HasDatabaseName("ux_premium_waitlist_entries_user")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<PremiumPlan>()
                   .WithMany()
                   .HasForeignKey(e => e.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
