using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.SecurityRoles.GetSecuritySmartView
{
    public class Response
    {
        public List<ReponseItem> SmartRoles { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public Guid Id { get; set; }
        public Guid SmartviewId { get; set; }
        public Guid SecurityRoleId { get; set; }
        public SmartView SmartView { get; set; }



    }

}
