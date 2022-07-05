using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ChargesDetailMap : IEntityTypeConfiguration<Entities.ChargesDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ChargesDetail> builder)
        {
            builder.ToTable("charges_detail", "dbo");
            builder.HasKey(t => t.ChargesDetailId);
            builder.Property(t => t.ChargesDetailId)
               .IsRequired()
               .HasColumnName("charges_detail_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ChargesId)
                .IsRequired()
                .HasColumnName("charges_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ShippingLineId)
                .HasColumnName("shipping_line_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.TrxModule)
                .IsRequired()
                .HasColumnName("trx_module")
                .HasColumnType("int");

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
                        
        }

    }
}
