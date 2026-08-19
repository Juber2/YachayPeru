using YachayPeru.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.UpdatedAt);
            builder.Property(e => e.UpdatedBy);
            builder.Property(e => e.Deleted).IsRequired().HasDefaultValue(false);
            builder.HasQueryFilter(e => !e.Deleted);
        }
    }
}
