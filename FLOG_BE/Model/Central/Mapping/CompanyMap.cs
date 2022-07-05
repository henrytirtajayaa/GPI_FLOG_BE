using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class CompanyMap
        : IEntityTypeConfiguration<Entities.Company>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Company> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("company", "dbo");

            // key
            builder.HasKey(t => t.CompanyId);

            // properties
            builder.Property(t => t.CompanyId)
                .IsRequired()
                .HasColumnName("company_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CompanyName)
                .IsRequired()
                .HasColumnName("company_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.DatabaseId)
                .IsRequired()
                .HasColumnName("database_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DatabaseAddress)
                .IsRequired()
                .HasColumnName("database_address")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.DatabasePassword)
                .HasColumnName("database_password")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CoaSymbol)
            .IsRequired()
            .HasColumnName("coa_symbol")
            .HasColumnType("char(1)")
            .HasMaxLength(1);

            builder.Property(t => t.CoaTotalLength)
            .IsRequired()
            .HasColumnName("coa_total_length")
            .HasColumnType("integer");  

            builder.Property(t => t.InActive)
                .IsRequired()
                .HasColumnName("in_active")
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

            builder.Property(t => t.SmtpServer)
                .HasColumnName("smtp_server")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.SmtpPort)
                .HasColumnName("smtp_port")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            builder.Property(t => t.SmtpUser)
                .HasColumnName("smtp_user")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.SmtpPassword)
                .HasColumnName("smtp_password")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);
            // relationships
            #endregion
        }

    }
}
