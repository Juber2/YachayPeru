using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class InsigniaConfiguration : BaseEntityConfiguration<Insignia>
    {
        public override void Configure(EntityTypeBuilder<Insignia> builder)
        {
            base.Configure(builder);

            builder.ToTable("insignias", "content");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.ImageUrl).HasMaxLength(2000);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.MinPoints);
            builder.Property(e => e.MinRetosCompleted);
            builder.Property(e => e.MinPerfectRetos);
            builder.Property(e => e.RequireAllQuestionTypes).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.MinLevel);
            builder.Property(e => e.RequirePremium).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.MinLoginStreakDays);
            builder.Property(e => e.MinAnswerStreak);
            builder.Property(e => e.MinRegionsExplored);
            builder.Property(e => e.RequiredZoneCode).HasMaxLength(20);
            builder.Property(e => e.MinZoneRegionsExplored);
        }
    }
}
