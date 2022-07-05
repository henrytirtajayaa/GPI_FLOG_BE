using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CheckbookTransactionHeaderMap : IEntityTypeConfiguration<Entities.CheckbookTransactionHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CheckbookTransactionHeader> builder)
        {
            builder.ToTable("checkbook_transaction_header", "dbo");

            builder.HasKey(t => t.CheckbookTransactionId);

            builder.Property(t => t.CheckbookTransactionId)
                .IsRequired()
                .HasColumnName("checkbook_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DocumentType)
                .HasColumnName("document_type")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.DocumentNo)
                .HasColumnName("document_no")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.BranchCode)
                .HasColumnName("branch_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionDate)
                .HasColumnName("transaction_date")
                .HasColumnType("datetime");

            builder.Property(t => t.TransactionType)
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CurrencyCode)
                .HasColumnName("currency_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.ExchangeRate)
                .HasColumnName("exchange_rate")
                .HasColumnType("decimal")
                .HasMaxLength(50);

            builder.Property(t => t.IsMultiply)
                .HasColumnName("is_multiply")
                .HasColumnType("bit");

            builder.Property(t => t.CheckbookCode)
                .HasColumnName("checkbook_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.IsVoid)
                .HasColumnName("is_void")
                .HasColumnType("bit");

            builder.Property(t => t.VoidDocumentNo)
                .HasColumnName("void_document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PaidSubject)
                .HasColumnName("paid_subject")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.SubjectCode)
                .HasColumnName("subject_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.OriginatingTotalAmount)
                .HasColumnName("originating_total_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalTotalAmount)
                .HasColumnName("functional_total_amount")
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
               .HasMaxLength(50);
        }
    }
}
