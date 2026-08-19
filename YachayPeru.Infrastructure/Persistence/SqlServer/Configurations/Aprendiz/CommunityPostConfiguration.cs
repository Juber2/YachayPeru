using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class CommunityPostConfiguration : BaseEntityConfiguration<CommunityPost>
    {
        public override void Configure(EntityTypeBuilder<CommunityPost> builder)
        {
            base.Configure(builder);

            builder.ToTable("community_posts", "aprendiz");

            builder.Property(e => e.AuthorName).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Text).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(e => e.PhotoUrl).HasMaxLength(2000);

            builder.HasOne<Course>()
                   .WithMany()
                   .HasForeignKey(e => e.RegionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
