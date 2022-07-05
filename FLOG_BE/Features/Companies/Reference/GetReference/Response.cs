using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Reference.GetReference
{
    public class Response
    {
        public List<ResponseItem> References { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceCode { get; set; }
        public string ReferenceName { get; set; }
        public bool Inactive { get; set; }
    }
}
