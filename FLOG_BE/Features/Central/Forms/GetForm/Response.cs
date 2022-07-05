using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.SecurityRoles.Forms.GetForm
{
    public class Response
    {
        public List<ReponseForms> Forms { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseForms
    {
        public string FormId { get; set; }
        public string FormName { get; set; }
        public string Module { get; set; }
    }

}
