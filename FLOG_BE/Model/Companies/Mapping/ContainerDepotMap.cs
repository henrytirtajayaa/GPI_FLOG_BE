using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ContainerDepotMap : IEntityTypeConfiguration<Entities.ContainerDepot>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ContainerDepot> builder)
        {
            builder.ToTable("container_depot", "dbo");

            builder.HasKey(t => t.ContainerDepotId);

            builder.Property(t => t.ContainerDepotId)
               .IsRequired()
               .HasColumnName("container_depot_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DepotCode)
                .IsRequired()
                .HasColumnName("depot_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DepotName)
                .IsRequired()
                .HasColumnName("depot_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);


            builder.Property(t => t.OwnerVendorId)
                .IsRequired()
                .HasColumnName("owner_vendor_id")
                .HasColumnType("uniqidentifier");

            builder.Property(t => t.CityCode)
                .IsRequired()
                .HasColumnName("city_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.InActive)
                .IsRequired()
                .HasColumnName("inactive")
                .HasColumnType("bit");
            
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
