using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class FestividadReminderConfiguration : BaseEntityConfiguration<FestividadReminder>
    {
        public override void Configure(EntityTypeBuilder<FestividadReminder> builder)
        {
            base.Configure(builder);

            builder.ToTable("festividad_reminders", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.FestividadId).IsRequired();
            builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(false);

            builder.HasIndex(e => new { e.UserId, e.FestividadId })
                   .IsUnique()
                   .HasDatabaseName("ux_festividad_reminders_user_festividad")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Festividad>()
                   .WithMany()
                   .HasForeignKey(e => e.FestividadId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
