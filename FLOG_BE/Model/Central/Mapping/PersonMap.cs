using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class PersonMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.Person>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.Person> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("person", "dbo");

            // key
            builder.HasKey(t => t.PersonId);

            // properties
            builder.Property(t => t.PersonId)
                .IsRequired()
                .HasColumnName("person_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.PersonFullName)
                .IsRequired()
                .HasColumnName("person_full_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.PersonPassword)
                .IsRequired()
                .HasColumnName("person_password")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.EmailAddress)
                .IsRequired()
                .HasColumnName("email_address")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.PersonCategoryId)
                .HasColumnName("person_category_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
           
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

            // relationships
            #endregion
        }

    }
}
