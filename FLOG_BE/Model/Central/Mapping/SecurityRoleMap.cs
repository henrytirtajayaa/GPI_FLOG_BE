using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class SecurityRoleMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.SecurityRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.SecurityRole> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("security_role", "dbo");

            // key
            builder.HasKey(t => t.SecurityRoleId);

            // properties
            builder.Property(t => t.SecurityRoleId)
                .IsRequired()
                .HasColumnName("security_role_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.SecurityRoleCode)
                .IsRequired()
                .HasColumnName("security_role_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.SecurityRoleName)
                .IsRequired()
                .HasColumnName("security_role_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

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
            #endregion
        }

    }
}
