using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.User.PutUser
{
    public class Response
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserPassword { get; set; }
        public string EmailAddress { get; set; }
        public string UserCategoryCode { get; set; }
        public bool InActive { get; set; }
    }


}
