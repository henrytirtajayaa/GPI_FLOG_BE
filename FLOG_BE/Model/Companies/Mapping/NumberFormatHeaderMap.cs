using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class NumberFormatHeaderMap : IEntityTypeConfiguration<Entities.NumberFormatHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.NumberFormatHeader> builder)
        { 
            builder.ToTable("number_format_header", "dbo");

            builder.HasKey(t => t.FormatHeaderId);

            builder.Property(t => t.FormatHeaderId)
                .IsRequired()
                .HasColumnName("format_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DocumentId)
                .IsRequired()
                .HasColumnName("document_id")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.LastGeneratedNo)
                .HasColumnName("last_generated_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.NumberFormat)
                .HasColumnName("number_format")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.InActive)
                .HasColumnName("inactive")
                .HasColumnType("bit");

            builder.Property(t => t.IsMonthlyReset)
                .HasColumnName("is_monthly_reset")
                .HasColumnType("bit");

            builder.Property(t => t.IsYearlyReset)
                .HasColumnName("is_yearly_reset")
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
