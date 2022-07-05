using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ReferenceMap : IEntityTypeConfiguration<Entities.Reference>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Reference> builder)
        {
            builder.ToTable("reference", "dbo");

            builder.HasKey(t => t.ReferenceId);

            builder.Property(t => t.ReferenceId)
               .IsRequired()
               .HasColumnName("reference_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ReferenceType)
                .IsRequired()
                .HasColumnName("reference_type")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);


            builder.Property(t => t.ReferenceCode)
                .IsRequired()
                .HasColumnName("reference_code")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.ReferenceName)
                .IsRequired()
                .HasColumnName("reference_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.Inactive)
                .IsRequired()
                .HasColumnName("inactive")
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
