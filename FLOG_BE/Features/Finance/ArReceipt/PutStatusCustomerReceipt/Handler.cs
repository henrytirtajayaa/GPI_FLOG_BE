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

namespace FLOG_BE.Features.Finance.ArReceipt.PutStatusCustomerReceipt
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
                    var ReceiptHeader = await _context.ArReceiptHeaders.FirstOrDefaultAsync(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId);

                    string docNo = "";

                    if (ReceiptHeader != null)
                    {
                        docNo = ReceiptHeader.DocumentNo;

                        if (request.Body.Status == DOCSTATUS.DELETE)
                        {
                            if (ReceiptHeader.Status == DOCSTATUS.NEW)
                            {
                                ReceiptHeader.Status = DOCSTATUS.DELETE;
                                ReceiptHeader.ModifiedBy = request.Initiator.UserId;
                                ReceiptHeader.ModifiedDate = DateTime.Now;

                                _context.ArReceiptHeaders.Update(ReceiptHeader);

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
                            if (ReceiptHeader.Status == DOCSTATUS.NEW)
                            {
                                //CREATE AUTO POSTING APPLY FOR RECEIVABLE INVOICE
                                var apply = await ApplyReceivableInvoice(ReceiptHeader, request.Initiator.UserId);

                                if (apply.Valid)
                                {
                                    ReceiptHeader.Status = DOCSTATUS.POST;
                                    ReceiptHeader.ModifiedBy = request.Initiator.UserId;
                                    ReceiptHeader.ModifiedDate = DateTime.Now;

                                    _context.ArReceiptHeaders.Update(ReceiptHeader);

                                    //DO SOME POSTING HERE
                                    var resp = await _financeManager.PostJournalAsync(ReceiptHeader.DocumentNo, TRX_MODULE.TRX_APPLY_RECEIPT, request.Initiator.UserId, ReceiptHeader.TransactionDate);

                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError(apply.ErrorMessage);
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
                            if (ReceiptHeader.Status == DOCSTATUS.POST)
                            {
                                ReceiptHeader.Status = DOCSTATUS.VOID;
                                ReceiptHeader.VoidBy = request.Initiator.UserId;
                                ReceiptHeader.VoidDate = request.Body.ActionDate;
                                ReceiptHeader.StatusComment = request.Body.StatusComment;

                                _context.ArReceiptHeaders.Update(ReceiptHeader);

                                //VOID APPLY IF ANY
                                var allocHeader = (from rc in _context.ArReceiptDetails
                                                   join al in _context.ARApplyDetails on rc.ReceivableApplyDetailId equals al.ReceivableApplyDetailId
                                                   join hd in _context.ARApplyHeaders on al.ReceivableApplyId equals hd.ReceivableApplyId
                                                   where rc.ReceiptHeaderId == ReceiptHeader.ReceiptHeaderId && rc.Status != DOCSTATUS.VOID && hd.Status == DOCSTATUS.POST 
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
                                    var resp = await _financeManager.VoidJournalAsync(ReceiptHeader.DocumentNo, TRX_MODULE.TRX_RECEIPT, request.Initiator.UserId, request.Body.ActionDate);

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
                        ReceiptHeaderId = request.Body.ReceiptHeaderId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError("[PutStatusCustomerReceipt] " + ex.Message);
                }
            }
        }

        private async Task<JournalResponse> ApplyReceivableInvoice(Entities.ArReceiptHeader header, string createdBy)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (header != null)
            {
                var details = _context.ArReceiptDetails.Where(x => x.ReceiptHeaderId == header.ReceiptHeaderId).ToList();

                if(details.Count() > 0)
                {
                    //CREATE CHECKBOOK
                    var jResp = await this.CreateReceivableApply(header, details, createdBy);

                    if (jResp == null)
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = jResp.ErrorMessage;
                        return resp;
                    }
                }                
            }

            return resp;
        }

        private async Task<JournalResponse> CreateReceivableApply(Entities.ArReceiptHeader receiptHeader, List<Entities.ArReceiptDetail> receiptDetails, string createdBy)
        {
            JournalResponse jresp = new JournalResponse();
            jresp.Valid = true;
            jresp.ErrorMessage = "";

            if (receiptDetails != null && receiptDetails.Count > 0)
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(receiptHeader.TransactionDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.NOTRX_RECEIVABLE_APPLY, "", _context.Database.CurrentTransaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    decimal totalApplied = receiptDetails.Sum(s => s.OriginatingPaid);

                    var applyHeader = new Entities.ARApplyHeader()
                    {
                        ReceivableApplyId = Guid.NewGuid(),
                        TransactionDate = receiptHeader.TransactionDate,
                        DocumentType = DOCUMENTTYPE.RECEIPT,
                        DocumentNo = documentUniqueNo,
                        ReceiptHeaderId = receiptHeader.ReceiptHeaderId,
                        CheckbookTransactionId = Guid.Empty,
                        ReceiveTransactionId = Guid.Empty,
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

                        _context.ArReceiptDetails.Update(item);
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
                        return resp;
                    }
                }
                else
                {
                    jresp.Valid = false;
                    jresp.ErrorMessage = "Apply Document No can not be created !";
                }
            }

            return jresp;
        }
    }

    

}
