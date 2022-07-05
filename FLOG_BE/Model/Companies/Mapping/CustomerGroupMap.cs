using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CustomerGroupMap : IEntityTypeConfiguration<Entities.CustomerGroup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CustomerGroup> builder)
        {
            builder.ToTable("customer_group", "dbo");

            builder.HasKey(t => t.CustomerGroupId);

            builder.Property(t => t.CustomerGroupId)
               .IsRequired()
               .HasColumnName("customer_group_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CustomerGroupCode)
                .IsRequired()
                .HasColumnName("customer_group_code")
                .HasColumnType("varchar(20)");

            builder.Property(t => t.CustomerGroupName)
                .HasColumnName("customer_group_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);
            
            //builder.Property(t => t.AddressCode)
             //   .HasColumnName("address_code")
             //   .HasColumnType("varchar(50)");

            builder.Property(t => t.PaymentTermCode)
                .HasColumnName("payment_term_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.ReceivableAccountNo)
                .HasColumnName("receivable_account_no")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.AccruedReceivableAccountNo)
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
        }
    }
}
