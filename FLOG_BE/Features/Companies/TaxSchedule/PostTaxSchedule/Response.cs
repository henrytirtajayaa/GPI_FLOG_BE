using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.TaxSchedule.PostTaxSchedule
{
    public class Response
    {
        public Guid TaxScheduleId { get; set; }
        public string TaxScheduleCode { get; set; }
        public string Description { get; set; }
       

    }


}
