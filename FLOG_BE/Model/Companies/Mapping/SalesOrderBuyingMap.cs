using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class SalesOrderBuyingMap : IEntityTypeConfiguration<Entities.SalesOrderBuying>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SalesOrderBuying> builder)
        {
            builder.ToTable("sales_order_buying", "dbo");
            builder.HasKey(t => t.SalesOrderBuyingId);
            
            builder.Property(t => t.SalesOrderBuyingId)
                .IsRequired()
                .HasColumnName("sales_order_buying_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(t => t.SalesOrderId)
                .IsRequired()
                .HasColumnName("sales_order_id")
                .HasColumnType("uniqueidentifier");
            
            builder.Property(t => t.SalesOrderSellingId)
                .IsRequired()
                .HasColumnName("sales_order_selling_id")
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



            //, is_tax_after_discount, percent_discount, payment_condition, vendor_id, remark, status

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


            builder.Property(t => t.VendorId)
             .IsRequired()
             .HasColumnName("vendor_id")
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

            builder.Property(t => t.UnitAmount)
                .HasColumnName("unit_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("decimal");

        }
    }
}
