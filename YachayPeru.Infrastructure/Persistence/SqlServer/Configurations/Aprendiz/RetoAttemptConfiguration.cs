using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class RetoAttemptConfiguration : BaseEntityConfiguration<RetoAttempt>
    {
        public override void Configure(EntityTypeBuilder<RetoAttempt> builder)
        {
            base.Configure(builder);

            builder.ToTable("reto_attempts", "aprendiz");

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.RetoId).IsRequired();
            builder.Property(e => e.RetoVersionId).IsRequired();
            builder.Property(e => e.EarnedPoints).IsRequired().HasColumnType("decimal(6,2)");
            builder.Property(e => e.TotalPoints).IsRequired().HasColumnType("decimal(6,2)");
            builder.Property(e => e.Passed).IsRequired();
            builder.Property(e => e.CorrectCount).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.TotalQuestions).IsRequired().HasDefaultValue(0);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Reto>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<RetoVersion>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoVersionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
