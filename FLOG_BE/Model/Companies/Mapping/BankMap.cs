using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class BankMap : IEntityTypeConfiguration<Entities.Bank>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Bank> builder)
        {
            builder.ToTable("bank", "dbo");

            builder.HasKey(t => t.BankId);

            builder.Property(t => t.BankId)
               .IsRequired()
               .HasColumnName("bank_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.BankCode)
                .IsRequired()
                .HasColumnName("bank_code")
                .HasColumnType("varchar(100)");


            builder.Property(t => t.BankName)
                .IsRequired()
                .HasColumnName("bank_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);


            builder.Property(t => t.Address)
                .IsRequired()
                .HasColumnName("address")
                .HasColumnType("varchar(300)");

            builder.Property(t => t.CityCode)
                .IsRequired()
                .HasColumnName("city_code")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.InActive)
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
