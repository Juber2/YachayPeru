using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class AprendizActivityLogConfiguration : BaseEntityConfiguration<AprendizActivityLog>
    {
        public override void Configure(EntityTypeBuilder<AprendizActivityLog> builder)
        {
            base.Configure(builder);

            builder.ToTable("aprendiz_activity_logs", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Text).IsRequired().HasMaxLength(500);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.RegionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
