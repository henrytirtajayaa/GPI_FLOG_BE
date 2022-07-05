using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ExchangeRateDetailMap : IEntityTypeConfiguration<Entities.ExchangeRateDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ExchangeRateDetail> builder)
        {
            builder.ToTable("exchange_rate_detail", "dbo");

            builder.HasKey(t => t.ExchangeRateDetailId);
            builder.Property(t => t.ExchangeRateDetailId)
               .IsRequired()
               .HasColumnName("exchange_rate_detail_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ExchangeRateHeaderId)
               .IsRequired()
               .HasColumnName("exchange_rate_header_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50);

            builder.Property(t => t.RateDate)
                .IsRequired()
                .HasColumnName("rate_date")
                .HasColumnType("datetime");

            builder.Property(t => t.ExpiredDate)
                .IsRequired()
                .HasColumnName("expired_date")
                .HasColumnType("datetime");

            builder.Property(t => t.RateAmount)
                .IsRequired()
                .HasColumnName("rate_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("integer");
        }

    }
}
