using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class NegotiationSheetSellingMap : IEntityTypeConfiguration<Entities.NegotiationSheetSelling>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.NegotiationSheetSelling> builder)
        {

           
            builder.ToTable("negotiation_sheet_selling", "dbo");
            builder.HasKey(t => t.NsSellingId);
            
            builder.Property(t => t.NsSellingId)
                .IsRequired()
                .HasColumnName("ns_selling_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");
        
            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                            .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.NegotiationSheetId)
                .IsRequired()
                .HasColumnName("negotiation_sheet_id")
                .HasColumnType("uniqueidentifier");
          
            
            builder.Property(t => t.ChargeId)
                .IsRequired()
                .HasColumnName("charge_id")
                .HasColumnType("uniqueidentifier");
             
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

            builder.Property(t => t.TaxScheduleId)
               .IsRequired()
               .HasColumnName("tax_schedule_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.IsTaxAfterDiscount)
               .HasColumnName("is_tax_after_discount")
               .HasColumnType("bit");

            builder.Property(t => t.PercentDiscount)
               .HasColumnName("percent_discount")
               .HasColumnType("decimal");
            
            builder.Property(t => t.PaymentCondition)
               .HasColumnName("payment_condition")
               .HasColumnType("int");


            builder.Property(t => t.CustomerId)
             .IsRequired()
             .HasColumnName("customer_id")
             .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Remark)
             .IsRequired()
             .HasColumnName("remark")
             .HasColumnType("varchar(50)")
             .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.RowIndex)
               .HasColumnName("row_index")
               .HasColumnType("int");

            builder.Property(t => t.ReceiveTransactionId)
             .HasColumnName("receive_transaction_id")
             .HasColumnType("uniqueidentifier");

            builder.Property(t => t.UnitAmount)
                .HasColumnName("unit_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("decimal");
        }
    }
}
