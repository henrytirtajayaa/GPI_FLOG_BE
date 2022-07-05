using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class BankReconcileDetailMap : IEntityTypeConfiguration<Entities.BankReconcileDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.BankReconcileDetail> builder)
        {
            builder.ToTable("bank_reconcile_detail", "dbo");

            builder.HasKey(t => t.BankReconcileDetailId);

            builder.Property(t => t.BankReconcileDetailId)
                .IsRequired()
                .HasColumnName("bank_reconcile_detail_id")
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

            builder.Property(t => t.TransactionDate)
                .HasColumnName("transaction_date")
                .HasColumnType("date");

            builder.Property(t => t.TransactionId)
                .HasColumnName("transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Modul)
                .HasColumnName("modul")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Status)
               .HasColumnName("status")
               .HasColumnType("int");

        }
    }
}
