using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.ApPayment.GetPaymentDetail
{
    public class Response
    {
        public List<ResponseDetailItem> PaymentDetail { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseDetailItem
    {
        public Guid PaymentHeaderId { get; set; }
        public Guid PayableTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FuctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FuctionalPaid { get; set; }
        public int Status { get; set; }

    }
   

}
