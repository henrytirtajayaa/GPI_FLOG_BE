using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Mapping
{
    public class SalesOrderHeaderMap : IEntityTypeConfiguration<Entities.SalesOrderHeader>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.SalesOrderHeader> builder)
        {
            builder.ToTable("sales_order_header", "dbo");
            
            builder.HasKey(t => t.SalesOrderId);
            builder.Property(t => t.SalesOrderId)
               .IsRequired()
               .HasColumnName("sales_order_id")
               .HasColumnType("uniqueidentifier")
               .HasMaxLength(50)
               .HasDefaultValueSql("(newid())");

            builder.Property(t => t.RowId)
                .HasColumnName("row_id")
                .UseSqlServerIdentityColumn()
                .HasColumnType("bigint");

            builder.Property(t => t.TransactionType)
                .HasColumnName("transaction_type")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.TransactionDate)
                .HasColumnName("transaction_date")
                .HasColumnType("datetime");

            builder.Property(t => t.DocumentNo)
                .HasColumnName("document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.BranchCode)
                .HasColumnName("branch_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CustomerId)
                .IsRequired()
                .HasColumnName("customer_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.CustomerAddressCode)
                .HasColumnName("customer_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.CustomerBillToAddressCode)
                .HasColumnName("customer_billto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.CustomerShipToAddressCode)
                .HasColumnName("customer_shipto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.QuotDocumentNo)
                .HasColumnName("quot_document_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.SalesCode)
                .HasColumnName("sales_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.ShipmentStatus)
                .HasColumnName("shipment_status")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.MasterNo)
                .HasColumnName("master_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            builder.Property(t => t.AgreementNo)
                .HasColumnName("agreement_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.BookingNo)
                .HasColumnName("booking_no")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.HouseNo)
                .HasColumnName("house_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);
            
            builder.Property(t => t.ShipperId)
                .IsRequired()
                .HasColumnName("shipper_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.ShipperAddressCode)
                .HasColumnName("shipper_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            builder.Property(t => t.ShipperBillToAddressCode)
                .HasColumnName("shipper_billto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.ShipperShipToAddressCode)
                .HasColumnName("shipper_shipto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.ConsigneeId)
                .IsRequired()
                .HasColumnName("consignee_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.ConsigneeAddressCode)
                .HasColumnName("consignee_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.ConsigneeBillToAddressCode)
                .HasColumnName("consignee_billto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.ConsigneeShipToAddressCode)
                .HasColumnName("consignee_shipto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(t => t.IsDifferentNotifyPartner)
                .HasColumnName("is_different_notify_partner")
                .HasColumnType("bit");

            builder.Property(t => t.NotifyPartnerId)
                .IsRequired()
                .HasColumnName("notify_partner_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.NotifyPartnerAddressCode)
                .HasColumnName("notify_partner_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            builder.Property(t => t.NotifyPartnerBilltoAddressCode)
                .HasColumnName("notify_partner_billto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            builder.Property(t => t.NotifyPartnerShipToAddressCode)
                .HasColumnName("notify_partner_shipto_address_code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
            
            builder.Property(t => t.ShippingLineId)
                .IsRequired()
                .HasColumnName("shipping_line_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.IsShippingLineMaster)
                .HasColumnName("is_shipping_line_master")
                .HasColumnType("bit");

            builder.Property(t => t.ShippingLineCode)
                .HasColumnName("shipping_line_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.ShippingLineName)
                .HasColumnName("shipping_line_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.ShippingLineVesselCode)
                .HasColumnName("shipping_line_vessel_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.ShippingLineVesselName)
                .HasColumnName("shipping_line_vessel_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.ShippingLineShippingNo)
                .HasColumnName("shipping_line_shipping_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.ShippingLineDelivery)
               .HasColumnName("shipping_line_delivery")
               .HasColumnType("datetime");
            
            builder.Property(t => t.ShippingLineArrival)
               .HasColumnName("shipping_line_arrival")
               .HasColumnType("datetime");
            
            builder.Property(t => t.FeederLineId)
                .IsRequired()
                .HasColumnName("feeder_line_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(t => t.IsFeederLineMaster)
                .HasColumnName("is_feeder_line_master")
                .HasColumnType("bit");

            builder.Property(t => t.FeederLineCode)
                .HasColumnName("feeder_line_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.FeederLineName)
                .HasColumnName("feeder_line_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.FeederLineVesselCode)
                .HasColumnName("feeder_line_vessel_code")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.FeederLineVesselName)
                .HasColumnName("feeder_line_vessel_name")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(t => t.FeederLineShippingNo)
                .HasColumnName("feeder_line_shipping_no")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.FeederLineDelivery)
               .HasColumnName("feeder_line_delivery")
               .HasColumnType("datetime");
            
            builder.Property(t => t.FeederLineArrival)
               .HasColumnName("feeder_line_arrival")
               .HasColumnType("datetime");

            builder.Property(t => t.TermOfShipment)
               .HasColumnName("term_of_shipment")
                .HasColumnType("varchar(50)")
               .HasMaxLength(50);
            
            builder.Property(t => t.FinalDestination)
               .HasColumnName("final_destination")
                .HasColumnType("varchar(250)")
               .HasMaxLength(250);
            
            builder.Property(t => t.PortOfLoading)
               .HasColumnName("port_of_loading")
                .HasColumnType("varchar(50)")
               .HasMaxLength(50);
            
            builder.Property(t => t.PortOfDischarge)
               .HasColumnName("port_of_discharge")
                .HasColumnType("varchar(50)")
               .HasMaxLength(50);
            
            builder.Property(t => t.Commodity)
               .HasColumnName("commodity")
                .HasColumnType("text");
              
            builder.Property(t => t.CargoGrossWeight)
               .HasColumnName("cargo_gross_weight")
                .HasColumnType("varchar(a00)")
               .HasMaxLength(100);

             builder.Property(t => t.CargoNetWeight)
               .HasColumnName("cargo_net_weight")
                .HasColumnType("varchar(100)")
               .HasMaxLength(100);
            builder.Property(t => t.CargoDescription)
               .HasColumnName("cargo_description")
                .HasColumnType("text");
            
            builder.Property(t => t.TotalFuncSelling)
               .HasColumnName("total_func_selling")
                .HasColumnType("decimal");

            builder.Property(t => t.TotalFuncBuying)
               .HasColumnName("total_func_buying")
                .HasColumnType("decimal");

            builder.Property(t => t.Remark)
               .HasColumnName("remark")
                .HasColumnType("text");
               
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasColumnType("integer");

            builder.Property(t => t.StatusComment)
              .HasColumnName("status_comment")
               .HasColumnType("varchar(255)")
              .HasMaxLength(255);

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
