using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Central.Entities;
namespace FLOG_BE.Model.Central.Mapping
{
    public partial class CompanySecurityMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.CompanySecurity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.CompanySecurity> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("company_security", "dbo");

            // key
            builder.HasKey(t => new { t.CompanySecurityId });

            // properties

            builder.Property(t => t.CompanySecurityId)
                .IsRequired()
                .HasColumnName("company_security_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PersonId)
                .IsRequired()
                .HasColumnName("person_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CompanyId)
                .IsRequired()
                .HasColumnName("company_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.SecurityRoleId)
                .IsRequired()
                .HasColumnName("security_role_id")
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


            #endregion
        }

    }
}
