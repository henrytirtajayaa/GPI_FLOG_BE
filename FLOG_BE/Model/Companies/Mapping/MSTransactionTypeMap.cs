using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class MSTransactionTypeMap : IEntityTypeConfiguration<Entities.MSTransactionType>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.MSTransactionType> builder)
        {
            builder.ToTable("ms_transaction_type", "dbo");
            builder.HasKey(t => t.TransactionTypeId);

            builder.Property(t => t.TransactionTypeId)
                .IsRequired()
                .HasColumnName("transaction_type_id")
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())");

            builder.Property(t => t.TransactionType)
                .IsRequired()
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.TransactionName)
                .IsRequired()
                .HasColumnName("transaction_name")
                .HasColumnType("varchar(250)")
                .HasMaxLength(250);

            builder.Property(t => t.PaymentCondition)
                .HasColumnName("payment_condition")
                .HasColumnType("int");

            builder.Property(t => t.RequiredSubject)
                .HasColumnName("required_subject")
                .HasColumnType("int");

            builder.Property(t => t.InActive)
                .HasColumnName("inactive")
                .HasColumnType("bit");
        }
    }
}
