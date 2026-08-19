using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Assessment
{
    public class RetoConfiguration : BaseEntityConfiguration<Reto>
    {
        public override void Configure(EntityTypeBuilder<Reto> builder)
        {
            base.Configure(builder);

            builder.ToTable("retos", "assessment");

            builder.Property(e => e.CourseId).IsRequired();

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
