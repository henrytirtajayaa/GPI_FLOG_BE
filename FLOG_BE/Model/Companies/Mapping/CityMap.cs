using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CityMap : IEntityTypeConfiguration<Entities.City>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.City> builder)
        {
            builder.ToTable("city", "dbo");

            builder.HasKey(t => t.CityId);

            builder.Property(t => t.CityId)
               .IsRequired()
               .HasColumnName("city_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CityCode)
                .IsRequired()
                .HasColumnName("city_code")
                .HasColumnType("varchar(50)");


            builder.Property(t => t.CityName)
                .IsRequired()
                .HasColumnName("city_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);


            builder.Property(t => t.Province)
                .IsRequired()
                .HasColumnName("province")
                .HasColumnType("varchar(100)");

            builder.Property(t => t.CountryID)
                .IsRequired()
                .HasColumnName("country_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.Inactive)
                .IsRequired()
                .HasColumnName("inactive")
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
