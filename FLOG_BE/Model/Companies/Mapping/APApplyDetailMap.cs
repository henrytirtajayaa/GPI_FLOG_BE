using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class APApplyDetailMap : IEntityTypeConfiguration<Entities.APApplyDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.APApplyDetail> builder)
        {
            builder.ToTable("ap_payable_apply_detail", "dbo");
            builder.HasKey(t => t.PayableApplyDetailId);

            builder.Property(t => t.PayableApplyDetailId)
                .IsRequired()
                .HasColumnName("payable_apply_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.PayableApplyId)
                .IsRequired()
                .HasColumnName("payable_apply_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.PayableTransactionId)
                .IsRequired()
                .HasColumnName("payable_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.OriginatingBalance)
                .HasColumnName("originating_balance")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalBalance)
                .HasColumnName("functional_balance")
                .HasColumnType("decimal");

            builder.Property(t => t.OriginatingPaid)
                .HasColumnName("originating_paid")
                .HasColumnType("decimal");

            builder.Property(t => t.FunctionalPaid)
                .HasColumnName("functional_paid")
                .HasColumnType("decimal");
              
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

        }
    }
}
