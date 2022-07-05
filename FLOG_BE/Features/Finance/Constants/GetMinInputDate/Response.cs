using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Constants.GetMinInputDate
{
    public class Response
    {
        public DateTime MinInputDate { get; set; }
        public List<Model.Companies.Entities.CompanyBranch> CompanyBranches { get; set; }
        public ListInfo ListInfo { get; set; }
    }

}
