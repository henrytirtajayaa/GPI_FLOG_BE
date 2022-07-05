using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CheckbookTransactionApprovalMap : IEntityTypeConfiguration<Entities.CheckbookTransactionApproval>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CheckbookTransactionApproval> builder)
        {
            builder.ToTable("checkbook_transaction_approval", "dbo");
            builder.HasKey(t => t.CheckbookTransactionApprovalId);

            builder.Property(t => t.CheckbookTransactionApprovalId)
                .IsRequired()
                .HasColumnName("checkbook_transaction_approval_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CheckbookTransactionId)
                .IsRequired()
                .HasColumnName("checkbook_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Index)
                .HasColumnName("index")
                .HasColumnType("integer");

            builder.Property(t => t.PersonId)
                .HasColumnName("person_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.PersonCategoryId)
                .HasColumnName("person_category_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("integer");
        }
    }
}
