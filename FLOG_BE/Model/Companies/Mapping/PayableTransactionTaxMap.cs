using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class PayableTransactionTaxMap : IEntityTypeConfiguration<Entities.PayableTransactionTax>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PayableTransactionTax> builder)
        {

            builder.ToTable("payable_transaction_tax", "dbo");
            builder.HasKey(t => t.TransactionTaxId);

            builder.Property(t => t.TransactionTaxId)
               .IsRequired()
               .HasColumnName("transaction_tax_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");


            builder.Property(t => t.PayableTransactionId)
                .HasColumnName("payable_transaction_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.TaxScheduleId)
                .HasColumnName("tax_schedule_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.IsTaxAfterDiscount)
                .HasColumnName("is_tax_after_discount")
                .HasColumnType("bit")
                .HasMaxLength(255);

            builder.Property(t => t.TaxScheduleCode)
                .HasColumnName("tax_schedule_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.TaxBasePercent)
                .HasColumnName("tax_base_percent")
                .HasColumnType("decimal");

            builder.Property(t => t.TaxBaseOriginatingAmount)
                .HasColumnName("tax_base_originating_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.TaxablePercent)
                .HasColumnName("taxable_percent")
                .HasColumnType("decimal");

            builder.Property(t => t.OriginatingTaxAmount)
              .HasColumnName("originating_tax_amount")
              .HasColumnType("decimal");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");
        }
    }
}
