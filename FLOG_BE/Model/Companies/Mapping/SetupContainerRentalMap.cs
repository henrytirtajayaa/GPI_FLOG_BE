using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class SetupContainerRentalMap : IEntityTypeConfiguration<Entities.SetupContainerRental>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SetupContainerRental> builder)
        {
            builder.ToTable("setup_container_rental", "dbo");

            builder.HasKey(t => t.SetupContainerRentalId);

            builder.Property(t => t.SetupContainerRentalId)
                .IsRequired()
                .HasColumnName("setup_container_rental_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TransactionType)
                .IsRequired()
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.RequestDocNo)
                .HasColumnName("request_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DeliveryDocNo)
                .HasColumnName("delivery_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ClosingDocNo)
                .HasColumnName("closing_doc_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.UomScheduleCode)
                .HasColumnName("uom_schedule_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CustomerFreeUsageDays)
                .HasColumnName("customer_free_usage_days")
                .HasColumnType("integer");

            builder.Property(t => t.ShippingLineFreeUsageDays)
                .HasColumnName("shipping_line_free_usage_days")
                .HasColumnType("integer");

            builder.Property(t => t.CntOwnerFreeUsageDays)
                .HasColumnName("cnt_owner_free_usage_days")
                .HasColumnType("integer");

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
