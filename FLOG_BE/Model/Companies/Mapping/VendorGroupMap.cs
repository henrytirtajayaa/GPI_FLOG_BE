using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class VendorGroupMap : IEntityTypeConfiguration<Entities.VendorGroup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.VendorGroup> builder)
        {
            builder.ToTable("vendor_group", "dbo");

            builder.HasKey(t => t.VendorGroupId);

            builder.Property(t => t.VendorGroupId)
                .IsRequired()
                .HasColumnName("vendor_group_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())"); ;

            builder.Property(t => t.VendorGroupCode)
                .HasColumnName("vendor_group_code")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            builder.Property(t => t.VendorGroupName)
                .HasColumnName("vendor_group_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            //builder.Property(t => t.AddressCode)
            //   .HasColumnName("address_code")
            //   .HasColumnType("varchar(50)")
            //   .HasMaxLength(50);

            builder.Property(t => t.PaymentTermCode)
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PayableAccountNo)
                .HasColumnName("payable_account_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.AccruedPayableAccountNo)
                .HasColumnName("accrued_payable_account_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.InActive)
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
        }
    }
}
