using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ShippingLineMap : IEntityTypeConfiguration<Entities.ShippingLine>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ShippingLine> builder)
        {
            builder.ToTable("shipping_line", "dbo");

            builder.HasKey(t => t.ShippingLineId);

            builder.Property(t => t.ShippingLineId)
               .IsRequired()
               .HasColumnName("shipping_line_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ShippingLineCode)
                .IsRequired()
                .HasColumnName("shipping_line_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ShippingLineName)
                .HasColumnName("shipping_line_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.ShippingLineType)
                .HasColumnName("shipping_line_type")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            builder.Property(t => t.VendorId)
                .IsRequired()
                .HasColumnName("vendor_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.IsFeeder)
                .IsRequired()
                .HasColumnName("is_feeder")
                .HasColumnType("bit");

            builder.Property(t => t.Inactive)
                .IsRequired()
                .HasColumnName("inactive")
                .HasColumnType("bit");

            builder.Property(t => t.Status)
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
