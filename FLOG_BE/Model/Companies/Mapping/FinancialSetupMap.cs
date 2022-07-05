using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class FinancialSetupMap : IEntityTypeConfiguration<Entities.FinancialSetup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.FinancialSetup> builder)
        {
            builder.ToTable("financial_setup", "dbo");

            builder.HasKey(t => t.FinancialSetupId);

            builder.Property(t => t.FinancialSetupId)
                .IsRequired()
                .HasColumnName("financial_setup_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.FuncCurrencyCode)
                .HasColumnName("func_currency_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DefaultRateType)
                .HasColumnName("default_rate_type")
                .HasColumnType("int");

            builder.Property(t => t.TaxRateType)
                .HasColumnName("tax_rate_type")
                .HasColumnType("int");
            
            builder.Property(t => t.DeptSegmentNo)
                .HasColumnName("dept_segment_no")
                .HasColumnType("int");

            builder.Property(t => t.CheckbookChargesType)
                .HasColumnName("checkbook_charges_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.UomScheduleCode)
                .HasColumnName("uom_schedule_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

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
