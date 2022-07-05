﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class APApplyHeaderMap : IEntityTypeConfiguration<Entities.APApplyHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.APApplyHeader> builder)
        {
            builder.ToTable("ap_payable_apply_header", "dbo");
            builder.HasKey(t => t.PayableApplyId);

            builder.Property(t => t.PayableApplyId)
                .IsRequired()
                .HasColumnName("payable_apply_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.TransactionDate)
                .IsRequired()
                .HasColumnName("transaction_date")
                .HasColumnType("date");

            builder.Property(t => t.DocumentType)
                .HasColumnName("document_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DocumentNo)
                .IsRequired()
                .HasColumnName("document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PaymentHeaderId)
                .HasColumnName("payment_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.CheckbookTransactionId)
                .HasColumnName("checkbook_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.PayableTransactionId)
                .HasColumnName("payable_transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.VendorId)
                .HasColumnName("vendor_id")
                .HasColumnType("uniqueidentifier");
              
            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("text");
            
            builder.Property(t => t.OriginatingTotalPaid)
               .HasColumnName("originating_total_paid")
               .HasColumnType("decimal");
               
            builder.Property(t => t.FunctionalTotalPaid)
               .HasColumnName("functional_total_paid")
               .HasColumnType("decimal");          

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

            builder.Property(t => t.VoidBy)
                .HasColumnName("void_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VoidDate)
                .HasColumnName("void_date")
                .HasColumnType("datetime");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.StatusComment)
                .HasColumnName("status_comment")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);
        }
    }
}
