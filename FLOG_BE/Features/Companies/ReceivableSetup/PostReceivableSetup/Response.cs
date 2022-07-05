using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.ReceivableSetup.PostReceivableSetup
{
    public class Response
    {
        public string ReceivableSetupId { get; set; }
        public string TransactionType { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public bool AgingByDocdate { get; set; }
        public string WriteoffAccountNo { get; set; }
        public decimal WriteoffLimit { get; set; }
        public string DiscountAccountNo { get; set; }
        public string NSInvoiceDocNo { get; set; }
        public string ReceivableInvoiceDocNo { get; set; }
        public string ReceivableDebitNoteDocNo { get; set; }
        public string ReceivableCreditNoteDocNo { get; set; }
        public string ReceivableWriteoffDocNo { get; set; }
        public string ReceivableWhtDocNo { get; set; }
        public string AdvanceReceiptDocNo { get; set; }
        public string AdvancePaymentDocNo { get; set; }
        public string SoaDebitNoteNo { get; set; }
        public string SoaCreditNoteNo { get; set; }
        public string AdvanceRequestDocNo { get; set; }
        public string AdvanceRealizationDocNo { get; set; }
        public string AdvanceCreditNoteDocNo { get; set; }
        public string ApplyDocNo { get; set; }
    }
}
