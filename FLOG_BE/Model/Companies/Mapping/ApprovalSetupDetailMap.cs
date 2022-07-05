using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ApprovalSetupDetailMap : IEntityTypeConfiguration<Entities.ApprovalSetupDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ApprovalSetupDetail> builder)
        {

            builder.ToTable("approval_setup_detail", "dbo");

            builder.HasKey(t => t.ApprovalSetupDetailId);

            builder.Property(t => t.ApprovalSetupDetailId)
               .IsRequired()
               .HasColumnName("approval_setup_detail_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.ApprovalSetupHeaderId)
               .IsRequired()
               .HasColumnName("approval_setup_header_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50);


            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(200)")
                .HasMaxLength(50);

            builder.Property(t => t.PersonId)
                .IsRequired()
                .HasColumnName("person_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.PersonCategoryId)
                .IsRequired()
                .HasColumnName("person_category_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Level)
                .IsRequired()
                .HasColumnName("level")
                .HasColumnType("int");

            builder.Property(t => t.HasLimit)
                .IsRequired()
                .HasColumnName("has_limit")
                .HasColumnType("bit");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("int");


            builder.Property(t => t.ApprovalLimit)
                .IsRequired()
                .HasColumnName("approval_limit")
                .HasColumnType("decimal");

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
