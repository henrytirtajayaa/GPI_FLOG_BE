using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class PayableSetupMap : IEntityTypeConfiguration<Entities.PayableSetup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PayableSetup> builder)
        {
            builder.ToTable("payable_setup", "dbo");

            builder.HasKey(t => t.PayableSetupId);

            builder.Property(t => t.PayableSetupId)
                .IsRequired()
                .HasColumnName("payable_setup_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DefaultRateType)
                .HasColumnName("default_rate_type")
                .HasColumnType("int");

            builder.Property(t => t.TaxRateType)
                .HasColumnName("tax_rate_type")
                .HasColumnType("int");

            builder.Property(t => t.AgingByDocdate)
                .HasColumnName("aging_by_docdate")
                .HasColumnType("bit");

            builder.Property(t => t.WriteoffLimit)
                .HasColumnName("writeoff_limit")
                .HasColumnType("decimal(20,5)");

           
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
