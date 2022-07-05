﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.Payable.PostPayable
{
    public class Response
    {
        public Guid PayableTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
    }
}
