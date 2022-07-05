using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class SalesPersonMap : IEntityTypeConfiguration<Entities.SalesPerson>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SalesPerson> builder)
        {
            builder.ToTable("sales_person", "dbo");

            builder.HasKey(t => t.SalesPersonId);

            builder.Property(t => t.SalesPersonId)
                .IsRequired()
                .HasColumnName("sales_person_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.SalesCode)
                .HasColumnName("sales_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.SalesName)
                .HasColumnName("sales_name")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.PersonId)
                .HasColumnName("person_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);
        }
    }
}
