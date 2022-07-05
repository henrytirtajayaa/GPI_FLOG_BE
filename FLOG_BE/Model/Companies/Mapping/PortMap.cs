using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class PortMap : IEntityTypeConfiguration<Entities.Port>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Port> builder)
        {
            builder.ToTable("port", "dbo");

            builder.HasKey(t => t.PortId);

            builder.Property(t => t.PortId)
                .IsRequired()
                .HasColumnName("port_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.PortCode) 
                .IsRequired()
                .HasColumnName("port_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PortName)
                .IsRequired()
                .HasColumnName("port_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CityCode)
                .IsRequired()
                .HasColumnName("city_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.InActive)
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

            builder.Property(t => t.PortType)
               .IsRequired()
               .HasColumnName("port_type")
               .HasColumnType("varchar(5)");
        }
    }
}
