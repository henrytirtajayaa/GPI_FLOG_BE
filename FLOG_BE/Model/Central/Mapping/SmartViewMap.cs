using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class SmartviewMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.SmartView>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.SmartView> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("smartview", "dbo");
            //smartview_id, group_name, smart_title, sql_view_name, is_function

            // key
            builder.HasKey(t => t.SmartviewId);
            builder.Property(t => t.SmartviewId)
               .IsRequired()
               .HasColumnName("smartview_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            // properties
            builder.Property(t => t.GroupName)
                .IsRequired()
                .HasColumnName("group_name")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.SmartTitle)
                .HasColumnName("smart_title")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.SqlViewName)
                .HasColumnName("sql_view_name")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.isFunction)
                .HasColumnName("is_function")
                .HasColumnType("bit");

            #endregion
        }

    }
}
