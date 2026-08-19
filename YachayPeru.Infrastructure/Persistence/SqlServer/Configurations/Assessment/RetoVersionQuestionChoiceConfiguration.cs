using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Assessment
{
    public class RetoVersionQuestionChoiceConfiguration : BaseEntityConfiguration<RetoVersionQuestionChoice>
    {
        public override void Configure(EntityTypeBuilder<RetoVersionQuestionChoice> builder)
        {
            base.Configure(builder);

            builder.ToTable("reto_version_question_choices", "assessment");

            builder.Property(e => e.RetoVersionQuestionId).IsRequired();
            builder.Property(e => e.Text).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(e => e.IsCorrect).IsRequired();
            builder.Property(e => e.OrderIndex).IsRequired();

            builder.HasOne<RetoVersionQuestion>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoVersionQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
