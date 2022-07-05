using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ExchangeRateHeaderMap : IEntityTypeConfiguration<Entities.ExchangeRateHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ExchangeRateHeader> builder)
        {
            builder.ToTable("exchange_rate_header", "dbo");

            builder.HasKey(t => t.ExchangeRateHeaderId);
            builder.Property(t => t.ExchangeRateHeaderId)
               .IsRequired()
               .HasColumnName("exchange_rate_header_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");


            builder.Property(t => t.ExchangeRateCode)
                .IsRequired()
                .HasColumnName("exchange_rate_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.CurrencyCode)
                .IsRequired()
                .HasColumnName("currency_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.RateType)
                .IsRequired()
                .HasColumnName("rate_type")
                .HasColumnType("integer");

            builder.Property(t => t.ExpiredPeriod)
                .IsRequired()
                .HasColumnName("expired_period")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CalculationType)
                .IsRequired()
                .HasColumnName("calculation_type")
                .HasColumnType("integer");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("integer");

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
