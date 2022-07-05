using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class TaxScheduleMap : IEntityTypeConfiguration<Entities.TaxSchedule>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.TaxSchedule> builder)
        {
            builder.ToTable("tax_schedule", "dbo");

            builder.HasKey(t => t.TaxScheduleId);

            builder.Property(t => t.TaxScheduleId)
               .IsRequired()
               .HasColumnName("tax_schedule_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TaxScheduleCode)
                .IsRequired()
                .HasColumnName("tax_schedule_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(50)")
                .HasMaxLength(100);


            builder.Property(t => t.IsSales)
                .IsRequired()
                .HasColumnName("is_sales")
                .HasColumnType("bit");

            builder.Property(t => t.PercentOfSalesPurchase)
                .IsRequired()
                .HasColumnName("percent_of_sales_purchase")
                .HasColumnType("decimal");

            builder.Property(t => t.TaxablePercent)
                .IsRequired()
                .HasColumnName("taxable_percent")
                .HasColumnType("decimal");

            builder.Property(t => t.RoundingType)
                .IsRequired()
                .HasColumnName("rounding_type")
                .HasColumnType("byte");
            
            builder.Property(t => t.RoundingLimitAmount)
                .IsRequired()
                .HasColumnName("rounding_limit_amount")
                .HasColumnType("decimal");
            
            builder.Property(t => t.TaxInclude)
                .IsRequired()
                .HasColumnName("tax_include")
                .HasColumnType("bit");
            
            builder.Property(t => t.WithHoldingTax)
                .IsRequired()
                .HasColumnName("withholding_tax")
                .HasColumnType("bit");

            builder.Property(t => t.TaxAccountNo)
                .HasColumnName("tax_account_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Inactive)
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
