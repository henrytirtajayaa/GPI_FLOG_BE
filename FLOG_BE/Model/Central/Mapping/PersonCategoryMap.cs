using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class PersonCategoryMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.PersonCategory>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.PersonCategory> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("person_category", "dbo");

            // key
            builder.HasKey(t => t.PersonCategoryId);

            // properties
            builder.Property(t => t.PersonCategoryId)
                .IsRequired()
                .HasColumnName("person_category_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.PersonCategoryCode)
               .HasColumnName("person_category_code")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.PersonCategoryName)
                .IsRequired()
                .HasColumnName("person_category_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("datetime");

            builder.Property(t => t.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.UpdatedDate)
                .HasColumnName("updated_date")
                .HasColumnType("datetime");

            // relationships
            #endregion
        }

    }
}
