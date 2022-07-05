using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.BankReconcile.PutStatusBankReconcile
{
    public class Response
    {
        public Guid BankReconcileId { get; set; }
        public string Message { get; set; }
    }


}
