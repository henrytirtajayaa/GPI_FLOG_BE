using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.BankReconcile.PutStatusBankReconcile
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _docGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.BankReconcileHeaders.FirstOrDefaultAsync(x => x.BankReconcileId == request.Body.BankReconcileId);
                    
                    string docNo = "";
                    if(entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        if (request.Body.ActionDocStatus == DOCSTATUS.DELETE)
                        {
                            if(entryHeader.Status == DOCSTATUS.NEW)
                            {
                                entryHeader.Status = DOCSTATUS.DELETE;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.BankReconcileHeaders.Update(entryHeader);

                                await _context.SaveChangesAsync();
                                
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be deleted.");
                            }
                        }else if(request.Body.ActionDocStatus == DOCSTATUS.POST)
                        {
                            if (entryHeader.Status == DOCSTATUS.NEW )
                            {
                                if(entryHeader.BankEndingOrgBalance != entryHeader.CheckbookEndingOrgBalance)
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Bank Ending balance not match againts Checkbook  Ending Balance ! Please save transaction to ensure data is updated.");
                                }

                                //CREATE CHECKBOOK && DISTRIBUTION JOURNAL
                                var adjustments = _context.BankReconcileAdjustments.Where(x=>x.BankReconcileId == entryHeader.BankReconcileId).ToList();
                                
                                var checkbooks = await ConvertAdjustments(adjustments, entryHeader);

                                //DO SOME POSTING HERE
                                JournalResponse resp = new JournalResponse();
                                resp.Valid = true;

                                foreach(var cb in checkbooks)
                                {
                                    resp = await _financeManager.PostJournalAsync(cb.DocumentNo, TRX_MODULE.TRX_CHECKBOOK, request.Initiator.UserId, cb.TransactionDate);

                                    if (!resp.Valid)
                                    {
                                        break;
                                    }
                                }
                                
                                if (resp.Valid)
                                {
                                    entryHeader.Status = DOCSTATUS.POST;
                                    entryHeader.ModifiedBy = request.Initiator.UserId;
                                    entryHeader.ModifiedDate = DateTime.Now;

                                    _context.BankReconcileHeaders.Update(entryHeader);

                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Post Journals can not be created.");
                                }                                
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be posted !");
                            }
                        }
                        else if(request.Body.ActionDocStatus == DOCSTATUS.VOID)
                        {
                            if (entryHeader.Status == DOCSTATUS.POST)
                            {
                                //CHECK NO NEW RECONCILE RELATED
                                var existNewReconcile = await (_context.BankReconcileHeaders.Where(x => x.PrevBankReconcileId == entryHeader.BankReconcileId && (x.Status == DOCSTATUS.NEW || x.Status == DOCSTATUS.POST)).AnyAsync());

                                if (existNewReconcile)
                                {
                                    transaction.Rollback();
                                    return ApiResult<Response>.ValidationError("Void is not Allowed ! Only last Reconciliation can be Void.");
                                }
                                
                                //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                //VOID ALL ADJUSTMENTS JOURNAL
                                var checkbooks = (from bd in _context.BankReconcileAdjustments
                                                  join cb in _context.CheckbookTransactionHeaders on bd.CheckbookTransactionId equals cb.CheckbookTransactionId
                                                  where bd.BankReconcileId == entryHeader.BankReconcileId
                                                  select new { CheckbookTransactionId = bd.CheckbookTransactionId}).Distinct().ToList();

                                var resp = new JournalResponse();
                                resp.Valid = true;
                                resp.ErrorMessage = "";
                                if (checkbooks.Count > 0)
                                {
                                    foreach(var cbi in checkbooks)
                                    {
                                        var cb = _context.CheckbookTransactionHeaders.Where(c => c.CheckbookTransactionId == cbi.CheckbookTransactionId).FirstOrDefault();

                                        if(cb != null)
                                        {
                                            resp = await _financeManager.VoidJournalAsync(cb.DocumentNo, TRX_MODULE.TRX_CHECKBOOK, request.Initiator.UserId, request.Body.ActionDate);

                                            if (resp.Valid)
                                            {
                                                cb.VoidBy = request.Initiator.UserId;
                                                cb.VoidDate = request.Body.ActionDate;
                                                cb.Status = DOCSTATUS.VOID;

                                                _context.CheckbookTransactionHeaders.Update(cb);

                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = "Checkbook not exist";
                                            break;
                                        }                                                                               
                                    }
                                }                                

                                if (resp.Valid)
                                {
                                    int updated = _context.Database.ExecuteSqlCommand(string.Format("UPDATE bank_reconcile_adjustment SET status = {0} WHERE bank_reconcile_id = '{1}' ", DOCSTATUS.VOID, entryHeader.BankReconcileId));

                                    entryHeader.Status = DOCSTATUS.VOID;
                                    entryHeader.VoidBy = request.Initiator.UserId;
                                    entryHeader.VoidDate = DateTime.Now;
                                    entryHeader.StatusComment = request.Body.Comments;

                                    _context.BankReconcileHeaders.Update(entryHeader);

                                    await _context.SaveChangesAsync();
                                    
                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Post Journals can not be created.");
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be void.");
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Document not available.");
                    }

                    var response = new Response()
                    {
                        BankReconcileId = request.Body.BankReconcileId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus)) 
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("[PutStatusBankReconcile] ******** ERROR ********* " + ex.Message);
                    Console.WriteLine("[PutStatusBankReconcile] ******** TRACE ********* " + ex.StackTrace);
                    return ApiResult<Response>.InternalServerError("[PutStatusBankReconcile] " + ex.Message);
                }
            }
        }

        private async Task<List<Entities.CheckbookTransactionHeader>> ConvertAdjustments(
            List<Entities.BankReconcileAdjustment> adjustments, Entities.BankReconcileHeader header)
        {
            List<Entities.CheckbookTransactionHeader> checkbooks = new List<Entities.CheckbookTransactionHeader>();

            if (adjustments != null)
            {
                //GROUP BY Exchange Rate, Multiply, Transaction Date, IN/OUT
                var groupAdjustments = (from adj in adjustments
                                        group adj by new
                                        {
                                            adj.DocumentType,
                                            adj.TransactionDate,
                                            adj.ExchangeRate,
                                            adj.IsMultiply
                                        } into grp
                                        select new
                                        {
                                            DocumentType = grp.Key.DocumentType,
                                            TransactionDate = grp.Key.TransactionDate,
                                            ExchangeRate = grp.Key.ExchangeRate,
                                            IsMultiply = grp.Key.IsMultiply
                                        }).ToList();

                foreach (var group in groupAdjustments)
                {
                    var items = adjustments.Where(g => g.DocumentType == group.DocumentType && g.TransactionDate.Date == group.TransactionDate.Date && g.ExchangeRate == group.ExchangeRate && g.IsMultiply == group.IsMultiply).ToList();

                    //CREATE CHECKBOOK
                    var cb = await this.CreateCheckbook( header, items);

                    if (cb != null && !string.IsNullOrEmpty(cb.DocumentNo))
                    {
                        checkbooks.Add(cb);
                    }
                }
            }

            return checkbooks;
        }

        private async Task<Entities.CheckbookTransactionHeader> CreateCheckbook(Entities.BankReconcileHeader header, List<Entities.BankReconcileAdjustment> adjustments)
        {
            Entities.CheckbookTransactionHeader checkbook = null;

            if (adjustments != null && adjustments.Count > 0)
            {
                Entities.BankReconcileAdjustment adjustment = adjustments.FirstOrDefault();
                
                string documentUniqueNo = "";
                if (adjustment.DocumentType.Trim().ToUpper().Contains("IN"))
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(adjustment.TransactionDate, header.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_IN, _context.Database.CurrentTransaction.GetDbTransaction());
                }
                else
                {
                    documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(adjustment.TransactionDate, header.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_OUT, _context.Database.CurrentTransaction.GetDbTransaction());
                }

                decimal totalOriginating = adjustments.Sum(x => x.OriginatingAmount);

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    checkbook = new Entities.CheckbookTransactionHeader()
                    {
                        CheckbookTransactionId = Guid.NewGuid(),
                        DocumentType = adjustment.DocumentType,
                        DocumentNo = documentUniqueNo,
                        TransactionDate = adjustment.TransactionDate,
                        TransactionType = "NORMAL",
                        CurrencyCode = adjustment.CurrencyCode,
                        ExchangeRate = adjustment.ExchangeRate,
                        IsMultiply = adjustment.IsMultiply,
                        CheckbookCode = header.CheckbookCode,
                        IsVoid = false,
                        VoidDocumentNo = "",
                        PaidSubject = adjustment.PaidSubject,
                        SubjectCode = "",
                        Description = string.Format("ADJUSTMENT RECONCILE BANK STATEMENT {0}", documentUniqueNo),
                        OriginatingTotalAmount = totalOriginating,
                        FunctionalTotalAmount = CALC.FunctionalAmount(adjustment.IsMultiply, totalOriginating, adjustment.ExchangeRate),
                        CreatedBy = header.CreatedBy,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.POST
                    };

                    _context.CheckbookTransactionHeaders.Add(checkbook);

                    //INSERT DETAILS
                    List<Entities.CheckbookTransactionDetail> details = new List<Entities.CheckbookTransactionDetail>();

                    foreach (var adjDetail in adjustments)
                    {
                        var checkBookDetail = new Entities.CheckbookTransactionDetail()
                        {
                            TransactionDetailId = Guid.NewGuid(),
                            CheckbookTransactionId = checkbook.CheckbookTransactionId,
                            ChargesId = adjDetail.ChargesId,
                            ChargesDescription = adjDetail.Description,
                            OriginatingAmount = adjDetail.OriginatingAmount,
                            FunctionalAmount = CALC.FunctionalAmount(adjDetail.IsMultiply, adjDetail.OriginatingAmount, adjDetail.ExchangeRate),
                            Status = DOCSTATUS.NEW
                        };

                        details.Add(checkBookDetail);

                        //UPDATE BANK RECONCILIATION ADJUSTMENT
                        adjDetail.CheckbookTransactionId = checkBookDetail.CheckbookTransactionId;
                        adjDetail.TransactionDetailId = checkBookDetail.TransactionDetailId;

                        _context.BankReconcileAdjustments.Update(adjDetail);
                    }

                    await _context.CheckbookTransactionDetails.AddRangeAsync(details);

                    //CREATE DISTRIBUTION JOURNAL HERE
                    var resp = await _financeManager.CreateDistributionJournalAsync(checkbook, details);
                    
                    if (resp.Valid)
                    {
                        await _context.SaveChangesAsync();

                        //PASS CHECKBOOK DETAILS FOR ADJUSTMENT DETAILS
                        checkbook.CheckbookDetails = details;
                    }
                    else
                    {
                        checkbook = null;
                    }
                }
            }

            return checkbook;
        }

    }
}
