﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.DepositSettlement.PostDepositSettlement
{
    public class Response
    {
        public Guid SettlementHeaderId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DepositNo { get; set; }
    }
}
