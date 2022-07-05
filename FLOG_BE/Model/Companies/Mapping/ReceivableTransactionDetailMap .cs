using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ReceivableTransactionDetailMap : IEntityTypeConfiguration<Entities.ReceivableTransactionDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ReceivableTransactionDetail> builder)
        {
            builder.ToTable("receive_transaction_detail", "dbo");
            builder.HasKey(t => t.TransactionDetailId);

            builder.Property(t => t.TransactionDetailId)
                .IsRequired()
                .HasColumnName("transaction_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ReceiveTransactionId)
                .HasColumnName("receive_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ChargesId)
                .HasColumnName("charges_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ChargesDescription)
                .HasColumnName("charges_description")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

            builder.Property(t => t.OriginatingAmount)
                .HasColumnName("originating_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.OriginatingTax)
                .HasColumnName("originating_tax")
                .HasColumnType("decimal");

            builder.Property(t => t.OriginatingDiscount)
                .HasColumnName("originating_discount")
                .HasColumnType("decimal");

            builder.Property(t => t.OriginatingExtendedAmount)
                .HasColumnName("originating_extended_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalTax)
                .HasColumnName("functional_tax")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalDiscount)
                .HasColumnName("functional_discount")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalExtendedAmount)
                .HasColumnName("functional_extended_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");
          
            builder.Property(t => t.TaxScheduleId)
               .HasColumnName("tax_schedule_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.IsTaxAfterDiscount)
               .HasColumnName("is_tax_after_discount")
               .HasColumnType("bit");

            builder.Property(t => t.PercentDiscount)
               .HasColumnName("percent_discount")
               .HasColumnType("decimal");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                            .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

        }
    }
}
