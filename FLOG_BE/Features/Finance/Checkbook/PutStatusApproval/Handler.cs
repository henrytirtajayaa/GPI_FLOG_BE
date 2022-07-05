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

namespace FLOG_BE.Features.Finance.Checkbook.PutStatusApproval
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
                    var entryHeader = await _context.CheckbookTransactionHeaders.FirstOrDefaultAsync(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId);
                    var currentPerson = await _context.CheckbookTransactionApprovals.FirstOrDefaultAsync(x => x.PersonId == request.Body.PersonId && x.CheckbookTransactionId == request.Body.CheckbookTransactionId && x.Index == request.Body.CurrentIndex);
                    var currentIndex = _context.CheckbookTransactionApprovals.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId && x.Index == currentPerson.Index);

                    string docNo = "";
                    if(entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        if (request.Body.ActionDocStatus == DOCSTATUS.APPROVE)
                        {
                            if(entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                //entryHeader.Status = DOCSTATUS.APPROVE;
                                //entryHeader.ModifiedBy = request.Initiator.UserId;
                                //entryHeader.ModifiedDate = DateTime.Now;

                               // _context.CheckbookTransactionHeaders.Update(entryHeader);

                                
                                foreach (var item in currentIndex)
                                {
                                    item.Status = DOCSTATUS.APPROVE;
                                    _context.CheckbookTransactionApprovals.Update(item);
                                }

                                var statusApprove = new Entities.CheckbookApprovalComment()
                                {
                                    ApprovalCommentId = Guid.NewGuid(),
                                    CheckbookTransactionApprovalId = currentPerson.CheckbookTransactionApprovalId,
                                    Status = DOCSTATUS.APPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.CheckbookApprovalComments.Add(statusApprove);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be approved.");
                            }
                        }else if(request.Body.ActionDocStatus == DOCSTATUS.DISAPPROVE)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                entryHeader.Status = DOCSTATUS.DISAPPROVE;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.CheckbookTransactionHeaders.Update(entryHeader);


                                foreach (var item in currentIndex)
                                {
                                    item.Status = DOCSTATUS.DISAPPROVE;
                                    _context.CheckbookTransactionApprovals.Update(item);
                                }

                                var statusApprove = new Entities.CheckbookApprovalComment()
                                {
                                    ApprovalCommentId = Guid.NewGuid(),
                                    CheckbookTransactionApprovalId = currentPerson.CheckbookTransactionApprovalId,
                                    Status = DOCSTATUS.DISAPPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.CheckbookApprovalComments.Add(statusApprove);

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
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                entryHeader.Status = DOCSTATUS.POST;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.CheckbookTransactionHeaders.Update(entryHeader);
                                
                                foreach (var item in currentIndex)
                                {
                                    item.Status = DOCSTATUS.APPROVE;
                                    _context.CheckbookTransactionApprovals.Update(item);
                                }

                                var statusApprove = new Entities.CheckbookApprovalComment()
                                {
                                    ApprovalCommentId = Guid.NewGuid(),
                                    CheckbookTransactionApprovalId = currentPerson.CheckbookTransactionApprovalId,
                                    Status = DOCSTATUS.APPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.CheckbookApprovalComments.Add(statusApprove);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be approve.");
                            }
                        }

                        else if (request.Body.ActionDocStatus == DOCSTATUS.NEW)
                        {
                            if (entryHeader.Status == DOCSTATUS.PROCESS)
                            {
                                entryHeader.Status = DOCSTATUS.NEW;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.CheckbookTransactionHeaders.Update(entryHeader);


                                foreach (var item in currentIndex)
                                {
                                    item.Status = DOCSTATUS.DISAPPROVE;
                                    _context.CheckbookTransactionApprovals.Update(item);
                                }

                                var statusApprove = new Entities.CheckbookApprovalComment()
                                {
                                    ApprovalCommentId = Guid.NewGuid(),
                                    CheckbookTransactionApprovalId = currentPerson.CheckbookTransactionApprovalId,
                                    Status = DOCSTATUS.DISAPPROVE,
                                    PersonId = Guid.Parse(request.Initiator.UserId),
                                    CommentDate = DateTime.Now,
                                    Comments = request.Body.Comments
                                };

                                _context.CheckbookApprovalComments.Add(statusApprove);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
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
                        CheckbookTransactionId = request.Body.CheckbookTransactionId,
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
    }
}
