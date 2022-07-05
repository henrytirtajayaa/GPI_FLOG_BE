using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class SalesQuotationDetailMap : IEntityTypeConfiguration<Entities.SalesQuotationDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SalesQuotationDetail> builder)
        {
            builder.ToTable("sales_quot_detail", "dbo");

            builder.HasKey(t => t.SalesQuotationDetailId);

            builder.Property(t => t.SalesQuotationDetailId)
                .IsRequired()
                .HasColumnName("sales_quot_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                 .HasColumnName("row_id")
                 .UseSqlServerIdentityColumn()
                 .HasColumnType("bigint");


            builder.Property(t => t.SalesQuotationId)
                .HasColumnName("sales_quotation_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Qty)
                .HasColumnName("qty")
                .HasColumnType("int");

            builder.Property(t => t.ContainerId)
               .HasColumnName("container_id")
               .HasColumnType("uniqueidentifier");
            
            builder.Property(t => t.UomDetailId)
               .HasColumnName("uom_detail_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Remark)
                .HasColumnName("remark")
                .HasColumnType("text");

            builder.Property(t => t.Status)
               .HasColumnName("status")
               .HasColumnType("integer");

            builder.Property(t => t.RowIndex)
               .HasColumnName("row_index")
               .HasColumnType("int");

        }
    }
}
