using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Assessment
{
    public class RetoVersionQuestionBlankConfiguration : BaseEntityConfiguration<RetoVersionQuestionBlank>
    {
        public override void Configure(EntityTypeBuilder<RetoVersionQuestionBlank> builder)
        {
            base.Configure(builder);

            builder.ToTable("reto_version_question_blanks", "assessment");

            builder.Property(e => e.RetoVersionQuestionId).IsRequired();
            builder.Property(e => e.BlankIndex).IsRequired();
            builder.Property(e => e.CorrectAnswer).IsRequired().HasMaxLength(500);
            builder.Property(e => e.OrderIndex).IsRequired();

            builder.HasOne<RetoVersionQuestion>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoVersionQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
