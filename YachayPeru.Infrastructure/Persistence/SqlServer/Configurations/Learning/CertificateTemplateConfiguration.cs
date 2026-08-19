using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Learning
{
    public class CertificateTemplateConfiguration : BaseEntityConfiguration<CertificateTemplate>
    {
        public override void Configure(EntityTypeBuilder<CertificateTemplate> builder)
        {
            base.Configure(builder);

            builder.ToTable("certificate_templates", "course");

            builder.Property(e => e.RetoId).IsRequired();
            builder.Property(e => e.MainTitle).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Subtitle).HasMaxLength(300);
            builder.Property(e => e.BodyText).HasColumnType("nvarchar(max)");
            builder.Property(e => e.FooterText).HasColumnType("nvarchar(max)");

            builder.Property(e => e.Orientation).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Prefix).HasMaxLength(50);

            builder.Property(e => e.PrimaryColor).IsRequired().HasMaxLength(20);
            builder.Property(e => e.SecondaryColor).IsRequired().HasMaxLength(20);
            builder.Property(e => e.AccentColor).IsRequired().HasMaxLength(20);

            builder.Property(e => e.FontFamily).IsRequired().HasMaxLength(20);
            builder.Property(e => e.BorderStyle).IsRequired().HasMaxLength(20);
            builder.Property(e => e.BorderWidth).IsRequired().HasMaxLength(20);

            builder.Property(e => e.LogoUrl).HasMaxLength(2000);
            builder.Property(e => e.SignerName).HasMaxLength(200);
            builder.Property(e => e.SignerTitle).HasMaxLength(200);
            builder.Property(e => e.SignatureUrl).HasMaxLength(2000);
            builder.Property(e => e.SealUrl).HasMaxLength(2000);

            builder.HasIndex(e => e.RetoId)
                   .IsUnique()
                   .HasDatabaseName("ux_certificate_templates_reto")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne<Reto>()
                   .WithMany()
                   .HasForeignKey(e => e.RetoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
