using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ArReceiptDetailMap : IEntityTypeConfiguration<Entities.ArReceiptDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ArReceiptDetail> builder)
        {
            builder.ToTable("ar_receipt_detail", "dbo");

            builder.HasKey(t => t.ReceiptDetailId);

            builder.Property(t => t.ReceiptDetailId)
                .IsRequired()
                .HasColumnName("receipt_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.ReceiptHeaderId)
                .HasColumnName("receipt_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ReceiveTransactionId)
                .HasColumnName("receive_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ReceivableApplyDetailId)
                .HasColumnName("receivable_apply_detail_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

            builder.Property(t => t.OriginatingBalance)
                .HasColumnName("originating_balance")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalBalance)
                .HasColumnName("functional_balance")
                .HasColumnType("decimal");

            builder.Property(t => t.OriginatingPaid)
                .HasColumnName("originating_paid")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalPaid)
                .HasColumnName("functional_paid")
                .HasColumnType("decimal");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");
        }
    }
}
