using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class NegotiationSheetContainerMap : IEntityTypeConfiguration<Entities.NegotiationSheetContainer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.NegotiationSheetContainer> builder)
        {
            builder.ToTable("negotiation_sheet_container", "dbo");
            builder.HasKey(t => t.NSContainerId);

            builder.Property(t => t.NSContainerId)
                .IsRequired()
                .HasColumnName("ns_container_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                 .HasColumnName("row_id")
                 .UseSqlServerIdentityColumn()
                 .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                            .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.NegotiationSheetId)
                .HasColumnName("negotiation_sheet_id")
                .HasColumnType("uniqueidentifier");

           

            builder.Property(t => t.ContainerId)
               .HasColumnName("container_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Qty)
               .HasColumnName("qty")
               .HasColumnType("int");

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
