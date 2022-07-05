using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Log;
using Microsoft.Extensions.Configuration;
using FLOG_BE.Model.Companies;
using FLOG_BE.Helper;
using System.Data;
using System.Text;
using Entities = FLOG_BE.Model.Companies.Entities;
using FlogEntities = FLOG_BE.Model.Central.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using Infrastructure;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG.Core.DocumentNo
{
    public static class DOCNO_FEATURE
    {
        //NO TRX TYPE
        public const int NOTRX_GJE                  = 1;
        public const int NOTRX_RECEIVABLE_APPLY     = 2;
        public const int NOTRX_PAYABLE_APPLY        = 3;
        public const int NOTRX_SOA_NO               = 4;
        public const int NOTRX_DEPOSIT_SETTLEMENT   = 5;

        //BY TRANSACTION TYPE
        public const int TRXTYPE_INVOICE            = 50;
        public const int TRXTYPE_DEBITNOTE          = 51;
        public const int TRXTYPE_CREDITNOTE         = 52;
        public const int TRXTYPE_WRITEOFF           = 53;
        public const int TRXTYPE_WHT                = 54;        
        public const int TRXTYPE_SOA_DEBIT          = 55;
        public const int TRXTYPE_SOA_CREDIT         = 56;
        public const int TRXTYPE_RENTAL_REQUEST     = 57;
        public const int TRXTYPE_RENTAL_DELIVERY    = 58;
        public const int TRXTYPE_RENTAL_CLOSING     = 59;
        public const int TRXTYPE_SALES_QUOT         = 60;
        public const int TRXTYPE_SALES_ORDER        = 61;
        public const int TRXTYPE_SALES_NEGOSHEET    = 62;

        //DEPOSIT TRANSACTION
        public const int TRXTYPE_DEPOSIT_DEMURRAGE   = 80;
        public const int TRXTYPE_CONTAINER_GUARANTEE = 81;
        public const int TRXTYPE_DETENTION           = 82;

        //CHECKBOOK
        public const int CHECKBOOK_IN               = 10;
        public const int CHECKBOOK_OUT              = 11;
        public const int CHECKBOOK_RECONCILE        = 12;
        public const int CHECKBOOK_RECEIPT          = 13;
        public const int CHECKBOOK_PAYMENT          = 14;

        public static string CaptionTrxType(int stat, string customLabel = "")
        {
            string result = "";

            string label = customLabel;
            switch (stat)
            {
                case TRXTYPE_INVOICE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Invoice" : label);
                        break;
                    }
                case TRXTYPE_DEBITNOTE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Debit Note" : label);
                        break;
                    }
                case TRXTYPE_CREDITNOTE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Credit Note" : label);
                        break;
                    }
                case TRXTYPE_WRITEOFF:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Write Off" : label);
                        break;
                    }
                case TRXTYPE_WHT:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "WHT" : label);
                        break;
                    }
                case TRXTYPE_SOA_DEBIT:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Statement of Account - Debit" : label);
                        break;
                    }
                case TRXTYPE_SOA_CREDIT:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Statement of Account - Credit" : label);
                        break;
                    }
                case TRXTYPE_RENTAL_REQUEST:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Rental Request" : label);
                        break;
                    }
                case TRXTYPE_RENTAL_DELIVERY:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Rental Delivery" : label);
                        break;
                    }
                case TRXTYPE_RENTAL_CLOSING:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Rental Closing" : label);
                        break;
                    }
                case TRXTYPE_SALES_QUOT:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Sales Quotation" : label);
                        break;
                    }
                case TRXTYPE_SALES_ORDER:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Sales Order" : label);
                        break;
                    }
                case TRXTYPE_SALES_NEGOSHEET:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Negotiation Sheet" : label);
                        break;
                    }
                case TRXTYPE_DEPOSIT_DEMURRAGE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Demurrage" : label);
                        break;
                    }
                case TRXTYPE_CONTAINER_GUARANTEE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Container Guarantee" : label);
                        break;
                    }
                case TRXTYPE_DETENTION:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Detention" : label);
                        break;
                    }
                default: break;
            }

            return result;
        }
    }

    public interface IDocumentGenerator
    {
        string UniqueDocumentNoByCheckbook(DateTime docDate, string checkbookCode, int docFeatureId, System.Data.Common.DbTransaction trans = null);

        string UniqueDocumentNoByTrxType(DateTime docDate, int trxModule, int docFeatureId, string transactionType, System.Data.Common.DbTransaction trans = null);

        bool DocNoDelete(string documentNo, System.Data.Common.DbTransaction trans = null);

        List<TrxPersonApprover> GetCurrentDocumentApprovers(int trxModule, int docFeatureId, string transactionType, Guid transactionId);

        List<Entities.TrxDocumentApprovalComment> GetDocumentApprovalComments(int trxModule, int docFeatureId, string transactionType, Guid transactionId);
    }

    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly CompanyContext _context;
        private readonly FlogContext _flogContext;
        private IConfiguration _config;
        private LogWriter _logger;

        public DocumentGenerator(CompanyContext context)
        {
            _context = context;
        }

        public DocumentGenerator(CompanyContext context, FlogContext flogContext)
        {
            _context = context;
            _flogContext = flogContext;
        }

        public DocumentGenerator(CompanyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

            _logger = new LogWriter(_config);
        }

        private const int SEGMENT_TYPE_CHAR = 1;
        private const int SEGMENT_TYPE_NUMBER = 2;
        private const int SEGMENT_TYPE_SYMBOL = 3;
        private const int SEGMENT_TYPE_YEAR = 4;
        private const int SEGMENT_TYPE_MONTH = 5;

        /// <summary>
        /// Generate Unique Doc No for Checkbook
        /// </summary>
        /// <param name="docDate"></param>
        /// <param name="moduleId"></param>
        /// <param name="docNumberSetup"></param>
        /// <param name="checkbookCode"></param>
        /// <param name="documentType"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public string UniqueDocumentNoByCheckbook(DateTime docDate, string checkbookCode, int docFeatureId, DbTransaction trans = null)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(checkbookCode) && docFeatureId > 0)
            {
                try
                {
                    Console.WriteLine("[UniqueDocumentNoByCheckbook] ******  checkbookCode -> " + checkbookCode + " | docFeatureId " + docFeatureId);

                    var checkbook = _context.Checkbooks.Where(x => x.CheckbookCode.Equals(checkbookCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if(checkbook != null)
                    {
                        string docNoSetup = string.Empty;
                        if(docFeatureId == DOCNO_FEATURE.CHECKBOOK_IN)
                        {
                            docNoSetup = checkbook.CheckbookInDocNo;
                        }
                        else if (docFeatureId == DOCNO_FEATURE.CHECKBOOK_OUT)
                        {
                            docNoSetup = checkbook.CheckbookOutDocNo;
                        }else if (docFeatureId == DOCNO_FEATURE.CHECKBOOK_PAYMENT)
                        {
                            docNoSetup = checkbook.PaymentDocNo;
                        }else if (docFeatureId == DOCNO_FEATURE.CHECKBOOK_RECEIPT)
                        {
                            docNoSetup = checkbook.ReceiptDocNo;
                        }else if (docFeatureId == DOCNO_FEATURE.CHECKBOOK_RECONCILE)
                        {
                            docNoSetup = checkbook.ReconcileDocNo;
                        }

                        if (!string.IsNullOrEmpty(docNoSetup))
                        {
                            Console.WriteLine("[UniqueDocumentNoByCheckbook] ****** DOC NO -> " + docNoSetup);

                            //DOC FORMAT
                            Entities.NumberFormatHeader formatHeader = _context.NumberFormatHeaders.Where(x => x.DocumentId.Equals(docNoSetup, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                            if (formatHeader != null)
                            {
                                var formatDetails = _context.NumberFormatDetails.Where(x => x.FormatHeaderId == formatHeader.FormatHeaderId).OrderBy(z => z.SegmentNo).AsQueryable();

                                if (formatDetails.Any())
                                {
                                    Guid numberFormatLastNoEditId = Guid.Empty;
                                    bool isMonthlyReset = formatHeader.IsMonthlyReset;
                                    bool isYearlyReset = formatHeader.IsYearlyReset;

                                    var segmentNumber = formatDetails.Where(x => x.SegmentType == SEGMENT_TYPE_NUMBER).FirstOrDefault();

                                    int lastIndex = 0;
                                    if (segmentNumber != null)
                                    {
                                        int autoNumberLength = segmentNumber.SegmentLength;

                                        int autoNumberPos = 0;
                                        int segmentBeforeNumber = formatDetails.Where(x => x.SegmentNo < segmentNumber.SegmentNo).Sum(s => s.SegmentLength);
                                        if (segmentBeforeNumber > 0)
                                            autoNumberPos = segmentBeforeNumber;

                                        //OBTAIN LAST EXISTING NO
                                        var lastNoQuery = _context.NumberFormatLastNos.Where(x => x.DocumentId.Equals(formatHeader.DocumentId)).OrderByDescending(o => o.PeriodYear).OrderByDescending(o => o.PeriodMonth).AsQueryable();

                                        if (lastNoQuery.Any())
                                        {
                                            if (formatHeader.IsYearlyReset)
                                            {
                                                if (formatHeader.IsMonthlyReset)
                                                {
                                                    var lastNo = lastNoQuery.Where(x => x.PeriodYear == docDate.Year && x.PeriodMonth == docDate.Month).FirstOrDefault();
                                                    if (lastNo != null)
                                                    {
                                                        lastIndex = lastNo.LastIndex;
                                                        numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                    }
                                                }
                                                else
                                                {
                                                    var lastNo = lastNoQuery.Where(x => x.PeriodYear == docDate.Year).OrderByDescending(o => o.LastIndex).FirstOrDefault();
                                                    if (lastNo != null)
                                                    {
                                                        lastIndex = lastNo.LastIndex;
                                                        numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (formatHeader.IsMonthlyReset)
                                                {
                                                    var lastNo = lastNoQuery.Where(x => x.PeriodYear == docDate.Year && x.PeriodMonth == docDate.Month).FirstOrDefault();
                                                    if (lastNo != null)
                                                    {
                                                        lastIndex = lastNo.LastIndex;
                                                        numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                    }
                                                }
                                                else
                                                {
                                                    var lastNo = lastNoQuery.OrderByDescending(o => o.PeriodYear).OrderByDescending(o => o.PeriodMonth).FirstOrDefault();
                                                    if (lastNo != null)
                                                    {
                                                        lastIndex = lastNo.LastIndex;
                                                        numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                    }
                                                }
                                            }
                                        }

                                        //Increase new number
                                        lastIndex = lastIndex + 1;

                                        //CREATE NEW NUMBER
                                        sb.Clear();
                                        foreach (Entities.NumberFormatDetail rowFormat in formatDetails)
                                        {
                                            if (rowFormat.SegmentType.Equals(SEGMENT_TYPE_MONTH))
                                            {
                                                sb.Append(docDate.ToString(rowFormat.MaskFormat));
                                            }
                                            else if (rowFormat.SegmentType.Equals(SEGMENT_TYPE_YEAR))
                                            {
                                                sb.Append(docDate.ToString(rowFormat.MaskFormat.ToLower()));
                                            }
                                            else if (rowFormat.SegmentType.Equals(SEGMENT_TYPE_NUMBER))
                                            {
                                                sb.Append("0".Repeat(autoNumberLength - lastIndex.ToString().Length));
                                                sb.Append(lastIndex.ToString());
                                            }
                                            else
                                            {
                                                sb.Append(rowFormat.MaskFormat.ToString());
                                            }
                                        }
                                    }

                                    if (sb.Length > 0)
                                    {
                                        if (numberFormatLastNoEditId != Guid.Empty)
                                        {
                                            //UPDATE
                                            Entities.NumberFormatLastNo edit = _context.NumberFormatLastNos.Where(x => x.NumberFormatLastNoId == numberFormatLastNoEditId).FirstOrDefault();

                                            if (edit != null)
                                            {
                                                edit.LastNo = sb.ToString();
                                                edit.LastIndex = lastIndex;
                                            }

                                            _context.NumberFormatLastNos.Update(edit);
                                        }
                                        else
                                        {
                                            //INSERT
                                            Entities.NumberFormatLastNo newLastNo = new Entities.NumberFormatLastNo()
                                            {
                                                NumberFormatLastNoId = Guid.NewGuid(),
                                                DocumentId = formatHeader.DocumentId,
                                                PeriodYear = docDate.Year,
                                                PeriodMonth = docDate.Month,
                                                LastNo = sb.ToString(),
                                                LastIndex = lastIndex,
                                                ModifiedDate = DateTime.Now
                                            };

                                            _context.NumberFormatLastNos.Add(newLastNo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    sb.Clear();

                    Console.WriteLine("[UniqueDocumentNoByCheckbook] " + ex.Message);
                    Console.WriteLine("[UniqueDocumentNoByCheckbook] " + ex.StackTrace);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate Unique Document No By Transaction Type
        /// </summary>
        /// <param name="docDate"></param>
        /// <param name="trxModule"></param>
        /// <param name="docFeatureId"></param>
        /// <param name="transactionType"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public string UniqueDocumentNoByTrxType(DateTime docDate, int trxModule, int docFeatureId, string transactionType, DbTransaction trans = null)
        {
            StringBuilder sb = new StringBuilder();

            if(trxModule > 0 && docFeatureId > 0)
            {
                try
                {
                    Console.WriteLine("[UniqueDocumentNoByTrxType] ******  A -> " + trxModule + " | " + docFeatureId + " | " + transactionType);

                    //GET DOCUMENT NO FROM FN DOC SETUP
                    var docNoSetup = _context.FNDocNumberSetups.Where(x => x.TrxModule==trxModule && x.DocFeatureId == docFeatureId && x.TransactionType.Equals(transactionType, StringComparison.OrdinalIgnoreCase)).Select(o=>o.DocNo).FirstOrDefault();

                    if (docNoSetup != null && !string.IsNullOrEmpty(docNoSetup))
                    {
                        Console.WriteLine("[UniqueDocumentNoByTrxType] ****** DOC NO -> " + docNoSetup);

                        //DOC FORMAT
                        Entities.NumberFormatHeader formatHeader = _context.NumberFormatHeaders.Where(x => x.DocumentId.Equals(docNoSetup, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                        if(formatHeader != null)
                        {
                            var formatDetails = _context.NumberFormatDetails.Where(x => x.FormatHeaderId == formatHeader.FormatHeaderId).OrderBy(z=>z.SegmentNo).AsQueryable();

                            if(formatDetails.Any())
                            {
                                Guid numberFormatLastNoEditId = Guid.Empty;
                                bool isMonthlyReset = formatHeader.IsMonthlyReset;
                                bool isYearlyReset = formatHeader.IsYearlyReset;

                                var segmentNumber = formatDetails.Where(x => x.SegmentType == SEGMENT_TYPE_NUMBER).FirstOrDefault();

                                int lastIndex = 0;
                                if(segmentNumber != null)
                                {
                                    int autoNumberLength = segmentNumber.SegmentLength;

                                    int autoNumberPos = 0;
                                    int segmentBeforeNumber = formatDetails.Where(x => x.SegmentNo < segmentNumber.SegmentNo).Sum(s => s.SegmentLength);
                                    if (segmentBeforeNumber > 0)
                                        autoNumberPos = segmentBeforeNumber;

                                    Console.WriteLine("[UniqueDocumentNo] ****** TRX TYPE ***" + transactionType + "***");

                                    //OBTAIN LAST EXISTING NO
                                    var lastNoQuery = _context.NumberFormatLastNos.Where(x => x.DocumentId.Equals(formatHeader.DocumentId)).OrderByDescending(o => o.PeriodYear).OrderByDescending(o => o.PeriodMonth).AsQueryable();

                                    if (lastNoQuery.Any())
                                    {
                                        if (formatHeader.IsYearlyReset)
                                        {
                                            if (formatHeader.IsMonthlyReset)
                                            {
                                                var lastNo = lastNoQuery.Where(x => x.PeriodYear == docDate.Year && x.PeriodMonth == docDate.Month).FirstOrDefault();
                                                if(lastNo != null)
                                                {
                                                    lastIndex = lastNo.LastIndex;
                                                    numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                }
                                            }
                                            else
                                            {
                                                var lastNo = lastNoQuery.Where(x => x.PeriodYear == docDate.Year).OrderByDescending(o=>o.LastIndex).FirstOrDefault();
                                                if (lastNo != null)
                                                {
                                                    lastIndex = lastNo.LastIndex;
                                                    numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (formatHeader.IsMonthlyReset)
                                            {
                                                var lastNo = lastNoQuery.Where(x => x.PeriodYear == docDate.Year && x.PeriodMonth == docDate.Month).FirstOrDefault();
                                                if (lastNo != null)
                                                {
                                                    lastIndex = lastNo.LastIndex;
                                                    numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                }
                                            }
                                            else
                                            {
                                                var lastNo = lastNoQuery.OrderByDescending(o => o.PeriodYear).OrderByDescending(o=>o.PeriodMonth).FirstOrDefault();
                                                if (lastNo != null)
                                                {
                                                    lastIndex = lastNo.LastIndex;
                                                    numberFormatLastNoEditId = lastNo.NumberFormatLastNoId;
                                                }
                                            }
                                        }
                                    }

                                    //Increase new number
                                    lastIndex = lastIndex + 1;

                                    //CREATE NEW NUMBER
                                    sb.Clear();
                                    foreach (Entities.NumberFormatDetail rowFormat in formatDetails)
                                    {
                                        if (rowFormat.SegmentType.Equals(SEGMENT_TYPE_MONTH))
                                        {
                                            sb.Append(docDate.ToString(rowFormat.MaskFormat));
                                        }
                                        else if (rowFormat.SegmentType.Equals(SEGMENT_TYPE_YEAR))
                                        {
                                            sb.Append(docDate.ToString(rowFormat.MaskFormat.ToLower()));
                                        }
                                        else if (rowFormat.SegmentType.Equals(SEGMENT_TYPE_NUMBER))
                                        {
                                            sb.Append("0".Repeat(autoNumberLength - lastIndex.ToString().Length));
                                            sb.Append(lastIndex.ToString());
                                        }
                                        else
                                        {
                                            sb.Append(rowFormat.MaskFormat.ToString());
                                        }
                                    }
                                }

                                if(sb.Length > 0)
                                {
                                    if (numberFormatLastNoEditId != Guid.Empty)
                                    {
                                        //UPDATE
                                        Entities.NumberFormatLastNo edit = _context.NumberFormatLastNos.Where(x => x.NumberFormatLastNoId == numberFormatLastNoEditId).FirstOrDefault();

                                        if (edit != null)
                                        {
                                            edit.LastNo = sb.ToString();
                                            edit.LastIndex = lastIndex;
                                        }

                                        _context.NumberFormatLastNos.Update(edit);
                                    }
                                    else
                                    {
                                        //INSERT
                                        Entities.NumberFormatLastNo newLastNo = new Entities.NumberFormatLastNo()
                                        {
                                            NumberFormatLastNoId = Guid.NewGuid(),
                                            DocumentId = formatHeader.DocumentId,
                                            PeriodYear = docDate.Year,
                                            PeriodMonth = docDate.Month,
                                            LastNo = sb.ToString(),
                                            LastIndex = lastIndex,
                                            ModifiedDate = DateTime.Now
                                        };

                                        _context.NumberFormatLastNos.Add(newLastNo);
                                    }
                                }                                
                            }
                        }
                        
                    }
                }catch(Exception ex)
                {
                    sb.Clear();

                    Console.WriteLine("[UniqueDocumentNoByTrxType] " + ex.Message);
                    Console.WriteLine("[UniqueDocumentNoByTrxType] " + ex.StackTrace);                    
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Update last document no (if any)
        /// </summary>
        /// <param name="documentNo"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DocNoDelete( string documentNo, DbTransaction trans = null)
        {
            bool valid = true;

            if(!string.IsNullOrEmpty(documentNo))
            {
                try
                {
                    var lastNo = _context.NumberFormatLastNos.AsNoTracking().Where(x => x.LastNo.Equals(documentNo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (lastNo != null)
                    {
                        if (lastNo.LastIndex > 1)
                        {
                            var formatDetails = (from d in _context.NumberFormatDetails
                                                 join h in _context.NumberFormatHeaders on d.FormatHeaderId equals h.FormatHeaderId
                                                 where h.DocumentId == lastNo.DocumentId
                                                 select d).ToList();

                            var segmentNumber = formatDetails.Where(x => x.SegmentType == SEGMENT_TYPE_NUMBER).FirstOrDefault();

                            int lastIndex = 0;
                            if (segmentNumber != null)
                            {
                                int autoNumberLength = segmentNumber.SegmentLength;

                                int autoNumberPos = 0;
                                int segmentBeforeNumber = formatDetails.Where(x => x.SegmentNo < segmentNumber.SegmentNo).Sum(s => s.SegmentLength);
                                if (segmentBeforeNumber > 0)
                                    autoNumberPos = segmentBeforeNumber;

                                //Console.WriteLine("[DoDelete] ****** autoNumberLength ***" + autoNumberLength + "***");
                                //Console.WriteLine("[DoDelete] ****** autoNumberPos ***" + autoNumberPos + "***");

                                //Decrease new number
                                lastIndex = lastNo.LastIndex - 1;

                                //CREATE NEW NUMBER
                                if (lastIndex > 0)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("0".Repeat(autoNumberLength - lastIndex.ToString().Length));
                                    sb.Append(lastIndex.ToString());

                                    string newGeneratedNo = lastNo.LastNo.ReplaceString(autoNumberPos, autoNumberLength, sb.ToString());

                                    //Console.WriteLine("[DoDelete] ****** newGeneratedNo ***" + newGeneratedNo + "***");

                                    lastNo.LastIndex = lastIndex;
                                    lastNo.LastNo = newGeneratedNo;
                                }
                                else
                                {
                                    lastNo.LastIndex = 0;
                                    lastNo.LastNo = "";
                                }
                            }
                        }
                        else
                        {
                            lastNo.LastIndex = 0;
                            lastNo.LastNo = "";
                        }

                        _context.NumberFormatLastNos.Update(lastNo);
                    }
                    else
                    {
                        Console.WriteLine("***** Document last no not found ! ********");
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine("[DocNoDelete] *************** " + ex.Message);
                    Console.WriteLine("[DocNoDelete] *************** " + ex.StackTrace);
                    valid = false;
                }
            }

            return valid;
        }

        /// <summary>
        /// Get Current Document Approvers
        /// </summary>
        /// <param name="trxModule"></param>
        /// <param name="docFeatureId"></param>
        /// <param name="transactionType"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public List<TrxPersonApprover> GetCurrentDocumentApprovers(int trxModule, int docFeatureId, string transactionType, Guid transactionId)
        {
            List<TrxPersonApprover> currentApprovers = new List<TrxPersonApprover>();

            if (_flogContext != null)
            {
                try
                {
                    List<FlogEntities.Person> personsAll = _flogContext.Persons.ToList();

                    //OBTAIN FIRST NEW APPROVER LEVEL
                    var initApprover = _context.TrxDocumentApprovals.Where(x => x.TrxModule == trxModule
                                && x.TransactionType == transactionType && x.DocFeatureId == docFeatureId
                                && x.Status == DOCSTATUS.NEW && x.TransactionId == transactionId).OrderBy(o => o.Index).FirstOrDefault();

                    if (initApprover != null)
                    {
                        //GET ALL SAME APPROVER LEVEL
                        var myApprovers = _context.TrxDocumentApprovals.Where(x => x.TrxModule == initApprover.TrxModule
                                && x.TransactionType == initApprover.TransactionType && x.DocFeatureId == initApprover.DocFeatureId
                                && x.Status == initApprover.Status && x.TransactionId == initApprover.TransactionId 
                                && x.Index == initApprover.Index).AsQueryable();
                        
                        foreach (var appr in myApprovers)
                        {
                            if ((Guid)appr.PersonCategoryId != Guid.Empty)
                            {
                                var groupApprovers = personsAll.Where(p => !string.IsNullOrEmpty(p.PersonCategoryId) && p.PersonCategoryId.Equals(appr.PersonCategoryId.ToString(),StringComparison.OrdinalIgnoreCase)).ToList();

                                foreach (var member in groupApprovers)
                                {
                                    Guid memberPersonId = Guid.Parse(member.PersonId);
                                    if (!currentApprovers.Where(x => x.PersonId == memberPersonId).Any())
                                    {
                                        currentApprovers.Add(new TrxPersonApprover { PersonId = memberPersonId, ApprovalIndex = appr.Index, PersonFullName = member.PersonFullName });
                                    }
                                }
                            }
                            else
                            {
                                var thisApprover = personsAll.Where(p => p.PersonId.Equals(appr.PersonId.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                                currentApprovers.Add(new TrxPersonApprover { PersonId = (Guid)appr.PersonId, ApprovalIndex = appr.Index, PersonFullName = thisApprover .PersonFullName});
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("[GetCurrentDocumentApprovers] **************** " + ex.Message);
                    Console.WriteLine("[GetCurrentDocumentApprovers] **************** " + ex.StackTrace);
                }
            }

            return currentApprovers;
        }

        public List<TrxDocumentApprovalComment> GetDocumentApprovalComments(int trxModule, int docFeatureId, string transactionType, Guid transactionId)
        {
            List<TrxDocumentApprovalComment> comments = new List<TrxDocumentApprovalComment>();

            if(_flogContext != null)
            {
                var persons = _flogContext.Persons.ToList();

                comments = (from c in _context.TrxDocumentApprovalComments
                            join h in _context.TrxDocumentApprovals on c.TrxDocumentApprovalId equals h.TrxDocumentApprovalId
                            where h.TrxModule == trxModule && h.DocFeatureId == docFeatureId && h.TransactionType == transactionType && h.TransactionId == transactionId
                            orderby c.CommentDate descending
                            select new Entities.TrxDocumentApprovalComment
                            {
                                TrxDocumentApprovalCommentId= c.TrxDocumentApprovalCommentId,
                                TrxDocumentApprovalId = c.TrxDocumentApprovalId,
                                Status = c.Status,
                                StatusCaption = DOCSTATUS.Caption(c.Status,""),
                                PersonId = c.PersonId,
                                UserFullName = persons.Where(p=>p.PersonId.Equals(c.PersonId.ToString(), StringComparison.OrdinalIgnoreCase)).Select(o=>o.PersonFullName).FirstOrDefault(),
                                CommentDate = c.CommentDate,
                                Comments = c.Comments,                                
                            }).ToList();
            }

            return comments;
        }
    }

    public class TrxPersonApprover
    {
        public Guid PersonId { get; set; }
        public string PersonFullName { get; set; }
        public int ApprovalIndex { get; set; }
    }

}

