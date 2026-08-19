using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class PremiumBenefitConfiguration : BaseEntityConfiguration<PremiumBenefit>
    {
        public override void Configure(EntityTypeBuilder<PremiumBenefit> builder)
        {
            base.Configure(builder);

            builder.ToTable("premium_benefits", "content");

            builder.Property(e => e.Code).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Label).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).HasMaxLength(500);
        }
    }
}
