using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Access;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Access
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("resources", "access");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.Code)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Scope)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasDefaultValue(AppConstants.ResourceScope.Platform);

            builder.HasIndex(e => e.Code).IsUnique();
        }
    }
}
