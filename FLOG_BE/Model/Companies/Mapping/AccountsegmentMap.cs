using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class AccountSegmentMap : IEntityTypeConfiguration<Entities.AccountSegment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AccountSegment> builder)
        {
            builder.ToTable("account_segment", "dbo");

            builder.HasKey(t => t.SegmentId);

            builder.Property(t => t.SegmentId)
               .IsRequired()
               .HasColumnName("segment_id")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.SegmentNo)
                .IsRequired()
                .HasColumnName("segment_no")
                .HasColumnType("integer");


            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("segment_description")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.Length)
                .IsRequired()
                .HasColumnName("segment_length")
                .HasColumnType("integer");

            builder.Property(t => t.IsMainAccount)
                .IsRequired()
                .HasColumnName("main_account")
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
