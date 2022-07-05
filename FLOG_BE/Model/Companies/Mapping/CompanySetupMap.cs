using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CompanySetupMap : IEntityTypeConfiguration<Entities.CompanySetup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CompanySetup> builder)
        {
            builder.ToTable("company_setup", "dbo");
            builder.HasKey(t => t.CompanySetupId);
            builder.Property(t => t.CompanySetupId)
               .IsRequired()
               .HasColumnName("company_setup_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CompanyId)
                .HasColumnName("company_id")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.CompanyName)
                .HasColumnName("company_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CompanyAddressId)
                .IsRequired()
                .HasColumnName("company_address_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TaxRegistrationNo)
                .HasColumnName("tax_registration_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CompanyTaxName)
                .HasColumnName("company_tax_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CompanyLogo)
                .HasColumnName("company_logo")
                .HasColumnType("varchar(150)")
                .HasMaxLength(150);

            builder.Property(t => t.CreatedBy)
                .IsRequired()
                .HasColumnName("created_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CreatedDate)
                .IsRequired()
                .HasColumnName("created_date")
                .HasColumnType("datetime");

            builder.Property(t => t.ModifiedBy)
                .HasColumnName("modified_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ModifiedDate)
                .HasColumnName("modified_date")
                .HasColumnType("datetime");

            builder.Property(t => t.LogoImageType)
                .HasColumnName("logo_image_type")
                .HasColumnType("nvarchar(100)");

            builder.Property(t => t.LogoImageTitle)
                .HasColumnName("logo_image_title")
                .HasColumnType("nvarchar(100)");

            builder.Property(t => t.LogoImageData)
                .HasColumnName("logo_image_data");
        }

    }
}
