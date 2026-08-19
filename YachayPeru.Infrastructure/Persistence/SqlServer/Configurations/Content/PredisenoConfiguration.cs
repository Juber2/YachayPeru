using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Content
{
    public class PredisenoConfiguration : BaseEntityConfiguration<Prediseno>
    {
        public override void Configure(EntityTypeBuilder<Prediseno> builder)
        {
            base.Configure(builder);

            builder.ToTable("predisenos", "content");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
            builder.Property(e => e.TreeJson).IsRequired().HasColumnType("nvarchar(max)");
        }
    }
}
