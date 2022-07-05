using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.UserGroup.GetUserGroup
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
        public List<string> UserGroupCode { get; set; }
        public List<string> UserGroupName { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> UpdatedBy { get; set; }
        public List<DateTime?> UpdatedDateStart { get; set; }
        public List<DateTime?> UpdatedDateEnd { get; set; }
    }
}
