using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class TrxDocumentApprovalCommentMap : IEntityTypeConfiguration<Entities.TrxDocumentApprovalComment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.TrxDocumentApprovalComment> builder)
        {
            builder.ToTable("trx_document_approval_comment", "dbo");
            builder.HasKey(t => t.TrxDocumentApprovalCommentId);

            builder.Property(t => t.TrxDocumentApprovalCommentId)
                .IsRequired()
                .HasColumnName("trx_document_approval_comment_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TrxDocumentApprovalId)
                .IsRequired()
                .HasColumnName("trx_document_approval_id")
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
