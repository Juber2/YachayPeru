using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class NoticiaConfiguration : BaseEntityConfiguration<Noticia>
    {
        public override void Configure(EntityTypeBuilder<Noticia> builder)
        {
            base.Configure(builder);

            builder.ToTable("noticias", "content");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
            builder.Property(e => e.Category).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Body).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(e => e.ImageUrl).HasMaxLength(2000);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        }
    }
}
