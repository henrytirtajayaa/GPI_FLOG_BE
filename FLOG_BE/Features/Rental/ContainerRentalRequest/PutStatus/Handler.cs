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

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutStatus
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
                    var RentalRequest = await _context.ContainerRentalRequestHeaders.FirstOrDefaultAsync(x => x.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId);

                    string docNo = "";
                    if (RentalRequest != null)
                    {
                        docNo = RentalRequest.DocumentNo;

                        if (request.Body.Status == DOCSTATUS.SUBMIT)
                        {
                            if (RentalRequest.Status == DOCSTATUS.OPEN)
                            {
                                RentalRequest.Status = DOCSTATUS.SUBMIT;
                                RentalRequest.ModifiedBy = request.Initiator.UserId;
                                RentalRequest.ModifiedDate = DateTime.Now;

                                _context.ContainerRentalRequestHeaders.Update(RentalRequest);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be deleted!");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.CANCEL)
                        {
                            if (RentalRequest.Status == DOCSTATUS.SUBMIT)
                            {
                                RentalRequest.Status = DOCSTATUS.CANCEL;
                                RentalRequest.CanceledBy = request.Initiator.UserId;
                                RentalRequest.CanceledDate = DateTime.Now;

                                _context.ContainerRentalRequestHeaders.Update(RentalRequest);

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
                        ContainerRentalRequestHeaderId = request.Body.ContainerRentalRequestHeaderId,
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
