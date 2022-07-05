using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.ApPayment.PostSubmitApprovalDetail
{
    public class Response
    {
        public Guid PaymentHeaderId { get; set; }
        public string Message { get; set; }
    }


}

