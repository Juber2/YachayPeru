using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YachayPeru.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Configurations.Common
{
    public class PersonConfiguration : BaseEntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder.ToTable("persons", "common");

            builder.Property(e => e.FirstName)
                   .HasMaxLength(150);

            builder.Property(e => e.LastName)
                   .HasMaxLength(150);

            builder.Property(e => e.DocumentType)
                   .HasMaxLength(50);

            builder.Property(e => e.DocumentNumber)
                   .HasMaxLength(50);

            builder.Property(e => e.BirthDate);

            builder.Property(e => e.BusinessName)
                   .HasMaxLength(200);

            builder.Property(e => e.Ruc)
                   .HasMaxLength(30);

            builder.Property(e => e.LegalRepresentative)
                   .HasMaxLength(200);

            builder.Property(e => e.Phone)
                   .HasMaxLength(20);

            builder.Property(e => e.Address)
                   .HasMaxLength(250);

            builder.Property(e => e.City)
                   .HasMaxLength(100);

            builder.HasIndex(e => e.DocumentNumber)
                   .IsUnique()
                   .HasFilter("[DocumentNumber] IS NOT NULL AND [Deleted] = 0");

            builder.HasIndex(e => e.Ruc)
                   .IsUnique()
                   .HasFilter("[Ruc] IS NOT NULL AND [Deleted] = 0");
        }
    }
}
