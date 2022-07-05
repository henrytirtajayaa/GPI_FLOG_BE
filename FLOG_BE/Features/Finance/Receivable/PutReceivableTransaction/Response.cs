using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Receivable.PutReceivableTransaction
{
    public class Response
    {
        public Guid ReceiveTransactionId { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public Guid CustomerId { get; set; }
        public string PaymentTermCode { get; set; }
        public string CustomerAddressCode { get; set; }
        public string SoDocumentNo { get; set; }
        public string NsDocumentNo { get; set; }
        public string Description { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Modifield { get; set; }
        public int Status { get; set; }
    }
}
