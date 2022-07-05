using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace FLOG_BE.Model.Companies.Mapping
{
    public class ContainerRequestConfirmHeaderMap : IEntityTypeConfiguration<Entities.ContainerRequestConfirmHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ContainerRequestConfirmHeader> builder)
        {
            builder.ToTable("container_request_confirm_header", "dbo");

            builder.HasKey(t => t.ContainerRequestConfirmHeaderId);

            builder.Property(t => t.ContainerRequestConfirmHeaderId)
                .IsRequired()
                .HasColumnName("container_request_confirm_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ContainerRentalRequestHeaderId)
                .IsRequired()
                .HasColumnName("container_rental_request_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.DocumentDate)
                .HasColumnName("document_date")
                .HasColumnType("datetime");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.DeliveryOrderNo)
                .HasColumnName("delivery_order_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.IssueDate)
                .HasColumnName("issue_date")
                .HasColumnType("datetime");

            builder.Property(t => t.ExpiredDate)
                .HasColumnName("expired_date")
                .HasColumnType("datetime");

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
