using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.ApPayment.PutStatusApprovalPayment
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _Flogcontext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext Flogcontext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _Flogcontext = Flogcontext;
            _financeManager = new FinanceManager(_context);
            _docGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.ApPaymentHeaders.FirstOrDefaultAsync(x => x.PaymentHeaderId == request.Body.PaymentHeaderId);
                    var currentApprover = await _context.ApPaymentApprovals.FirstOrDefaultAsync(x => x.PersonId == request.Body.PersonId && x.PaymentHeaderId == request.Body.PaymentHeaderId && x.Index == request.Body.CurrentIndex);
                    var currentApprovals = _context.ApPaymentApprovals.Where(x => x.PaymentHeaderId == request.Body.PaymentHeaderId && x.Index == currentApprover.Index);

                    var HasCheckoutApproval = _context.Checkbooks.Where(x => x.CheckbookCode == entryHeader.CheckbookCode).Select(x => x.HasCheckoutApproval).FirstOrDefault();

                    string docNo = "";
                    if (entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        if (request.Body.ActionDocStatus == DOCSTATUS.APPROVE)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                foreach (var item in currentApprovals)
                                {
                                    item.Status = DOCSTATUS.APPROVE;
                                    _context.ApPaymentApprovals.Update(item);
                                }

                                var statusApprove = new Entities.ApPaymentApprovalComment()
                                {
                                    PaymentApprovalCommentId = Guid.NewGuid(),
                                    PaymentApprovalId = currentApprover.PaymentApprovalId,
                                    Status = DOCSTATUS.APPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.ApPaymentApprovalComments.Add(statusApprove);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be approved.");
                            }
                        }
                        else if (request.Body.ActionDocStatus == DOCSTATUS.DISAPPROVE)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                entryHeader.Status = DOCSTATUS.DISAPPROVE;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.ApPaymentHeaders.Update(entryHeader);


                                foreach (var item in currentApprovals)
                                {
                                    item.Status = DOCSTATUS.DISAPPROVE;
                                    _context.ApPaymentApprovals.Update(item);
                                }

                                var statusApprove = new Entities.ApPaymentApprovalComment()
                                {
                                    PaymentApprovalCommentId = Guid.NewGuid(),
                                    PaymentApprovalId = currentApprover.PaymentApprovalId,
                                    Status = DOCSTATUS.DISAPPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.ApPaymentApprovalComments.Add(statusApprove);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be disapprove.");
                            }
                        }
                        else if (request.Body.ActionDocStatus == DOCSTATUS.POST)
                        {
                            //APPROVE & POSTING ONLY
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                foreach (var item in currentApprovals)
                                {
                                    item.Status = DOCSTATUS.APPROVE;
                                    _context.ApPaymentApprovals.Update(item);
                                }

                                var statusApprove = new Entities.ApPaymentApprovalComment()
                                {
                                    PaymentApprovalCommentId = Guid.NewGuid(),
                                    PaymentApprovalId = currentApprover.PaymentApprovalId,
                                    Status = DOCSTATUS.APPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.ApPaymentApprovalComments.Add(statusApprove);
                            }

                            if (entryHeader.Status == DOCSTATUS.PROCESS || entryHeader.Status == DOCSTATUS.NEW)
                            {
                                //CREATE AUTO POSTING APPLY FOR RECEIVABLE INVOICE
                                var apply = await ApplyPayableInvoice(entryHeader, request.Initiator.UserId);

                                if (apply.Valid)
                                {
                                    entryHeader.Status = DOCSTATUS.POST;
                                    entryHeader.ModifiedBy = request.Initiator.UserId;
                                    entryHeader.ModifiedDate = DateTime.Now;

                                    _context.ApPaymentHeaders.Update(entryHeader);

                                    //DO SOME POSTING HERE
                                    var resp = await _financeManager.PostJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_APPLY_PAYABLE, request.Initiator.UserId, entryHeader.TransactionDate);

                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError(apply.ErrorMessage);
                                }
                            }
                        }

                        else if (request.Body.ActionDocStatus == DOCSTATUS.NEW)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                entryHeader.Status = DOCSTATUS.NEW;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.ApPaymentHeaders.Update(entryHeader);

                                if (currentApprovals != null)
                                {
                                    foreach (var item in currentApprovals)
                                    {
                                        item.Status = DOCSTATUS.DISAPPROVE;
                                        _context.ApPaymentApprovals.Update(item);
                                    }

                                    var statusApprove = new Entities.ApPaymentApprovalComment()
                                    {
                                        PaymentApprovalCommentId = Guid.NewGuid(),
                                        PaymentApprovalId = currentApprover.PaymentApprovalId,
                                        Status = DOCSTATUS.DISAPPROVE,
                                        PersonId = Guid.Parse(request.Initiator.UserId),
                                        CommentDate = DateTime.Now,
                                        Comments = request.Body.Comments
                                    };

                                    _context.ApPaymentApprovalComments.Add(statusApprove);
                                }

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be approve.");
                            }
                        }
                        else if (request.Body.ActionDocStatus == DOCSTATUS.VOID)
                        {
                            if (entryHeader.Status == DOCSTATUS.POST)
                            {
                                entryHeader.Status = DOCSTATUS.VOID;
                                entryHeader.VoidBy = request.Initiator.UserId;
                                entryHeader.VoidDate = DateTime.Now;

                                _context.ApPaymentHeaders.Update(entryHeader);

                                if (currentApprovals != null)
                                {
                                    //HAS APPROVAL ONLY
                                    if (HasCheckoutApproval == true)
                                    {
                                        foreach (var item in currentApprovals)
                                        {
                                            item.Status = DOCSTATUS.VOID;
                                            _context.ApPaymentApprovals.Update(item);
                                        }

                                        var statusApprove = new Entities.ApPaymentApprovalComment()
                                        {
                                            PaymentApprovalCommentId = Guid.NewGuid(),
                                            PaymentApprovalId = currentApprover.PaymentApprovalId,
                                            Status = DOCSTATUS.VOID,
                                            PersonId = Guid.Parse(request.Initiator.UserId),
                                            CommentDate = DateTime.Now,
                                            Comments = request.Body.Comments
                                        };

                                        _context.ApPaymentApprovalComments.Add(statusApprove);
                                    }
                                }
                                
                                //VOID APPLY IF ANY
                                var allocHeader = (from rc in _context.ApPaymentDetails
                                                   join al in _context.APApplyDetails on rc.PayableApplyDetailId equals al.PayableApplyDetailId
                                                   join hd in _context.APApplyHeaders on al.PayableApplyId equals hd.PayableApplyId
                                                   where rc.PaymentHeaderId == entryHeader.PaymentHeaderId && rc.Status != DOCSTATUS.VOID && hd.Status == DOCSTATUS.POST
                                                   select hd).FirstOrDefault();

                                JournalResponse voidApply = new JournalResponse { Valid = true, ErrorMessage = "" };

                                if (allocHeader != null)
                                {
                                    voidApply = await _financeManager.VoidJournalAsync(allocHeader.DocumentNo, TRX_MODULE.TRX_APPLY_PAYABLE, request.Initiator.UserId, request.Body.ActionDate);
                                }

                                if (voidApply.Valid)
                                {
                                    allocHeader.Status = DOCSTATUS.VOID;
                                    allocHeader.StatusComment = request.Body.Comments;
                                    allocHeader.VoidBy = request.Initiator.UserId;
                                    allocHeader.VoidDate = request.Body.ActionDate;

                                    _context.APApplyHeaders.Update(allocHeader);

                                    //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                    var resp = await _financeManager.VoidJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_PAYMENT, request.Initiator.UserId, request.Body.ActionDate);

                                    if (resp.Valid)
                                    {
                                        await _context.SaveChangesAsync();
                                        transaction.Commit();

                                        return ApiResult<Response>.Ok(new Response()
                                        {
                                            PaymentHeaderId = request.Body.PaymentHeaderId,
                                            Message = string.Format("{0} status successfully Void !", docNo)
                                        });
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
                                return ApiResult<Response>.ValidationError("Record can not be approve.");
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
                        PaymentHeaderId = request.Body.PaymentHeaderId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.InternalServerError("[PutStatusApproval] " + ex.Message);
                }
            }
        }

        private async Task<JournalResponse> ApplyPayableInvoice(Entities.ApPaymentHeader header, string createdBy)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (header != null)
            {
                var details = _context.ApPaymentDetails.Where(x => x.PaymentHeaderId == header.PaymentHeaderId).ToList();

                if (details.Count() > 0)
                {
                    //CREATE CHECKBOOK
                    var apApply = await this.CreatePayableApply(header, details, createdBy);

                    if (apApply == null)
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Payable Apply can not be created !";
                        return resp;
                    }
                    else
                    {
                        resp.ValidMessage = string.Format("Apply Payable {0} successfully created.", apApply.DocumentNo); 
                    }
                }
            }

            return resp;
        }

        private async Task<Entities.APApplyHeader> CreatePayableApply(Entities.ApPaymentHeader paymentHeader, List<Entities.ApPaymentDetail> paymentDetails, string createdBy)
        {
            Entities.APApplyHeader applyHeader = null;

            if (paymentDetails != null && paymentDetails.Count > 0)
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(paymentHeader.TransactionDate, TRX_MODULE.TRX_PAYABLE, DOCNO_FEATURE.NOTRX_PAYABLE_APPLY, "", _context.Database.CurrentTransaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    decimal totalApplied = paymentDetails.Sum(s => s.OriginatingPaid);

                    applyHeader = new Entities.APApplyHeader()
                    {
                        PayableApplyId = Guid.NewGuid(),
                        TransactionDate = paymentHeader.TransactionDate,
                        DocumentType = DOCUMENTTYPE.PAYMENT,
                        DocumentNo = documentUniqueNo,
                        PaymentHeaderId = paymentHeader.PaymentHeaderId,
                        CheckbookTransactionId = Guid.Empty,
                        PayableTransactionId = Guid.Empty,
                        VendorId = paymentHeader.VendorId,
                        Description = paymentHeader.Description,
                        OriginatingTotalPaid = totalApplied,
                        FunctionalTotalPaid = CALC.FunctionalAmount(paymentHeader.IsMultiply, totalApplied, paymentHeader.ExchangeRate),
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.POST
                    };

                    _context.APApplyHeaders.Add(applyHeader);

                    //INSERT DETAILS
                    List<Entities.APApplyDetail> applyDetails = new List<Entities.APApplyDetail>();

                    foreach (var item in paymentDetails)
                    {
                        var applyDetail = new Entities.APApplyDetail()
                        {
                            PayableApplyDetailId = Guid.NewGuid(),
                            PayableApplyId = applyHeader.PayableApplyId,
                            PayableTransactionId = item.PayableTransactionId,
                            Description = item.Description,
                            OriginatingBalance = item.OriginatingBalance,
                            FunctionalBalance = CALC.FunctionalAmount(paymentHeader.IsMultiply, item.OriginatingBalance, paymentHeader.ExchangeRate),
                            OriginatingPaid = item.OriginatingPaid,
                            FunctionalPaid = CALC.FunctionalAmount(paymentHeader.IsMultiply, item.OriginatingPaid, paymentHeader.ExchangeRate),
                            Status = DOCSTATUS.NEW,
                        };

                        applyDetails.Add(applyDetail);

                        //UPDATE RECEIPT DETAIL
                        item.PayableApplyDetailId = applyDetail.PayableApplyDetailId;

                        _context.ApPaymentDetails.Update(item);
                    }

                    await _context.APApplyDetails.AddRangeAsync(applyDetails);

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
    }
}
