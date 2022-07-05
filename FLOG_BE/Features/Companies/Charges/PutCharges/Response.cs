using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Charges.PutCharges
{
    public class Response
    {
        public string ChargesId { get; set; }
        public string ChargesCode { get; set; }
        public string ChargesName { get; set; }
    }


}
