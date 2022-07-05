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
using Infrastructure;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Finance.DepositSettlement.PutStatusDepositSettlement
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
                    var SettlementHeader = await _context.DepositSettlementHeaders.FirstOrDefaultAsync(x => x.SettlementHeaderId == request.Body.SettlementHeaderId);

                    string docNo = "";

                    if (SettlementHeader != null)
                    {
                        docNo = SettlementHeader.DocumentNo;

                        if (request.Body.Status == DOCSTATUS.DELETE)
                        {
                            if (SettlementHeader.Status == DOCSTATUS.NEW)
                            {
                                SettlementHeader.Status = DOCSTATUS.DELETE;
                                SettlementHeader.ModifiedBy = request.Initiator.UserId;
                                SettlementHeader.ModifiedDate = DateTime.Now;

                                _context.DepositSettlementHeaders.Update(SettlementHeader);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                return ApiResult<Response>.ValidationError("Record can not be deleted!");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.POST)
                        {
                            if (SettlementHeader.Status == DOCSTATUS.NEW)
                            {
                                SettlementHeader.Status = DOCSTATUS.POST;

                                var resp = await this.DoPosting(SettlementHeader, request);

                                if (resp.Valid)
                                {
                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return ApiResult<Response>.ValidationError(resp.ErrorMessage);
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be posted!");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.SUBMIT)
                        {
                            if (SettlementHeader.Status == DOCSTATUS.NEW)
                            {
                                SettlementHeader.Status = DOCSTATUS.SUBMIT;

                                var resp = await this.DoPosting(SettlementHeader, request);

                                if (resp.Valid)
                                {
                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return ApiResult<Response>.ValidationError(resp.ErrorMessage);
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be posted!");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.VOID)
                        {
                            if (SettlementHeader.Status == DOCSTATUS.POST)
                            {
                                SettlementHeader.Status = DOCSTATUS.VOID;
                                SettlementHeader.VoidBy = request.Initiator.UserId;
                                SettlementHeader.VoidDate = DateTime.Now;
                                SettlementHeader.StatusComment = request.Body.StatusComment;

                                _context.DepositSettlementHeaders.Update(SettlementHeader);

                                //VOID APPLY IF ANY
                                var allocHeader = (from rc in _context.DepositSettlementDetails
                                                   join al in _context.ARApplyDetails on rc.ReceivableApplyDetailId equals al.ReceivableApplyDetailId
                                                   join hd in _context.ARApplyHeaders on al.ReceivableApplyId equals hd.ReceivableApplyId
                                                   where rc.SettlementHeaderId == SettlementHeader.SettlementHeaderId && rc.Status != DOCSTATUS.VOID && hd.Status == DOCSTATUS.POST
                                                   select hd).FirstOrDefault();

                                JournalResponse voidApply = new JournalResponse { Valid = true, ErrorMessage = "" };

                                if (allocHeader != null)
                                {
                                    voidApply = await _financeManager.VoidJournalAsync(allocHeader.DocumentNo, TRX_MODULE.TRX_APPLY_RECEIPT, request.Initiator.UserId, request.Body.ActionDate);
                                }

                                if (voidApply.Valid)
                                {
                                    allocHeader.Status = DOCSTATUS.VOID;
                                    allocHeader.StatusComment = request.Body.StatusComment;
                                    allocHeader.VoidBy = request.Initiator.UserId;
                                    allocHeader.VoidDate = DateTime.Now;

                                    _context.ARApplyHeaders.Update(allocHeader);

                                    //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                    var resp = await _financeManager.VoidJournalAsync(SettlementHeader.DocumentNo, TRX_MODULE.TRX_RECEIPT, request.Initiator.UserId, request.Body.ActionDate);

                                    if (resp.Valid)
                                    {
                                        await _context.SaveChangesAsync();

                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return ApiResult<Response>.ValidationError(resp.ErrorMessage);
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return ApiResult<Response>.ValidationError(voidApply.ErrorMessage);
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be void!");
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
                        SettlementHeaderId = request.Body.SettlementHeaderId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("[PutStatusDepositSettlement] *********** " + ex.StackTrace);
                    return ApiResult<Response>.InternalServerError("[PutStatusDepositSettlement] " + ex.Message);
                }
            }
        }

        private async Task<JournalResponse> DoPosting(Entities.DepositSettlementHeader SettlementHeader, Request request)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "" };

            //CREATE AUTO POSTING APPLY FOR RECEIVABLE INVOICE
            var apply = await ApplyDepositSettlement(SettlementHeader, request.Initiator.UserId);

            if (apply.Valid)
            {
                SettlementHeader.ApplyDocumentNo = apply.ValidMessage;

                //CREATE AUTO POSTING CHECKBOOK OUT\
                var cbDetails = _context.DepositSettlementDetails.Where(x => x.SettlementHeaderId == SettlementHeader.SettlementHeaderId).ToList();
                var checkbook = await CreateCheckbook(SettlementHeader, cbDetails, request.Initiator.UserId);

                if (checkbook != null)
                {
                    SettlementHeader.CheckbookDocumentNo = checkbook.DocumentNo;
                    Console.WriteLine("==================== **************** CHECKBOOK DOC NO : " + checkbook.DocumentNo);
                }

                //SettlementHeader.Status = DOCSTATUS.POST;
                SettlementHeader.ModifiedBy = request.Initiator.UserId;
                SettlementHeader.ModifiedDate = DateTime.Now;

                _context.DepositSettlementHeaders.Update(SettlementHeader);

            }

            return resp;
        }
        private async Task<JournalResponse> ApplyDepositSettlement(Entities.DepositSettlementHeader header, string createdBy)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (header != null)
            {
                var details = _context.DepositSettlementDetails.Where(x => x.SettlementHeaderId == header.SettlementHeaderId).ToList();

                if (details.Count() > 0)
                {
                    //CREATE CHECKBOOK
                    var arApply = await this.CreateReceivableApply(header, details, createdBy);

                    if (arApply == null)
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Receivable Apply can not be created !";
                        return resp;
                    }
                    else
                    {
                        resp.ValidMessage = arApply.DocumentNo;
                    }
                }
            }
            return resp;
        }

        private async Task<Entities.ARApplyHeader> CreateReceivableApply(Entities.DepositSettlementHeader receiptHeader, List<Entities.DepositSettlementDetail> receiptDetails, string createdBy)
        {
            Entities.ARApplyHeader applyHeader = null;

            if (receiptDetails != null && receiptDetails.Count > 0)
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(receiptHeader.TransactionDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.NOTRX_RECEIVABLE_APPLY, "", _context.Database.CurrentTransaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    decimal totalApplied = receiptDetails.Sum(s => s.OriginatingPaid);

                    applyHeader = new Entities.ARApplyHeader()
                    {
                        ReceivableApplyId = Guid.NewGuid(),
                        TransactionDate = receiptHeader.TransactionDate,
                        DocumentType = DOCUMENTTYPE.DEPOSIT_SETTLEMENT,
                        DocumentNo = documentUniqueNo,
                        CheckbookTransactionId = Guid.Empty,
                        ReceiveTransactionId = receiptHeader.ReceiveTransactionId,
                        ReceiptHeaderId = Guid.Empty,
                        //SettlementHeaderId = Guid.Empty,
                        CustomerId = receiptHeader.CustomerId,
                        Description = receiptHeader.Description,
                        OriginatingTotalPaid = totalApplied,
                        FunctionalTotalPaid = CALC.FunctionalAmount(receiptHeader.IsMultiply, totalApplied, receiptHeader.ExchangeRate),
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.POST
                    };

                    _context.ARApplyHeaders.Add(applyHeader);

                    //INSERT DETAILS
                    List<Entities.ARApplyDetail> applyDetails = new List<Entities.ARApplyDetail>();

                    foreach (var item in receiptDetails)
                    {
                        var applyDetail = new Entities.ARApplyDetail()
                        {
                            ReceivableApplyDetailId = Guid.NewGuid(),
                            ReceivableApplyId = applyHeader.ReceivableApplyId,
                            ReceiveTransactionId = item.ReceiveTransactionId,
                            Description = item.Description,
                            OriginatingBalance = item.OriginatingBalance,
                            FunctionalBalance = CALC.FunctionalAmount(receiptHeader.IsMultiply, item.OriginatingBalance, receiptHeader.ExchangeRate),
                            OriginatingPaid = item.OriginatingPaid,
                            FunctionalPaid = CALC.FunctionalAmount(receiptHeader.IsMultiply, item.OriginatingPaid, receiptHeader.ExchangeRate),
                            Status = DOCSTATUS.NEW,
                        };

                        applyDetails.Add(applyDetail);

                        //UPDATE RECEIPT DETAIL
                        item.ReceivableApplyDetailId = applyDetail.ReceivableApplyDetailId;

                        _context.DepositSettlementDetails.Update(item);
                    }

                    await _context.ARApplyDetails.AddRangeAsync(applyDetails);

                    //CREATE DISTRIBUTION JOURNAL HERE
                    var resp = await _financeManager.CreateDistributionJournalAsync(applyHeader, applyDetails);

                    if (resp.Valid)
                    {
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        applyHeader = null;
                    }
                }
            }

            return applyHeader;
        }

        private async Task<Entities.CheckbookTransactionHeader> CreateCheckbook(Entities.DepositSettlementHeader header, List<Entities.DepositSettlementDetail> detail, string createdBy)
        {
            Entities.CheckbookTransactionHeader checkbookHeader = null;
            
            string documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(header.TransactionDate, header.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_OUT, _context.Database.CurrentTransaction.GetDbTransaction());

            if (!string.IsNullOrEmpty(documentUniqueNo))
            {
                decimal balance = header.OriginatingTotalPaid;
                decimal totalApply = detail != null ? detail.Sum(s => s.OriginatingPaid) : 0;
                balance = balance - totalApply;

                if (balance > 0)
                {
                    checkbookHeader = new Entities.CheckbookTransactionHeader()
                    {
                        CheckbookTransactionId = Guid.NewGuid(),
                        DocumentType = "OUT",
                        DocumentNo = documentUniqueNo,
                        TransactionDate = header.TransactionDate,
                        TransactionType = "NORMAL",
                        CurrencyCode = header.CurrencyCode,
                        ExchangeRate = header.ExchangeRate,
                        IsMultiply = header.IsMultiply,
                        CheckbookCode = header.CheckbookCode,
                        IsVoid = false,
                        VoidDocumentNo = "",
                        PaidSubject = "Customer",
                        SubjectCode = "",
                        Description = string.Format("DEPOSIT SETTLEMENT REFUND {0}", documentUniqueNo),
                        OriginatingTotalAmount = balance,
                        FunctionalTotalAmount = CALC.FunctionalAmount(header.IsMultiply, balance, header.ExchangeRate),
                        CreatedBy = header.CreatedBy,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.POST
                    };

                    _context.CheckbookTransactionHeaders.Add(checkbookHeader);

                    //INSERT DETAILS
                    List<Entities.CheckbookTransactionDetail> details = new List<Entities.CheckbookTransactionDetail>();

                    var depositDetail = (from dd in _context.ReceivableTransactionDetails
                                         join ch in _context.Charges on dd.ChargesId equals ch.ChargesId
                                         join dep in _context.DepositSettlementHeaders on dd.ReceiveTransactionId equals dep.ReceiveTransactionId
                                         where dep.SettlementHeaderId == header.SettlementHeaderId
                                         select new
                                         {
                                             ChargesId = dd.ChargesId,
                                             ChargeDescription = ch.ChargesName,
                                         }).FirstOrDefault();

                    if (depositDetail != null)
                    {
                        var checkbookDetail = new Entities.CheckbookTransactionDetail()
                        {
                            TransactionDetailId = Guid.NewGuid(),
                            CheckbookTransactionId = checkbookHeader.CheckbookTransactionId,
                            ChargesId = depositDetail.ChargesId,
                            ChargesDescription = depositDetail.ChargeDescription,
                            OriginatingAmount = balance,
                            FunctionalAmount = CALC.FunctionalAmount(header.IsMultiply, balance, header.ExchangeRate),
                            Status = DOCSTATUS.NEW
                        };

                        details.Add(checkbookDetail);

                        //UPDATE RECEIPT DETAIL
                        //item.ReceivableApplyDetailId = checkbookDetail.ReceivableApplyDetailId;

                    }

                    await _context.CheckbookTransactionDetails.AddRangeAsync(details);

                    //CREATE DISTRIBUTION JOURNAL HERE
                    var resp = await _financeManager.CreateDistributionJournalAsync(checkbookHeader, details);

                    if (resp.Valid)
                    {
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        checkbookHeader = null;
                    }
                }

            }


            return checkbookHeader;
        }
    }
}
