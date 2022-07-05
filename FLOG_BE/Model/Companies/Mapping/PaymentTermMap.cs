using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class PaymentTermMap : IEntityTypeConfiguration<Entities.PaymentTerm>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PaymentTerm> builder)
        {
            //payment_term_id, payment_term_code, payment_term_desc, due, unit
            builder.ToTable("payment_term", "dbo");

            builder.HasKey(t => t.PaymentTermId);

            builder.Property(t => t.PaymentTermId)
               .IsRequired()
               .HasColumnName("payment_term_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.PaymentTermCode)
                .IsRequired()
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.PaymentTermDesc)
                .IsRequired()
                .HasColumnName("payment_term_desc")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);


            builder.Property(t => t.Due)
                .IsRequired()
                .HasColumnName("due")
                .HasColumnType("int");

            builder.Property(t => t.Unit)
                .IsRequired()
                .HasColumnName("unit")
                .HasColumnType("int");

            builder.Property(t => t.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("datetime");

            builder.Property(t => t.ModifiedBy)
                .HasColumnName("modified_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ModifiedDate)
                .HasColumnName("modified_date")
                .HasColumnType("datetime");

        }

    }
}
