using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class FormMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.Form>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.Form> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("form", "dbo");

            // key
            builder.HasKey(t => t.FormId);

            // properties
            builder.Property(t => t.FormId)
                .IsRequired()
                .HasColumnName("form_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.FormName)
                .IsRequired()
                .HasColumnName("form_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.FormLink)
                .HasColumnName("form_link")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.ParentId)
                .HasColumnName("form_parent_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.MenuIcon)
                .HasColumnName("form_menu_icon")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50); 

            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasColumnType("int");

            builder.Property(t => t.Module)
                .IsRequired()
                .HasColumnName("module")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.IsVisible)
                .HasColumnName("is_visible")
                .HasColumnType("bit");

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
