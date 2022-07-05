using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Company.GetCompany
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> CompanyName { get; set; }
        public List<string> DatabaseId { get; set; }
        public List<string> DatabaseAddress { get; set; }
        public List<string> DatabasePassword { get; set; }
        public List<string> CoaSymbol { get; set; }
        public List<int> CoaTotalLengthMin { get; set; }
        public List<int> CoaTotalLengthMax { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }

        public bool? InActive { get; set; }
    }
}
