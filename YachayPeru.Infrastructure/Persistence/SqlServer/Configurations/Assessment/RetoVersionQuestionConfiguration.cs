using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Common;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Assessment
{
    public class RetoVersionQuestionConfiguration : BaseEntityConfiguration<RetoVersionQuestion>
    {
        public override void Configure(EntityTypeBuilder<RetoVersionQuestion> builder)
        {
            base.Configure(builder);

            builder.ToTable("reto_version_questions", "assessment");

            builder.Property(e => e.RetoVersionId).IsRequired();
            builder.Property(e => e.QuestionTypeCode).IsRequired().HasMaxLength(50);
            builder.Property(e => e.QuestionText).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(e => e.Points).IsRequired().HasColumnType("decimal(6,2)");
            builder.Property(e => e.OrderIndex).IsRequired();

            builder.HasOne<RetoVersion>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoVersionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MasterCode>()
                   .WithMany()
                   .HasForeignKey(e => e.QuestionTypeCode)
                   .HasPrincipalKey(mc => mc.Code)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
