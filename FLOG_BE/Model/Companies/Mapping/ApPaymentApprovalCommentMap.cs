using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class ApPaymentApprovalCommentMap : IEntityTypeConfiguration<Entities.ApPaymentApprovalComment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.ApPaymentApprovalComment> builder)
        {


            //approval_comment_id, payment_approval_id, status, person_id, comment_date, comments, row_id

            builder.ToTable("ap_payment_approval_comment", "dbo");
            builder.HasKey(t => t.PaymentApprovalCommentId);

            builder.Property(t => t.PaymentApprovalCommentId)
                .IsRequired()
                .HasColumnName("approval_comment_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.PaymentApprovalId)
                .IsRequired()
                .HasColumnName("payment_approval_id")
                .HasColumnType("uniqueidentifier");


            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(t => t.PersonId)
                .IsRequired()
                .HasColumnName("Person_id")
                .HasColumnType("uniqueidentifier");


            builder.Property(t => t.CommentDate)
                .IsRequired()
                .HasColumnName("comment_date")
                .HasColumnType("Datetime");

            builder.Property(t => t.Comments)
                .HasColumnName("comments")
                .HasColumnType("varchar(max)");

            builder.Property(t => t.RowId)
                  .HasColumnName("row_id")
                  .UseSqlServerIdentityColumn()
                  .HasColumnType("bigint");

            builder.Property(p => p.RowId)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;


        }
    }
}
