using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class AprendizProfileConfiguration : BaseEntityConfiguration<AprendizProfile>
    {
        public override void Configure(EntityTypeBuilder<AprendizProfile> builder)
        {
            base.Configure(builder);

            builder.ToTable("aprendiz_profiles", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.AvatarUrl).HasMaxLength(2000);
            builder.Property(e => e.Points).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.Level).IsRequired().HasDefaultValue(1);
            builder.Property(e => e.IsPremiumUser).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.ModulesDone).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.LearningTimeMinutes).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CurrentLoginStreakDays).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.BestLoginStreakDays).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CurrentAnswerStreak).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.BestAnswerStreak).IsRequired().HasDefaultValue(0);

            builder.HasIndex(e => e.UserId)
                   .IsUnique()
                   .HasDatabaseName("ux_aprendiz_profiles_user")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.FavoriteRegionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
