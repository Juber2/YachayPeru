using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class InsigniaRequiredRetoConfiguration : BaseEntityConfiguration<InsigniaRequiredReto>
    {
        public override void Configure(EntityTypeBuilder<InsigniaRequiredReto> builder)
        {
            base.Configure(builder);

            builder.ToTable("insignia_required_retos", "content");

            builder.Property(e => e.InsigniaId).IsRequired();
            builder.Property(e => e.RetoId).IsRequired();

            builder.HasOne<Insignia>()
                   .WithMany()
                   .HasForeignKey(e => e.InsigniaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Reto>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
