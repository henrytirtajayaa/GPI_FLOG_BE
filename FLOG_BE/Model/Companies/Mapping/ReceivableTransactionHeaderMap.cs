using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ReceivableTransactionHeaderMap : IEntityTypeConfiguration<Entities.ReceivableTransactionHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ReceivableTransactionHeader> builder)
        {
            builder.ToTable("receive_transaction_header", "dbo");

            builder.HasKey(t => t.ReceiveTransactionId);

            builder.Property(t => t.ReceiveTransactionId)
                .IsRequired()
                .HasColumnName("receive_transaction_id")
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

            builder.Property(t => t.CustomerId)
                .HasColumnName("customer_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.PaymentTermCode)
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            builder.Property(t => t.CustomerAddressCode)
                .HasColumnName("customer_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            builder.Property(t => t.SoDocumentNo)
                .HasColumnName("so_document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.NsDocumentNo)
                .HasColumnName("ns_document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

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
               .HasColumnName("customer_bill_address_code")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);
            
            builder.Property(t => t.ShipToAddressCode)
               .HasColumnName("customer_ship_address_code")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);
        }
    }
}
