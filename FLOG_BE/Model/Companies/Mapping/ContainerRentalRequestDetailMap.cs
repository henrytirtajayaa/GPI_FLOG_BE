using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ContainerRentalRequestDetailMap : IEntityTypeConfiguration<Entities.ContainerRentalRequestDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ContainerRentalRequestDetail> builder)
        {
            builder.ToTable("container_rental_request_detail", "dbo");

            builder.HasKey(t => t.ContainerRentalRequestDetailId);

            builder.Property(t => t.ContainerRentalRequestDetailId)
                .IsRequired()
                .HasColumnName("container_rental_request_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ContainerRentalRequestHeaderId)
                .IsRequired()
                .HasColumnName("container_rental_request_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ContainerCode)
                .HasColumnName("container_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ContainerName)
                .HasColumnName("container_name")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.UomCode)
                .HasColumnName("uom_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Remarks)
                .HasColumnName("remarks")
                .HasColumnType("varchar(250)")
                .HasMaxLength(50);

            builder.Property(t => t.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("int");
        }
    }
}
