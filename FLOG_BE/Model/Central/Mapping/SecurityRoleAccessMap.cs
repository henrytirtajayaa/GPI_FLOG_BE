using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class SecurityRoleAccessMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.SecurityRoleAccess>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.SecurityRoleAccess> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("security_role_access", "dbo");

            // key
            builder.HasKey(t => new { t.SecurityRoleAccessId });

            // properties
            builder.Property(t => t.SecurityRoleAccessId)
                .IsRequired()
                .HasColumnName("security_role_access_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.SecurityRoleId)
                .IsRequired()
                .HasColumnName("security_role_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.FormId)
                .IsRequired()
                .HasColumnName("form_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

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
          
            builder.Property(t => t.AllowNew)
                .HasColumnName("AllowNew")
                .HasColumnType("boolean");

            builder.Property(t => t.AllowOpen)
                .HasColumnName("AllowOpen")
                .HasColumnType("boolean");

            builder.Property(t => t.AllowEdit)
               .HasColumnName("AllowEdit")
               .HasColumnType("boolean");

            builder.Property(t => t.AllowDelete)
               .HasColumnName("AllowDelete")
               .HasColumnType("boolean");
            
            builder.Property(t => t.AllowPost)
               .HasColumnName("AllowPost")
               .HasColumnType("boolean");

            builder.Property(t => t.AllowPrint)
               .HasColumnName("AllowPrint")
               .HasColumnType("boolean");

            // relationships
            #endregion
        }

    }
}
