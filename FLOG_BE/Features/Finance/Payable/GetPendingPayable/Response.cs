﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.Payable.GetPendingPayable
{
    public class Response
    {
        public List<Model.Companies.View.APPayablePending> PayableTransaction { get; set; }
        public List<string> MasterNo { get; set; }
        public List<string> AgreementNo { get; set; }
        public ListInfo ListInfo { get; set; }
    }

}
