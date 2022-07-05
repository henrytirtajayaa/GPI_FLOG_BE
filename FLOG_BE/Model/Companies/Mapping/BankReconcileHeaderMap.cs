using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class BankReconcileHeaderMap : IEntityTypeConfiguration<Entities.BankReconcileHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.BankReconcileHeader> builder)
        {
            builder.ToTable("bank_reconcile_header", "dbo");

            builder.HasKey(t => t.BankReconcileId);

            builder.Property(t => t.BankReconcileId)
                .IsRequired()
                .HasColumnName("bank_reconcile_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.TransactionDate)
                .HasColumnName("transaction_date")
                .HasColumnType("date");

            builder.Property(t => t.PrevBankReconcileId)
                .HasColumnName("prev_bank_reconcile_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.DocumentNo)
                .HasColumnName("document_no")
                .HasColumnType("varchar(50)");
            
            builder.Property(t => t.CheckbookCode)
                .HasColumnName("checkbook_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CurrencyCode)
                .HasColumnName("currency_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.BankCutoffStart)
                .HasColumnName("bank_cutoff_start")
                .HasColumnType("date")
                .HasMaxLength(50);

            builder.Property(t => t.BankCutoffEnd)
                .HasColumnName("bank_cutoff_end")
                .HasColumnType("date")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            builder.Property(t => t.BankEndingOrgBalance)
                .HasColumnName("bank_ending_originating_balance")
                .HasColumnType("decimal");

            builder.Property(t => t.CheckbookEndingOrgBalance)
                .HasColumnName("checkbook_ending_originating_balance")
                .HasColumnType("decimal");

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

            builder.Property(t => t.VoidBy)
                .HasColumnName("void_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VoidDate)
                .HasColumnName("void_date")
                .HasColumnType("datetime");

            builder.Property(t => t.Status)
               .HasColumnName("status")
               .HasColumnType("int");

            builder.Property(t => t.StatusComment)
               .HasColumnName("status_comment")
               .HasColumnType("varchar(255)")
               .HasMaxLength(255);
        }
    }
}
