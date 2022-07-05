using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ContainerMap : IEntityTypeConfiguration<Entities.Container>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Container> builder)
        {
            builder.ToTable("container", "dbo");

            builder.HasKey(t => t.ContainerId);

            builder.Property(t => t.ContainerId)
               .IsRequired()
               .HasColumnName("container_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ContainerCode)
                .IsRequired()
                .HasColumnName("container_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);


            builder.Property(t => t.ContainerName)
                .IsRequired()
                .HasColumnName("container_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.ContainerSize)
                .IsRequired()
                .HasColumnName("container_size")
                .HasColumnType("integer");

            builder.Property(t => t.ContainerType)
                .IsRequired()
                .HasColumnName("ref_container_type")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.ContainerTeus)
                .IsRequired()
                .HasColumnName("container_teus")
                .HasColumnType("integer");


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
