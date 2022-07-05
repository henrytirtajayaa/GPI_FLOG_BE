using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CurrencyMap : IEntityTypeConfiguration<Entities.Currency>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Currency> builder)
        {
            builder.ToTable("currency", "dbo");

            builder.HasKey(t => t.CurrencyId);

            builder.Property(t => t.CurrencyId)
               .IsRequired()
               .HasColumnName("currency_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CurrencyCode)
                .IsRequired()
                .HasColumnName("currency_code")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.Symbol)
                .IsRequired()
                .HasColumnName("symbol")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.DecimalPlaces)
                .IsRequired()
                .HasColumnName("decimal_places")
                .HasColumnType("integer");

            builder.Property(t => t.RealizedGainAcc)
                .HasColumnName("realized_gain_acc")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.RealizedLossAcc)
                .HasColumnName("realized_loss_acc")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.UnrealizedGainAcc)
                .HasColumnName("unrealized_gain_acc")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.UnrealizedLossAcc)
                .HasColumnName("unrealized_loss_acc")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

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

            builder.Property(t => t.CurrencyUnit)
                .HasColumnName("currency_unit")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CurrencySubUnit)
                .HasColumnName("currency_subunit")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

        }

    }
}
