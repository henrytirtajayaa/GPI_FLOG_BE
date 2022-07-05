using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Receivable.PutTrxDelete
{
    public class Response
    {
        public Guid ReceiveTransactionId { get; set; }
        public int Status { get; set; }

    }


}
