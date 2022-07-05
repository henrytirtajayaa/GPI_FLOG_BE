using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class CheckbookApprovalCommentMap : IEntityTypeConfiguration<Entities.CheckbookApprovalComment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.CheckbookApprovalComment> builder)
        {
            builder.ToTable("checkbook_approval_comment", "dbo");
            builder.HasKey(t => t.ApprovalCommentId);

            builder.Property(t => t.ApprovalCommentId)
                .IsRequired()
                .HasColumnName("approval_comment_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.CheckbookTransactionApprovalId)
                .IsRequired()
                .HasColumnName("checkbook_transaction_approval_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("integer");

            builder.Property(t => t.PersonId)
                .HasColumnName("person_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.CommentDate)
                .HasColumnName("comment_date")
                .HasColumnType("datetime");

            builder.Property(t => t.Comments)
                .HasColumnName("comments")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);
        }
    }
}
