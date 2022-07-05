using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ContainerRentalRequestHeaderMap : IEntityTypeConfiguration<Entities.ContainerRentalRequestHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ContainerRentalRequestHeader> builder)
        {
            builder.ToTable("container_rental_request_header", "dbo");

            builder.HasKey(t => t.ContainerRentalRequestHeaderId);

            builder.Property(t => t.ContainerRentalRequestHeaderId)
                .IsRequired()
                .HasColumnName("container_rental_request_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TransactionType)
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DocumentDate)
                .HasColumnName("document_date")
                .HasColumnType("datetime");

            builder.Property(t => t.DocumentNo)
                .HasColumnName("document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CustomerId)
                .HasColumnName("customer_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.AddressCode)
                .HasColumnName("address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.SalesCode)
                .HasColumnName("sales_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VendorId)
                .HasColumnName("vendor_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.BillToAddressCode)
                .HasColumnName("bill_to_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ShipToAddressCode)
                .HasColumnName("ship_to_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

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

            builder.Property(t => t.CanceledBy)
                .HasColumnName("canceled_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CanceledDate)
                .HasColumnName("canceled_date")
                .HasColumnType("datetime");
        }
    }
}
