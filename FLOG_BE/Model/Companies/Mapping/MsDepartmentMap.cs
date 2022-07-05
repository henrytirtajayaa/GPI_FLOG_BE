using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class MSDepartmentMap : IEntityTypeConfiguration<Entities.MsDepartment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.MsDepartment> builder)
        {
            builder.ToTable("ms_department", "dbo");
            builder.HasKey(t => t.DepartmentId);

            builder.Property(t => t.DepartmentId)
                .IsRequired()
                .HasColumnName("department_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.DepartmentCode)
                .IsRequired()
                .HasColumnName("department_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DepartmentName)
                .HasColumnName("department_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.Inactive)
                .IsRequired()
                .HasColumnName("inactive")
                .HasColumnType("bit");

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
