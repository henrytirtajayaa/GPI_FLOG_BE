using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class TaxRefferenceNumberMap : IEntityTypeConfiguration<Entities.TaxRefferenceNumber>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.TaxRefferenceNumber> builder)
        {
            builder.ToTable("tax_refference_number", "dbo");

            builder.HasKey(t => t.TaxRefferenceId);

            builder.Property(t => t.TaxRefferenceId)
                .IsRequired()
                .HasColumnName("tax_refference_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.StartDate)
                .IsRequired()
                .HasColumnName("start_date")
                .HasColumnType("datetime");

            builder.Property(t => t.ReffNoStart)
                .HasColumnName("reff_no_start")
                .HasColumnType("int");

            builder.Property(t => t.ReffNoEnd)
                .HasColumnName("reff_no_end")
                .HasColumnType("int");

            builder.Property(t => t.DocLength)
                .HasColumnName("doc_length")
                .HasColumnType("int");

            builder.Property(t => t.LastNo)
                .HasColumnName("last_no")
                .HasColumnType("int");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("int");

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
