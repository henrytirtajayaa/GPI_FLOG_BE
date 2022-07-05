using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CompanyAddressMap : IEntityTypeConfiguration<Entities.CompanyAddress>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CompanyAddress> builder)
        {
            builder.ToTable("company_address", "dbo");

            builder.HasKey(t => t.CompanyAddressId);

            builder.Property(t => t.CompanyAddressId)
                .IsRequired()
                .HasColumnName("company_address_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.AddressCode)
                .IsRequired()
                .HasColumnName("address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.AddressName)
                .IsRequired()
                .HasColumnName("address_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.ContactPerson)
                .HasColumnName("contact_person")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.Address)
                .IsRequired()
                .HasColumnName("address")
                .HasColumnType("varchar(300)")
                .HasMaxLength(300);

            builder.Property(t => t.Handphone)
                .HasColumnName("handphone")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Phone1)
                .HasColumnName("phone1")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Extension1)
                .HasColumnName("ext1")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.Phone2)
                .HasColumnName("phone2")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Extension2)
                .HasColumnName("ext2")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.Fax)
                .HasColumnName("fax")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.EmailAddress)
                .HasColumnName("email_address")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.HomePage)
                .HasColumnName("homepage")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.Neighbourhood)
                .HasColumnName("neighbourhood")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.Hamlet)
                .HasColumnName("hamlet")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.UrbanVillage)
                .HasColumnName("urban_village")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.SubDistrict)
                .HasColumnName("sub_district")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CityCode)
                .HasColumnName("city_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.PostCode)
                .HasColumnName("postcode")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10);

            builder.Property(t => t.IsSameAddress)
                .HasColumnName("is_same_address")
                .HasColumnType("bit");

            builder.Property(t => t.TaxAddressId)
                .HasColumnName("tax_address_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Default)
                .HasColumnName("default")
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
        }
    }
}
