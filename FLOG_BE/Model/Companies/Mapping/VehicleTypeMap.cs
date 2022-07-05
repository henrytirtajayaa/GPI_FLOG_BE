using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public partial class VehicleTypeMap
        : IEntityTypeConfiguration<Entities.VehicleType>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.VehicleType> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("vehicle_type", "dbo");

            // key
            builder.HasKey(t => t.VehicleTypeId);

            // properties
            builder.Property(t => t.VehicleTypeId)
                .IsRequired()
                .HasColumnName("vehicle_type_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.VehicleTypeCode)
                .IsRequired()
                .HasColumnName("vehicle_type_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.VehicleTypeName)
                .HasColumnName("vehicle_type_name ")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.VehicleCategory)
                .IsRequired()
                .HasColumnName("vehicle_category")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Inactive)
                .HasColumnName("inactive")
                .HasColumnType("bit");

            // relationships
            #endregion
        }

    }
}
