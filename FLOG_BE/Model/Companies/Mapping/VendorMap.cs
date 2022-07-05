using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class VendorMap : IEntityTypeConfiguration<Entities.Vendor>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Vendor> builder)
        {
            builder.ToTable("vendor", "dbo");

            builder.HasKey(t => t.VendorId);

            builder.Property(t => t.VendorId)
               .IsRequired()
               .HasColumnName("vendor_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.VendorCode)
                .IsRequired()
                .HasColumnName("vendor_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VendorName)
                .IsRequired()
                .HasColumnName("vendor_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.AddressCode)
                .HasColumnName("address_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.TaxRegistrationNo)
                .HasColumnName("tax_registration_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.VendorTaxName)
                .HasColumnName("vendor_tax_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.VendorGroupCode)
                .IsRequired()
                .HasColumnName("vendor_group_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PaymentTermCode)
                .IsRequired()
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.HasCreditLimit)
                .IsRequired()
                .HasColumnName("has_credit_limit")
                .HasColumnType("bit");

            builder.Property(t => t.CreditLimit)
                .IsRequired()
                .HasColumnName("credit_limit")
                .HasColumnType("decimal");

            builder.Property(t => t.ShipToAddressCode)
                .HasColumnName("ship_to_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.BillToAddressCode)
                .HasColumnName("bill_to_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.TaxAddressCode)
                .HasColumnName("tax_address_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.PayableAccountNo)
                .IsRequired()
                .HasColumnName("payable_account_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.AccruedPayableAccountNo)
                .IsRequired()
                .HasColumnName("accrued_payable_account_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

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

        }

    }
}
