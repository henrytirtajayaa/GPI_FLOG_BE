using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.UserGroup.GetUserGroup
{
    public class Response
    {
        public List<ReponseItem> UserGroups { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public string UserGroupId { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
