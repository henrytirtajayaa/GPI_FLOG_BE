using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CustomerMap : IEntityTypeConfiguration<Entities.Customer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Customer> builder)
        {
            builder.ToTable("customer", "dbo");

            builder.HasKey(t => t.CustomerId);

            builder.Property(t => t.CustomerId)
               .IsRequired()
               .HasColumnName("customer_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CustomerCode)
                .IsRequired()
                .HasColumnName("customer_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.CustomerName)
                .IsRequired()
                .HasColumnName("customer_name")
                .HasColumnType("varchar(200)");
            
            builder.Property(t => t.AddressCode)
                .HasColumnName("address_code")
                .HasColumnType("varchar(50)");
       
            builder.Property(t => t.TaxRegistrationNo)
                .HasColumnName("tax_registration_no")
                .HasColumnType("varchar(100)");
            
            builder.Property(t => t.CustomerTaxName)
                .HasColumnName("customer_tax_name")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.CustomerGroupCode)
                .IsRequired()
                .HasColumnName("customer_group_code")
                .HasColumnType("varchar(50)");
        
            builder.Property(t => t.PaymentTermCode)
                .IsRequired()
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)");
            
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
                .HasColumnType("varchar(50)");
            
            builder.Property(t => t.BillToAddressCode)
                .HasColumnName("bill_to_address_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.TaxAddressCode)
                .HasColumnName("tax_address_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.ReceivableAccountNo)
                .IsRequired()
                .HasColumnName("receivable_account_no")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.AccruedReceivableAccountNo)
                .IsRequired()
                .HasColumnName("accrued_receivable_account_no")
                .HasColumnType("varchar(50)");

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
            
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

        }

    }
}
