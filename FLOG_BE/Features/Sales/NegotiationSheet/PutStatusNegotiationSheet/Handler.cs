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
using FLOG_BE.Model.Central;

namespace FLOG_BE.Features.Sales.NegotiationSheet.PutStatusNegotiationSheet
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _flogContext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext flogContext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _flogContext = flogContext;
            _login = login;
            _financeManager = new FinanceManager(_context);

        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.NegotiationSheetHeaders.FirstOrDefaultAsync(x => x.NegotiationSheetId == request.Body.NegotiationSheetId);

                    string docNo = "";
                    if(entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        if(request.Body.ActionDocStatus == DOCSTATUS.PROCESS)
                        {
                            if (entryHeader.Status == DOCSTATUS.NEW || entryHeader.Status == DOCSTATUS.REVISE)
                            {
                                //DO SOME APPROVALS HERE
                                JournalResponse validApproval = await CreateApprovalTransaction(entryHeader, request);

                                if(validApproval.Valid && validApproval.ValidStatus > 0)
                                {
                                    entryHeader.Status = validApproval.ValidStatus;
                                    entryHeader.ModifiedBy = request.Initiator.UserId;
                                    entryHeader.ModifiedDate = DateTime.Now;

                                    _context.NegotiationSheetHeaders.Update(entryHeader);

                                    await _context.SaveChangesAsync();

                                    transaction.Commit();

                                    var okResponse = new Response()
                                    {
                                        NegotiationSheetId = request.Body.NegotiationSheetId,
                                        Message = string.Format("{0} status successfully Submitted For Approval !", docNo)
                                    };

                                    return ApiResult<Response>.Ok(okResponse);
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(validApproval.ErrorMessage) ? validApproval.ErrorMessage : "Approvals can not be created !");
                                }                    
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Only Record with status NEW can be processed to Approval !");
                            }
                        }
                        else if(request.Body.ActionDocStatus == DOCSTATUS.APPROVE)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                if(request.Body.ApprovalIndex > 0)
                                {
                                    //DO SOME APPROVALS HERE
                                    JournalResponse validApproval = await DoApproveTransaction(entryHeader, request);

                                    if (validApproval.Valid)
                                    {
                                        if(validApproval.ValidStatus == DOCSTATUS.APPROVE)
                                        {
                                            //FINAL NEXT APPROVER
                                            entryHeader.Status = DOCSTATUS.APPROVE;
                                            entryHeader.ModifiedBy = request.Initiator.UserId;
                                            entryHeader.ModifiedDate = DateTime.Now;
                                            
                                            _context.NegotiationSheetHeaders.Update(entryHeader);

                                            await _context.SaveChangesAsync();

                                            transaction.Commit();

                                            var okResponse = new Response()
                                            {
                                                NegotiationSheetId = request.Body.NegotiationSheetId,
                                                Message = string.Format("{0} successfully {1}", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus))
                                            };

                                            return ApiResult<Response>.Ok(okResponse);
                                        }
                                        else
                                        {
                                            await _context.SaveChangesAsync();

                                            transaction.Commit();
                                            
                                            //HAVE NEXT APPROVER
                                            var okResponse = new Response()
                                            {
                                                NegotiationSheetId = request.Body.NegotiationSheetId,
                                                Message = string.Format("{0} successfully {1} and waiting for Next Approver decision.", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus))
                                            };

                                            return ApiResult<Response>.Ok(okResponse);
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();

                                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(validApproval.ErrorMessage) ? validApproval.ErrorMessage : "Approval workflows is invalid !");
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Approval index is not valid !");
                                }                                
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be Approved !");
                            }
                        }
                        else if (request.Body.ActionDocStatus == DOCSTATUS.DISAPPROVE)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                if (request.Body.ApprovalIndex > 0)
                                {
                                    //DO SOME APPROVALS HERE
                                    JournalResponse validApproval = await DoDisapproveTransaction(entryHeader, request);

                                    if (validApproval.Valid)
                                    {
                                        entryHeader.Status = DOCSTATUS.CANCEL;
                                        entryHeader.ModifiedBy = request.Initiator.UserId;
                                        entryHeader.ModifiedDate = DateTime.Now;

                                        _context.NegotiationSheetHeaders.Update(entryHeader);

                                        await _context.SaveChangesAsync();

                                        transaction.Commit();

                                        var okResponse = new Response()
                                        {
                                            NegotiationSheetId = request.Body.NegotiationSheetId,
                                            Message = string.Format("{0} successfully Disapproved !", docNo)
                                        };

                                        return ApiResult<Response>.Ok(okResponse);
                                    }
                                    else
                                    {
                                        transaction.Rollback();

                                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(validApproval.ErrorMessage) ? validApproval.ErrorMessage : "Approval workflows is invalid !");
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Approval index is not valid !");
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be Disapproved !");
                            }
                        }
                        else if (request.Body.ActionDocStatus == DOCSTATUS.REVISE)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                if (request.Body.ApprovalIndex > 0)
                                {
                                    //DO SOME APPROVALS HERE
                                    JournalResponse validApproval = await DoReviseTransaction(entryHeader, request);

                                    if (validApproval.Valid)
                                    {
                                        entryHeader.Status = DOCSTATUS.REVISE;
                                        entryHeader.ModifiedBy = request.Initiator.UserId;
                                        entryHeader.ModifiedDate = DateTime.Now;

                                        _context.NegotiationSheetHeaders.Update(entryHeader);

                                        await _context.SaveChangesAsync();

                                        transaction.Commit();

                                        var okResponse = new Response()
                                        {
                                            NegotiationSheetId = request.Body.NegotiationSheetId,
                                            Message = string.Format("{0} successfully Revised. Document now editable !", docNo)
                                        };

                                        return ApiResult<Response>.Ok(okResponse);
                                    }
                                    else
                                    {
                                        transaction.Rollback();

                                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(validApproval.ErrorMessage) ? validApproval.ErrorMessage : "Approval workflows is invalid !");
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Approval index is not valid !");
                                }
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be Revised !");
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
                        NegotiationSheetId = request.Body.NegotiationSheetId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus)) 
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError("[PutStatusNegotiationSheet] " + ex.Message);
                }
            }
        }

        private async Task<JournalResponse> CreateApprovalTransaction(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            // START REMOVE EXISTING
            var existingApprovals =_context.TrxDocumentApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                    && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                    && x.ModeStatus == DOCSTATUS.NEW && x.TransactionId == ns.NegotiationSheetId).ToList();

            foreach(var appr in existingApprovals)
            {
                //_context.TrxDocumentApprovalComments.Where(x => x.TrxDocumentApprovalId == appr.TrxDocumentApprovalId).ToList().ForEach(p => _context.Remove(p));
                _context.Remove(appr);
            }

            await _context.SaveChangesAsync();

            //END REMOVE EXISTING

            //OBTAIN CENTRAL MEMBER
            var personsInCategory = _flogContext.Persons.Where(x => !string.IsNullOrEmpty(x.PersonCategoryId) && !x.InActive).ToList();

            //CREATE NEW APPROVAL
            var newNSApproval = _context.FNDocNumberSetupApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                    && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                    && x.ModeStatus == DOCSTATUS.NEW && !string.IsNullOrEmpty(x.ApprovalCode)).FirstOrDefault();

            if (newNSApproval != null)
            {
                var approvalSetups = await (from d in _context.ApprovalSetupDetails
                                            join h in _context.ApprovalSetupHeaders on d.ApprovalSetupHeaderId equals h.ApprovalSetupHeaderId
                                            where h.ApprovalCode.Equals(newNSApproval.ApprovalCode, StringComparison.OrdinalIgnoreCase)
                                            orderby d.Level ascending
                                            select d).ToListAsync();

                List<Entities.TrxDocumentApproval> newNSApprovers = new List<Entities.TrxDocumentApproval>();
                foreach (var approver in approvalSetups)
                {
                    if(approver.PersonCategoryId != null)
                    {
                        var members = personsInCategory.Where(x => x.PersonCategoryId.Equals(approver.PersonCategoryId.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();

                        if(members.Count() > 0)
                        {
                            foreach(var member in members)
                            {
                                var nsApprover = new Entities.TrxDocumentApproval
                                {
                                    TrxDocumentApprovalId = Guid.NewGuid(),
                                    TrxModule = newNSApproval.TrxModule,
                                    TransactionType = newNSApproval.TransactionType,
                                    DocFeatureId = newNSApproval.DocFeatureId,
                                    ModeStatus = newNSApproval.ModeStatus,
                                    TransactionId = ns.NegotiationSheetId,
                                    Index = approver.Level,
                                    Status = DOCSTATUS.NEW,
                                    PersonId = Guid.Parse(member.PersonId),
                                    PersonCategoryId = Guid.Empty
                                };

                                newNSApprovers.Add(nsApprover);
                            }
                        }
                        else
                        {
                            resp.ValidStatus = 0;
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Approver in Level {0} Group have no member !", approver.Level);
                            break;
                        }
                    }
                    else
                    {
                        var nsApprover = new Entities.TrxDocumentApproval
                        {
                            TrxDocumentApprovalId = Guid.NewGuid(),
                            TrxModule = newNSApproval.TrxModule,
                            TransactionType = newNSApproval.TransactionType,
                            DocFeatureId = newNSApproval.DocFeatureId,
                            ModeStatus = newNSApproval.ModeStatus,
                            TransactionId = ns.NegotiationSheetId,
                            Index = approver.Level,
                            Status = DOCSTATUS.NEW,
                            PersonId = approver.PersonId,
                            PersonCategoryId = Guid.Empty
                        };

                        newNSApprovers.Add(nsApprover);
                    }                    
                }

                if (resp.Valid)
                {
                    foreach (var appr in existingApprovals)
                    {
                        var edittedComments = _context.TrxDocumentApprovalComments.Where(x => x.TrxDocumentApprovalId == appr.TrxDocumentApprovalId).ToList();

                        foreach(var editted in edittedComments)
                        {
                            editted.TrxDocumentApprovalId = newNSApprovers.ElementAt(0).TrxDocumentApprovalId;

                            _context.TrxDocumentApprovalComments.Update(editted);
                        }
                    }

                    //START For APPROVAL COMMENT
                    var comment = new Entities.TrxDocumentApprovalComment
                    {
                        TrxDocumentApprovalCommentId = Guid.NewGuid(),
                        TrxDocumentApprovalId = newNSApprovers.ElementAt(0).TrxDocumentApprovalId,
                        Status = DOCSTATUS.PROCESS,
                        CommentDate = DateTime.Now,
                        Comments = "Submit For Approval",
                        PersonId = Guid.Parse(req.Initiator.UserId),
                    };

                    await _context.TrxDocumentApprovalComments.AddAsync(comment);

                    if (newNSApprovers.Count > 0)
                    {
                        resp.ValidStatus = DOCSTATUS.PROCESS;
                        await _context.TrxDocumentApprovals.AddRangeAsync(newNSApprovers);
                    }
                }
            }
            else
            {
                //NO APPROVAL NEEDED
                var header = new Entities.TrxDocumentApproval
                {
                    TrxDocumentApprovalId = Guid.NewGuid(),
                    TrxModule = TRX_MODULE.TRX_SALES,
                    TransactionType = ns.TransactionType,
                    DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET,
                    ModeStatus = DOCSTATUS.NEW,
                    Status = DOCSTATUS.APPROVE,
                    Index = 1,
                    PersonId = Guid.Parse(req.Initiator.UserId),
                    PersonCategoryId = Guid.Empty,
                    TransactionId = ns.NegotiationSheetId,
                };

                await _context.TrxDocumentApprovals.AddAsync(header);

                var comment = new Entities.TrxDocumentApprovalComment
                {
                    TrxDocumentApprovalCommentId = Guid.NewGuid(),
                    TrxDocumentApprovalId = header.TrxDocumentApprovalId,
                    Status = DOCSTATUS.APPROVE,
                    CommentDate = DateTime.Now,
                    Comments = "No Approval Needed or Approved beacuse Approval Not Set.",
                    PersonId = (Guid)header.PersonId,
                };

                await _context.TrxDocumentApprovalComments.AddAsync(comment);

                resp.ValidStatus = DOCSTATUS.APPROVE;
            }

            return resp;
        }

        private async Task<JournalResponse> DoApproveTransaction(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            if(req.Body.ApprovalIndex > 0)
            {
                int statusApprove = DOCSTATUS.APPROVE;

                //UPDATE CURRENT APPROVER LEVEL
                var currentNSApprovals = _context.TrxDocumentApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                    && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                    && x.ModeStatus == DOCSTATUS.NEW
                    && x.TransactionId == ns.NegotiationSheetId && x.Index == req.Body.ApprovalIndex).ToList();

                foreach(var nsApproval in currentNSApprovals)
                {
                    nsApproval.Status = statusApprove;

                    _context.TrxDocumentApprovals.Update(nsApproval);
                }

                //CREATE COMMENTS
                var thisApprover = currentNSApprovals.Where(x => x.PersonId.ToString().Equals(req.Initiator.UserId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                var comment = new Entities.TrxDocumentApprovalComment
                {
                    TrxDocumentApprovalCommentId = Guid.NewGuid(),
                    TrxDocumentApprovalId = (thisApprover != null ? thisApprover.TrxDocumentApprovalId : currentNSApprovals.ElementAt(0).TrxDocumentApprovalId),
                    CommentDate = DateTime.Now,
                    PersonId = Guid.Parse(req.Initiator.UserId),
                    Comments = req.Body.Comments,
                    Status = statusApprove,                    
                };

                await _context.TrxDocumentApprovalComments.AddAsync(comment);

                //CHECK NEXT APPROVER
                var hasNextApprovers = _context.TrxDocumentApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                   && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                   && x.ModeStatus == DOCSTATUS.NEW
                   && x.TransactionId == ns.NegotiationSheetId && x.Index > req.Body.ApprovalIndex && x.Status == DOCSTATUS.NEW).Any();

                if (hasNextApprovers)
                {
                    resp.ValidStatus = DOCSTATUS.PROCESS;
                }
                else
                {
                    resp.ValidStatus = DOCSTATUS.APPROVE;
                }                
            }
            else
            {
                resp.ValidStatus = 0;
                resp.Valid = false;
                resp.ErrorMessage = string.Format("{0} Approval Index not valid !", ns.DocumentNo);
            }

            return resp;
        }

        private async Task<JournalResponse> DoDisapproveTransaction(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            if (req.Body.ApprovalIndex > 0)
            {
                int statusDisapprove = DOCSTATUS.DISAPPROVE;

                //UPDATE CURRENT APPROVER LEVEL
                var currentNSApprovals = _context.TrxDocumentApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                    && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                    && x.ModeStatus == DOCSTATUS.NEW
                    && x.TransactionId == ns.NegotiationSheetId && x.Index == req.Body.ApprovalIndex).ToList();

                foreach (var nsApproval in currentNSApprovals)
                {
                    nsApproval.Status = statusDisapprove;

                    _context.TrxDocumentApprovals.Update(nsApproval);
                }

                //CREATE COMMENTS
                var thisApprover = currentNSApprovals.Where(x => x.PersonId.ToString().Equals(req.Initiator.UserId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                var comment = new Entities.TrxDocumentApprovalComment
                {
                    TrxDocumentApprovalCommentId = Guid.NewGuid(),
                    TrxDocumentApprovalId = (thisApprover != null ? thisApprover.TrxDocumentApprovalId : currentNSApprovals.ElementAt(0).TrxDocumentApprovalId),
                    CommentDate = DateTime.Now,
                    PersonId = Guid.Parse(req.Initiator.UserId),
                    Comments = req.Body.Comments,
                    Status = statusDisapprove,
                };

                await _context.TrxDocumentApprovalComments.AddAsync(comment);

                resp.ValidStatus = statusDisapprove;
            }
            else
            {
                resp.ValidStatus = 0;
                resp.Valid = false;
                resp.ErrorMessage = string.Format("{0} Approval Index not valid !", ns.DocumentNo);
            }

            return resp;
        }

        private async Task<JournalResponse> DoReviseTransaction(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            if (req.Body.ApprovalIndex > 0)
            {
                int statusDisapprove = DOCSTATUS.REVISE;

                //UPDATE CURRENT APPROVER LEVEL
                var currentNSApprovals = _context.TrxDocumentApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                    && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                    && x.ModeStatus == DOCSTATUS.NEW
                    && x.TransactionId == ns.NegotiationSheetId && x.Index == req.Body.ApprovalIndex).ToList();

                foreach (var nsApproval in currentNSApprovals)
                {
                    nsApproval.Status = statusDisapprove;

                    _context.TrxDocumentApprovals.Update(nsApproval);
                }

                //CREATE COMMENTS
                var thisApprover = currentNSApprovals.Where(x => x.PersonId.ToString().Equals(req.Initiator.UserId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                var comment = new Entities.TrxDocumentApprovalComment
                {
                    TrxDocumentApprovalCommentId = Guid.NewGuid(),
                    TrxDocumentApprovalId = (thisApprover != null ? thisApprover.TrxDocumentApprovalId : currentNSApprovals.ElementAt(0).TrxDocumentApprovalId),
                    CommentDate = DateTime.Now,
                    PersonId = Guid.Parse(req.Initiator.UserId),
                    Comments = req.Body.Comments,
                    Status = statusDisapprove,
                };

                await _context.TrxDocumentApprovalComments.AddAsync(comment);

                //REMOVE ANY NEXT APPROVERs
                _context.TrxDocumentApprovals.Where(x => x.TrxModule == TRX_MODULE.TRX_SALES
                    && x.TransactionType == ns.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                    && x.ModeStatus == DOCSTATUS.NEW
                    && x.TransactionId == ns.NegotiationSheetId && x.Index > req.Body.ApprovalIndex && x.Status == DOCSTATUS.NEW).ToList()
                    .ForEach(p => _context.Remove(p));

                resp.ValidStatus = statusDisapprove;
            }
            else
            {
                resp.ValidStatus = 0;
                resp.Valid = false;
                resp.ErrorMessage = string.Format("{0} Approval Index not valid !", ns.DocumentNo);
            }

            return resp;
        }

    }
}
