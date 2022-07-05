using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class PayableTransactionHeaderMap : IEntityTypeConfiguration<Entities.PayableTransactionHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PayableTransactionHeader> builder)
        {
            builder.ToTable("payable_transaction_header", "dbo");
            builder.HasKey(t => t.PayableTransactionId);

            builder.Property(t => t.PayableTransactionId)
                .IsRequired()
                .HasColumnName("payable_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DocumentType)
                .IsRequired()
                .HasColumnName("document_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DocumentNo)
                .IsRequired()
                .HasColumnName("document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.BranchCode)
                .HasColumnName("branch_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionDate)
                .IsRequired()
                .HasColumnName("transaction_date")
                .HasColumnType("date");

            builder.Property(t => t.TransactionType)
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CurrencyCode)
                .IsRequired()
                .HasColumnName("currency_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ExchangeRate)
                .HasColumnName("exchange_rate")
                .HasColumnType("decimal");

            builder.Property(t => t.IsMultiply)
                .HasColumnName("is_multiply")
                .HasColumnType("bit");

            builder.Property(t => t.VendorId)
                .HasColumnName("vendor_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.PaymentTermCode)
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VendorAddressCode)
                .HasColumnName("vendor_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VendorDocumentNo)
                .HasColumnName("vendor_document_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(50);

            builder.Property(t => t.NsDocumentNo)
                .HasColumnName("ns_document_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            builder.Property(t => t.SubtotalAmount)
                .HasColumnName("subtotal_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.TaxAmount)
                .HasColumnName("tax_amount")
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

            builder.Property(t => t.BillToAddressCode)
               .HasColumnName("vendor_bill_address_code")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.ShipToAddressCode)
               .HasColumnName("vendor_ship_address_code")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);
        }
    }
}
