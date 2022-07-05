using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class NumberFormatLastNoMap : IEntityTypeConfiguration<Entities.NumberFormatLastNo>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.NumberFormatLastNo> builder)
        { 
            builder.ToTable("number_format_last_no", "dbo");

            builder.HasKey(t => t.NumberFormatLastNoId);

            builder.Property(t => t.NumberFormatLastNoId)
                .IsRequired()
                .HasColumnName("number_format_last_no_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DocumentId)
                .IsRequired()
                .HasColumnName("document_id")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            builder.Property(t => t.PeriodYear)
                .HasColumnName("period_year")
                .HasColumnType("int");

            builder.Property(t => t.PeriodMonth)
                .HasColumnName("period_month")
                .HasColumnType("int");

            builder.Property(t => t.LastNo)
                .HasColumnName("last_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.LastIndex)
                .HasColumnName("last_index")
                .HasColumnType("int");

            builder.Property(t => t.ModifiedDate)
                .HasColumnName("modified_date")
                .HasColumnType("datetime");
        }
    }
}
