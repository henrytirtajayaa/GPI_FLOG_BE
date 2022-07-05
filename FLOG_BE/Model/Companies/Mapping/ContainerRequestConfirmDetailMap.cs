using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ContainerRequestConfirmDetailMap : IEntityTypeConfiguration<Entities.ContainerRequestConfirmDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ContainerRequestConfirmDetail> builder)
        {
            builder.ToTable("container_request_confirm_detail", "dbo");

            builder.HasKey(t => t.ContainerRequestConfirmDetailId);

            builder.Property(t => t.ContainerRequestConfirmDetailId)
                .IsRequired()
                .HasColumnName("container_request_confirm_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ContainerRequestConfirmHeaderId)
                .IsRequired()
                .HasColumnName("container_request_confirm_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ContainerRentalRequestDetailId)
                .IsRequired()
                .HasColumnName("container_rental_request_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Remarks)
                .HasColumnName("remarks")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.Grade)
                .HasColumnName("grade")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.QuantityToConfirm)
                .HasColumnName("quantity_to_confirm")
                .HasColumnType("int");

            builder.Property(t => t.QuantityBalance)
                .HasColumnName("quantity_balance")
                .HasColumnType("int");
        }
    }
}
