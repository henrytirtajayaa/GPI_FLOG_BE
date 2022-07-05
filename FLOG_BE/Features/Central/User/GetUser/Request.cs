using System;
using System.Collections.Generic;
using Infrastructure.Mediator;
using Newtonsoft.Json;

namespace FLOG_BE.Features.Central.User.GetUser
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
        public List<string> UserFullName { get; set; }
        public List<string> EmailAddress { get; set; }
        public List<string> UserGroupCode { get; set; }
        public bool? InActive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDate { get; set; }
    }

    public class CreatedDatePeriode
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
