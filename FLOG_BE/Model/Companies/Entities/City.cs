using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class City
    {
        public City()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid CityId { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public Guid CountryID { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Country Country { get; set; }
        #endregion

        [IgnoreDataMember]
        public virtual List<Bank> Banks { get; set; }
        [IgnoreDataMember]
        public virtual List<ContainerDepot> ContainerDepots { get; set; }

        public virtual List<Port> Ports { get; set; }
        
    }
}
