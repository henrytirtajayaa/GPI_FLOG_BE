using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class FiscalPeriodDetailMap : IEntityTypeConfiguration<Entities.FiscalPeriodDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.FiscalPeriodDetail> builder)
        {
            builder.ToTable("fiscal_period_detail", "dbo");

            builder.HasKey(t => t.FiscalDetailId);

            builder.Property(t => t.FiscalDetailId)
               .IsRequired()
               .HasColumnName("fiscal_detail_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.FiscalHeaderId)
               .IsRequired()
               .HasColumnName("fiscal_header_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50);

            builder.Property(t => t.PeriodIndex)
                .IsRequired()
                .HasColumnName("period_index")
                .HasColumnType("int");
 
            builder.Property(t => t.PeriodStart)
                .IsRequired()
                .HasColumnName("period_start")
                .HasColumnType("Datetime");

            builder.Property(t => t.PeriodEnd)
                .IsRequired()
                .HasColumnName("period_end")
                .HasColumnType("Datetime");

            builder.Property(t => t.IsClosePurchasing)
                .IsRequired()
                .HasColumnName("is_close_purchasing")
                .HasColumnType("bit");
            
            builder.Property(t => t.IsCloseSales)
                .IsRequired()
                .HasColumnName("is_close_sales")
                .HasColumnType("bit");
            
            builder.Property(t => t.IsCloseInventory)
                .IsRequired()
                .HasColumnName("is_close_inventory")
                .HasColumnType("bit");

            builder.Property(t => t.IsCloseFinancial)
                .IsRequired()
                .HasColumnName("is_close_financial")
                .HasColumnType("bit");
            
            builder.Property(t => t.IsCloseAsset)
                .IsRequired()
                .HasColumnName("is_close_asset")
                .HasColumnType("bit");

        }

    }
}
