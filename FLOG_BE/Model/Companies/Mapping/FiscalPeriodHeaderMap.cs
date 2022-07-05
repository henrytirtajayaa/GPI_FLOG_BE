using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class FiscalPeriodHeaderMap : IEntityTypeConfiguration<Entities.FiscalPeriodHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.FiscalPeriodHeader> builder)
        {
            builder.ToTable("fiscal_period_header", "dbo");

            builder.HasKey(t => t.FiscalHeaderId);
            builder.Property(t => t.FiscalHeaderId)
               .IsRequired()
               .HasColumnName("fiscal_header_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");


            builder.Property(t => t.PeriodYear)
                .IsRequired()
                .HasColumnName("period_year")
                .HasColumnType("int");

            builder.Property(t => t.TotalPeriod)
                .IsRequired()
                .HasColumnName("total_period")
                .HasColumnType("int");
           
            builder.Property(t => t.StartDate)
                .IsRequired()
                .HasColumnName("start_date")
                .HasColumnType("Datetime");

            builder.Property(t => t.EndDate)
                .IsRequired()
                .HasColumnName("end_date")
                .HasColumnType("Datetime");

            builder.Property(t => t.ClosingYear)
                .IsRequired()
                .HasColumnName("closing_year")
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
