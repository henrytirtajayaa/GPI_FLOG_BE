using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ApPaymentApprovalMap : IEntityTypeConfiguration<Entities.ApPaymentApproval>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ApPaymentApproval> builder)
        {

            builder.ToTable("ap_payment_approval", "dbo");
            builder.HasKey(t => t.PaymentApprovalId);

            builder.Property(t => t.PaymentApprovalId)
                .IsRequired()
                .HasColumnName("payment_approval_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.PaymentHeaderId)
                .IsRequired()
                .HasColumnName("payment_header_id")
                .HasColumnType("uniqueidentifier");


            builder.Property(t => t.Index)
                .IsRequired()
                .HasColumnName("index")
                .HasColumnType("int");

            builder.Property(t => t.PersonId)
                .IsRequired()
                .HasColumnName("Person_id")
                .HasColumnType("uniqueidentifier");


            builder.Property(t => t.PersonCategoryId)
                .IsRequired()
                .HasColumnName("person_category_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.RowId)
                  .HasColumnName("row_id")
                  .UseSqlServerIdentityColumn()
                  .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
        }
    }
}
