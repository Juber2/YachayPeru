using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Common;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Assessment
{
    public class RetoVersionConfiguration : BaseEntityConfiguration<RetoVersion>
    {
        public override void Configure(EntityTypeBuilder<RetoVersion> builder)
        {
            base.Configure(builder);

            builder.ToTable("reto_versions", "assessment");

            builder.Property(e => e.RetoId).IsRequired();
            builder.Property(e => e.VersionNumber).IsRequired();
            builder.Property(e => e.StatusCode).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
            builder.Property(e => e.PassingScore).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(e => e.TimeLimitMinutes);
            builder.Property(e => e.MaxAttempts).IsRequired().HasDefaultValue(3);
            builder.Property(e => e.ShuffleQuestionOrder).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.ShuffleOptionOrder).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.ShowResultsAtEnd).IsRequired().HasDefaultValue(true);

            builder.HasOne<Reto>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MasterCode>()
                   .WithMany()
                   .HasForeignKey(e => e.StatusCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
