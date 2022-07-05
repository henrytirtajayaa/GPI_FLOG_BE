using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Central.Mapping
{
    public partial class SessionStateMap
        : IEntityTypeConfiguration<FLOG_BE.Model.Central.Entities.SessionState>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FLOG_BE.Model.Central.Entities.SessionState> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("session_state", "dbo");

            // key
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
               .IsRequired()
               .HasColumnName("id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            // properties
            builder.Property(t => t.PersonId)
                .IsRequired()
                .HasColumnName("person_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CompanySecurityId)
                .HasColumnName("company_security_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CompanyId)
                .HasColumnName("company_id")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("datetime");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("integer");

            #endregion
        }

    }
}
