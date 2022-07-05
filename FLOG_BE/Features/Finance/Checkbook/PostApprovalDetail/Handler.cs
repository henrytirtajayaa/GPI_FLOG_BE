using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;

namespace FLOG_BE.Features.Finance.Checkbook.PostApprovalDetail
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _Flogcontext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext Flogcontext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _Flogcontext = Flogcontext;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    var CheckbookHeaders = await _context.CheckbookTransactionHeaders.FirstOrDefaultAsync(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId);

                    string docNo = "";

                    if (CheckbookHeaders != null)
                    {
                        docNo = CheckbookHeaders.DocumentNo;
                        CheckbookHeaders.Status = DOCSTATUS.PROCESS;
                        
                        _context.CheckbookTransactionHeaders.Update(CheckbookHeaders);

                        //ADD SUBMIT FOR APPROVAL COMMENTS
                        var firstApproval = _context.CheckbookTransactionApprovals.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId).OrderBy(o => o.Index).FirstOrDefault();

                        if(firstApproval != null)
                        {
                            var approvalComment = new Entities.CheckbookApprovalComment();
                            approvalComment.ApprovalCommentId = Guid.NewGuid();
                            approvalComment.CommentDate = DateTime.Now;
                            approvalComment.CheckbookTransactionApprovalId = firstApproval.CheckbookTransactionApprovalId;
                            approvalComment.PersonId = Guid.Parse(request.Initiator.UserId);
                            approvalComment.Comments = "Submit For Approval";
                            approvalComment.Status = DOCSTATUS.NEW;

                            _context.CheckbookApprovalComments.Add(approvalComment);
                        }

                        await _context.SaveChangesAsync();

                        var CekCheckbook = await _context.Checkbooks.FirstOrDefaultAsync(x => x.CheckbookCode == request.Body.CheckbookCode);
                        var getApprovalHeader = await _context.ApprovalSetupHeaders.FirstOrDefaultAsync(x => x.ApprovalCode == CekCheckbook.ApprovalCode);
                        var getApprovalDetail = _context.ApprovalSetupDetails.Where(x => x.ApprovalSetupHeaderId == getApprovalHeader.ApprovalSetupHeaderId);

                       
                        foreach (var item in getApprovalDetail)
                        {
                            if (item.PersonId != null)
                            {
                                var cekPerson = _context.CheckbookTransactionApprovals.FirstOrDefault(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId 
                                && x.PersonId == item.PersonId && x.Index == item.Level);
                                if (cekPerson != null)
                                {
                                    cekPerson.Status = DOCSTATUS.NEW;
                                    _context.CheckbookTransactionApprovals.Update(cekPerson);
                                }
                                else
                                {
                                    var transactionApproval = new Entities.CheckbookTransactionApproval()
                                    {
                                        CheckbookTransactionApprovalId = Guid.NewGuid(),
                                        CheckbookTransactionId = request.Body.CheckbookTransactionId,
                                        Index = item.Level,
                                        PersonId = item.PersonId,
                                        PersonCategoryId = item.PersonCategoryId,
                                        Status = DOCSTATUS.NEW
                                    };
                                    _context.CheckbookTransactionApprovals.Add(transactionApproval);
                                }
                            }
                            else
                            {
                                var getPersonId = _Flogcontext.Persons.Where(x => x.PersonCategoryId == item.PersonCategoryId.ToString());
                                foreach (var child in getPersonId)
                                {
                                    var CekPersonCat = _context.CheckbookTransactionApprovals.FirstOrDefault(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId
                                     && x.PersonCategoryId == item.PersonCategoryId && x.Index == item.Level);
                                    if (CekPersonCat != null)
                                    {
                                        CekPersonCat.Status = DOCSTATUS.NEW;
                                        _context.CheckbookTransactionApprovals.Update(CekPersonCat);
                                    }
                                    else
                                    {
                                        var transactionApproval = new Entities.CheckbookTransactionApproval()
                                        {
                                            CheckbookTransactionApprovalId = Guid.NewGuid(),
                                            CheckbookTransactionId = request.Body.CheckbookTransactionId,
                                            Index = item.Level,
                                            PersonId = Guid.Parse(child.PersonId),
                                            PersonCategoryId = item.PersonCategoryId,
                                            Status = DOCSTATUS.NEW
                                        };

                                        _context.CheckbookTransactionApprovals.Add(transactionApproval);
                                    }
                                }
                            }

                        }
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    var response = new Response()
                    {
                        CheckbookTransactionId = request.Body.CheckbookTransactionId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(CheckbookHeaders.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.InternalServerError("[PutStatusCheckbookTransaction] " + ex.Message);
                }
            }
        }
    }
}
