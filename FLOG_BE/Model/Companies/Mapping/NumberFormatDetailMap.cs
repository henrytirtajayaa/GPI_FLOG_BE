using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class NumberFormatDetailMap : IEntityTypeConfiguration<Entities.NumberFormatDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.NumberFormatDetail> builder)
        {
            builder.ToTable("number_format_detail", "dbo");

            builder.HasKey(t => t.FormatDetailId);

            builder.Property(t => t.FormatDetailId)
                .IsRequired()
                .HasColumnName("format_detail_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.FormatHeaderId)
                .IsRequired()
                .HasColumnName("format_header_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.SegmentNo)
                .HasColumnName("segment_no")
                .HasColumnType("int");

            builder.Property(t => t.SegmentType)
                .HasColumnName("segment_type")
                .HasColumnType("int");

            builder.Property(t => t.SegmentLength)
                .HasColumnName("segment_length")
                .HasColumnType("int");

            builder.Property(t => t.MaskFormat)
                .HasColumnName("mask_format")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.StartingValue)
                .HasColumnName("starting_value")
                .HasColumnType("int");

            builder.Property(t => t.EndingValue)
                .HasColumnName("ending_value")
                .HasColumnType("int");

            builder.Property(t => t.Increase)
                .HasColumnName("increase")
                .HasColumnType("bit");
        }
    }
}
