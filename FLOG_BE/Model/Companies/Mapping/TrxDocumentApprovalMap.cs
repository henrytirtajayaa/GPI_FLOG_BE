using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class TrxDocumentApprovalMap : IEntityTypeConfiguration<Entities.TrxDocumentApproval>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.TrxDocumentApproval> builder)
        {
            builder.ToTable("trx_document_approval", "dbo");
            builder.HasKey(t => t.TrxDocumentApprovalId);

            builder.Property(t => t.TrxDocumentApprovalId)
                .IsRequired()
                .HasColumnName("trx_document_approval_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TrxModule)
                .HasColumnName("trx_module")
                .HasColumnType("integer");

            builder.Property(t => t.TransactionType)
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.DocFeatureId)
                .HasColumnName("doc_feature_id")
                .HasColumnType("integer");

            builder.Property(t => t.ModeStatus)
                .HasColumnName("mode_status")
                .HasColumnType("integer");

            builder.Property(t => t.TransactionId)
                .IsRequired()
                .HasColumnName("transaction_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Index)
                .HasColumnName("index")
                .HasColumnType("integer");

            builder.Property(t => t.PersonId)
                .HasColumnName("person_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.PersonCategoryId)
                .HasColumnName("person_category_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("integer");
        }
    }
}
