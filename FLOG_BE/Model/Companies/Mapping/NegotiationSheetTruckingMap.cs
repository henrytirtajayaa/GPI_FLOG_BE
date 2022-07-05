﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class NegotiationSheetTruckingMap : IEntityTypeConfiguration<Entities.NegotiationSheetTrucking>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.NegotiationSheetTrucking> builder)
        {


            builder.ToTable("negotiation_sheet_trucking", "dbo");
            builder.HasKey(t => t.NsTruckingId);
            builder.Property(t => t.NsTruckingId)
                .IsRequired()
                .HasColumnName("ns_trucking_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                 .HasColumnName("row_id")
                 .UseSqlServerIdentityColumn()
                 .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                            .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Property(t => t.NegotiationSheetId)
                .HasColumnName("negotiation_sheet_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.VehicleTypeId)
               .HasColumnName("vehicle_type_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.TruckloadTerm)
               .HasColumnName("truckload_term")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.VendorId)
               .HasColumnName("vendor_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Qty)
               .HasColumnName("qty")
               .HasColumnType("int");

            builder.Property(t => t.UomDetailId)
               .HasColumnName("uom_detail_id")
               .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Status)
               .HasColumnName("status")
               .HasColumnType("integer");

            builder.Property(t => t.RowIndex)
               .HasColumnName("row_index")
               .HasColumnType("int");

        }
    }
}
