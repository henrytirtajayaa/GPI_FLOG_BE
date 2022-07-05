using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Checkbook.PutTransaction
{
    public class Response
    {
        public Guid CheckbookTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }



    }


}
