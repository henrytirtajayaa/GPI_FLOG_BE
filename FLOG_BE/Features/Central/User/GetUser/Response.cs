using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.User.GetUser
{
    public class Response
    {
        public List<ReponseItem> Users { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string EmailAddress { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupId { get; set; }
        public string UserPassword { get; set; }
        public bool? InActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
