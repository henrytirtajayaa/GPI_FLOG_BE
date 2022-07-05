using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ChargesMap : IEntityTypeConfiguration<Entities.Charges>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Charges> builder)
        {

        
            builder.ToTable("charges", "dbo");
            builder.HasKey(t => t.ChargesId);
            builder.Property(t => t.ChargesId)
               .IsRequired()
               .HasColumnName("charges_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ChargesCode)
                .IsRequired()
                .HasColumnName("charges_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ChargesName)
                .IsRequired()
                .HasColumnName("charges_name")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionType)
                .IsRequired()
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ChargeGroupCode)
                .HasColumnName("charge_group_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.IsPurchasing)
                .IsRequired()
                .HasColumnName("is_purchasing")
                .HasColumnType("bit");

            builder.Property(t => t.IsSales)
                .IsRequired()
                .HasColumnName("is_sales")
                .HasColumnType("bit");

            builder.Property(t => t.IsInventory)
                .IsRequired()
                .HasColumnName("is_inventory")
                .HasColumnType("bit");

            builder.Property(t => t.IsFinancial)
                .IsRequired()
                .HasColumnName("is_financial")
                .HasColumnType("bit");

            builder.Property(t => t.IsAsset)
                .IsRequired()
                .HasColumnName("is_asset")
                .HasColumnType("bit");

            builder.Property(t => t.IsDeposit)
                .IsRequired()
                .HasColumnName("is_deposit")
                .HasColumnType("bit");

            builder.Property(t => t.RevenueAccountNo)
               .IsRequired()
               .HasColumnName("revenue_account_no")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.TempRevenueAccountNo)
               .IsRequired()
               .HasColumnName("temp_revenue_account_no")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.CostAccountNo)
              .IsRequired()
              .HasColumnName("cost_account_no")
              .HasColumnType("varchar(50)")
              .HasMaxLength(50);

            builder.Property(t => t.TaxScheduleCode)
               .IsRequired()
               .HasColumnName("tax_schedule_code")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.ShippingLineType)
               .HasColumnName("shipping_line_type")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.HasCosting)
               .HasColumnName("has_costing")
               .HasColumnType("bit");

            builder.Property(t => t.InActive)
               .IsRequired()
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
