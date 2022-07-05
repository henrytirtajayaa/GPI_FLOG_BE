using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CheckbookTransactionDetailMap : IEntityTypeConfiguration<Entities.CheckbookTransactionDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CheckbookTransactionDetail> builder)
        {
            builder.ToTable("checkbook_transaction_detail", "dbo");
            builder.HasKey(t => t.TransactionDetailId);

            builder.Property(t => t.TransactionDetailId)
                .IsRequired()
                .HasColumnName("transaction_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CheckbookTransactionId)
                .IsRequired()
                .HasColumnName("checkbook_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ChargesId)
                .HasColumnName("charges_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ChargesDescription)
                .HasColumnName("charges_description")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

            builder.Property(t => t.OriginatingAmount)
                .HasColumnName("originating_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalAmount)
                .HasColumnName("functional_amount")
                .HasColumnType("decimal");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.RowIndex)
                .HasColumnName("row_index")
                .HasColumnType("int");
        }
    }
}
