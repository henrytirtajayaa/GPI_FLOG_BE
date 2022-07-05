using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class SalesOrderTruckingMap : IEntityTypeConfiguration<Entities.SalesOrderTrucking>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SalesOrderTrucking> builder)
        {


            builder.ToTable("sales_order_trucking", "dbo");
            builder.HasKey(t => t.SalesOrderTruckingId);
            builder.Property(t => t.SalesOrderTruckingId)
                .IsRequired()
                .HasColumnName("sales_order_trucking_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                 .HasColumnName("row_id")
                 .UseSqlServerIdentityColumn()
                 .HasColumnType("bigint");

            builder.Property(t => t.SalesOrderId)
                .HasColumnName("sales_order_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.VehicleTypeId)
               .HasColumnName("vehicle_type_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.TruckloadTerm)
               .HasColumnName("truckload_term")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.VendorId)
               .HasColumnName("vendor_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Qty)
               .HasColumnName("qty")
               .HasColumnType("int");

            builder.Property(t => t.UomDetailId)
               .HasColumnName("uom_detail_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Status)
               .HasColumnName("status")
               .HasColumnType("integer");

            builder.Property(t => t.RowIndex)
               .HasColumnName("row_index")
               .HasColumnType("int");
        }
    }
}
