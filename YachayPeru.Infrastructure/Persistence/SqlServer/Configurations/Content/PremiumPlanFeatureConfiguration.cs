using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class PremiumPlanFeatureConfiguration : BaseEntityConfiguration<PremiumPlanFeature>
    {
        public override void Configure(EntityTypeBuilder<PremiumPlanFeature> builder)
        {
            base.Configure(builder);

            builder.ToTable("premium_plan_features", "content");

            builder.Property(e => e.PlanId).IsRequired();
            builder.Property(e => e.BenefitId).IsRequired();

            builder.HasIndex(e => new { e.PlanId, e.BenefitId })
                   .IsUnique()
                   .HasDatabaseName("ux_premium_plan_features_plan_benefit")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne(e => e.Plan)
                   .WithMany()
                   .HasForeignKey(e => e.PlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Benefit)
                   .WithMany()
                   .HasForeignKey(e => e.BenefitId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
