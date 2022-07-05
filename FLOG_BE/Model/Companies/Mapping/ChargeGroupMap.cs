using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ChargeGroupMap : IEntityTypeConfiguration<Entities.ChargeGroup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ChargeGroup> builder)
        {

            //, , 

            builder.ToTable("charge_group", "dbo");
            builder.HasKey(t => t.ChargeGroupId);
            builder.Property(t => t.ChargeGroupId)
               .IsRequired()
               .HasColumnName("charge_group_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ChargeGroupCode)
                .IsRequired()
                .HasColumnName("charge_group_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.ChargeGroupName)
                .IsRequired()
                .HasColumnName("charge_group_name")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

           
        }

    }
}
