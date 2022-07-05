using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class BankReconcileAdjustmentMap : IEntityTypeConfiguration<Entities.BankReconcileAdjustment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.BankReconcileAdjustment> builder)
        {
            builder.ToTable("bank_reconcile_adjustment", "dbo");

            builder.HasKey(t => t.BankReconcileAdjustmentId);

            builder.Property(t => t.BankReconcileAdjustmentId)
                .IsRequired()
                .HasColumnName("bank_reconcile_adjustment_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.BankReconcileId)
                .HasColumnName("bank_reconcile_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.CheckbookTransactionId)
                .HasColumnName("checkbook_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionDetailId)
                .HasColumnName("transaction_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionDate)
                .HasColumnName("transaction_date")
                .HasColumnType("date");

            builder.Property(t => t.DocumentType)
                .HasColumnName("document_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionType)
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ChargesId)
                .HasColumnName("charges_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.CurrencyCode)
                .HasColumnName("currency_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.ExchangeRate)
                .HasColumnName("exchange_rate")
                .HasColumnType("decimal");

            builder.Property(t => t.IsMultiply)
                .HasColumnName("is_multiply")
                .HasColumnType("bit");

            builder.Property(t => t.PaidSubject)
                .HasColumnName("paid_subject")
                .HasColumnType("varchar(250)");

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(255)");

            builder.Property(t => t.OriginatingAmount)
                .HasColumnName("originating_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.Status)
               .HasColumnName("status")
               .HasColumnType("int");

        }
    }
}
