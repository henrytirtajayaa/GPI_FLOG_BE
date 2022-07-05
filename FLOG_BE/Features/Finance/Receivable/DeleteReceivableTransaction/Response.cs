using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Receivable.DeleteReceivableTransaction
{
    public class Response
    {
        public Guid ReceiveTransactionId { get; set; }
        public String Message { get; set; }
    }
}
