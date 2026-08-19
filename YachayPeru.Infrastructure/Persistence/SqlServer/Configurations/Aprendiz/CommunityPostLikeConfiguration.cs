using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Aprendiz
{
    public class CommunityPostLikeConfiguration : BaseEntityConfiguration<CommunityPostLike>
    {
        public override void Configure(EntityTypeBuilder<CommunityPostLike> builder)
        {
            base.Configure(builder);

            builder.ToTable("community_post_likes", "aprendiz");

            builder.Property(e => e.PostId).IsRequired();
            builder.Property(e => e.UserId).IsRequired();

            builder.HasIndex(e => new { e.PostId, e.UserId })
                   .IsUnique()
                   .HasDatabaseName("ux_community_post_likes_post_user")
                   .HasFilter("[Deleted] = 0");

            builder.HasOne<CommunityPost>()
                   .WithMany()
                   .HasForeignKey(e => e.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
