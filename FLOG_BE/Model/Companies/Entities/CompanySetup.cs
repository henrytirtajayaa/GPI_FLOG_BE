using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class CompanySetup
    {
        public CompanySetup()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid CompanySetupId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid CompanyAddressId { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string CompanyTaxName { get; set; }
        public string CompanyLogo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string LogoImageType { get; set; }
        public string LogoImageTitle { get; set; }
        public byte[] LogoImageData { get; set; }

        #endregion

        [NotMapped]
        public  CompanyAddress CompanyAddress { get; set; }

        [NotMapped]
        public string LogoImageUrl { get; set; }
    }
}
