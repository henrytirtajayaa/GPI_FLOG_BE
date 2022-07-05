using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class SmartRoleMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.SmartRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.SmartRole> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("role_smartview", "dbo");
           
            // key
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
               .IsRequired()
               .HasColumnName("id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            // properties
            builder.Property(t => t.SmartviewId)
                .IsRequired()
                .HasColumnName("smartview_id")
                .HasColumnType("uniqueidentifier");


            builder.Property(t => t.SecurityRoleId)
                .HasColumnName("security_role_id")
                .HasColumnType("uniqueidentifier");

            #endregion
        }

    }
}
