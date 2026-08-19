using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class PremiumPlanConfiguration : BaseEntityConfiguration<PremiumPlan>
    {
        public override void Configure(EntityTypeBuilder<PremiumPlan> builder)
        {
            base.Configure(builder);

            builder.ToTable("premium_plans", "content");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        }
    }
}
