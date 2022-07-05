using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CheckbookMap : IEntityTypeConfiguration<Entities.Checkbook>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Checkbook> builder)
        {
            builder.ToTable("checkbook", "dbo");

            builder.HasKey(t => t.CheckbookId);

            builder.Property(t => t.CheckbookId)
                .IsRequired()
                .HasColumnName("checkbook_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())"); ;

            builder.Property(t => t.CheckbookCode)
                .HasColumnName("checkbook_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CheckbookName)
                .HasColumnName("checkbook_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.BankAccountCode)
                .HasColumnName("bank_account_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CurrencyCode)
                .HasColumnName("currency_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.BankCode)
                .HasColumnName("bank_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CheckbookAccountNo)
                .HasColumnName("checkbook_account_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.HasCheckoutApproval)
                .HasColumnName("has_checkout_approval")
                .HasColumnType("bit");

            builder.Property(t => t.ApprovalCode)
                .HasColumnName("approval_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CheckbookInDocNo)
                .HasColumnName("checkbook_in_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CheckbookOutDocNo)
                .HasColumnName("checkbook_out_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ReceiptDocNo)
                .HasColumnName("receipt_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PaymentDocNo)
                .HasColumnName("payment_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ReconcileDocNo)
                .HasColumnName("reconcile_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.IsCash)
                .HasColumnName("is_cash")
                .HasColumnType("bit");

            builder.Property(t => t.InActive)
                .HasColumnName("inactive")
                .HasColumnType("bit");

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
