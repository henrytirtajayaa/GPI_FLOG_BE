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

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PutStatus
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var DoConfirm = await _context.ContainerRequestConfirmHeaders.FirstOrDefaultAsync(x => x.ContainerRequestConfirmHeaderId == request.Body.ContainerRequestConfirmHeaderId);

                    string docNo = "";
                    if (DoConfirm != null)
                    {
                        docNo = DoConfirm.DocumentNo;

                        if (request.Body.Status == DOCSTATUS.CONFIRM)
                        {
                            if (DoConfirm.Status == DOCSTATUS.NEW)
                            {
                                DoConfirm.Status = DOCSTATUS.CONFIRM;
                                DoConfirm.ModifiedBy = request.Initiator.UserId;
                                DoConfirm.ModifiedDate = DateTime.Now;

                                _context.ContainerRequestConfirmHeaders.Update(DoConfirm);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else if (DoConfirm.Status == DOCSTATUS.CONFIRM)
                            {
                                DoConfirm.ExpiredDate = request.Body.ExpiredDate;

                                _context.ContainerRequestConfirmHeaders.Update(DoConfirm);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be deleted!");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.EXPIRE)
                        {
                            if (DoConfirm.Status == DOCSTATUS.CONFIRM)
                            {
                                DoConfirm.ExpiredDate = request.Body.ExpiredDate;

                                _context.ContainerRequestConfirmHeaders.Update(DoConfirm);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be extend!");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.CANCEL)
                        {
                            if (DoConfirm.Status == DOCSTATUS.CONFIRM)
                            {
                                DoConfirm.Status = DOCSTATUS.CANCEL;
                                DoConfirm.ModifiedBy = request.Initiator.UserId;
                                DoConfirm.ModifiedDate = DateTime.Now;

                                _context.ContainerRequestConfirmHeaders.Update(DoConfirm);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be cancelled!");
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Document not available");
                    }

                    var response = new Response()
                    {
                        ContainerRequestConfirmHeaderId = request.Body.ContainerRequestConfirmHeaderId,
                        ExpiredDate = request.Body.ExpiredDate,
                        Status = request.Body.Status
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.InternalServerError("Cannot put status!");
                }
            }
        }
    }
}
