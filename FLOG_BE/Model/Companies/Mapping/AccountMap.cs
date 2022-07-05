using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class AccountMap : IEntityTypeConfiguration<Entities.Account>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Account> builder)
        {
            builder.ToTable("account", "dbo");

            builder.HasKey(t => t.AccountId);

            builder.Property(t => t.AccountId)
               .IsRequired()
               .HasColumnName("account_id")
               .HasColumnType("varchar(50)")
               .HasMaxLength(50);

            builder.Property(t => t.Segment1)
                .IsRequired()
                .HasColumnName("segment1")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment2)
                .IsRequired()
                .HasColumnName("segment2")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment3)
                .IsRequired()
                .HasColumnName("segment3")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment4)
                .IsRequired()
                .HasColumnName("segment4")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment5)
                .IsRequired()
                .HasColumnName("segment5")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment6)
                .IsRequired()
                .HasColumnName("segment6")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment7)
                .IsRequired()
                .HasColumnName("segment7")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment8)
                .IsRequired()
                .HasColumnName("segment8")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment9)
                .IsRequired()
                .HasColumnName("segment9")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.Segment10)
                .IsRequired()
                .HasColumnName("segment10")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50); 

            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.PostingType)
                .IsRequired()
                .HasColumnName("posting_type")
                .HasColumnType("bit");

            builder.Property(t => t.NormalBalance)
                .IsRequired()
                .HasColumnName("normal_balance")
                .HasColumnType("bit");

            builder.Property(t => t.NoDirectEntry)
                .IsRequired()
                .HasColumnName("no_direct_entry")
                .HasColumnType("bit");

            builder.Property(t => t.Revaluation)
                .IsRequired()
                .HasColumnName("revaluation")
                .HasColumnType("bit");

            builder.Property(t => t.Inactive)
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
