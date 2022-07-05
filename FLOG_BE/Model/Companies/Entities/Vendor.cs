using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string AddressCode { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string VendorTaxName { get; set; }
        public string VendorGroupCode { get; set; }
        public string PaymentTermCode { get; set; }
        public bool HasCreditLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public string ShipToAddressCode { get; set; }
        public string BillToAddressCode { get; set; }
        public string TaxAddressCode { get; set; }
        public string PayableAccountNo { get; set; }
        public string AccruedPayableAccountNo { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        [NotMapped]
        public virtual List<VendorAddress> VendorAddresses { get; set; }
        [NotMapped]
        public VendorAddress VendorAddress { get; set; }
        public virtual List<CustomerVendorRelation> CustomerVendorRelations { get; set; }

        [IgnoreDataMember]
        public virtual List<ShippingLine> ShippingLines { get; set; }

    }
}
