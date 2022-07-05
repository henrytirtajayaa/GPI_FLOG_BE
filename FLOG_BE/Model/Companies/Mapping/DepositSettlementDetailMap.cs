using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class DepositSettlementDetailMap : IEntityTypeConfiguration<Entities.DepositSettlementDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.DepositSettlementDetail> builder)
        {
            builder.ToTable("deposit_settlement_detail", "dbo");

            builder.HasKey(t => t.SettlementDetailId);

            builder.Property(t => t.SettlementDetailId)
                .IsRequired()
                .HasColumnName("deposit_settlement_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .HasColumnType("bigint");

            //builder.Property(p => p.RowId)
            //    .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.SettlementHeaderId)
                .HasColumnName("deposit_settlement_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ReceiveTransactionId)
                .HasColumnName("receive_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.ReceivableApplyDetailId)
                .HasColumnName("receivable_apply_detail_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

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
