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
using FLOG_BE.Model.Companies.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Finance.Sales.Quotation.PutStatusQuotation
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private FLOG.Core.Finance.Util.IFinanceManager _financeManager;
        private readonly HATEOASLinkCollection _linkCollection;

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
                    var entryHeader = await _context.SalesQuotationHeaders.FirstOrDefaultAsync(x => x.SalesQuotationId == request.Body.SalesQuotationId);

                    string docNo = "";
                    if (entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;


                        if (entryHeader.Status == DOCSTATUS.NEW)
                        {
                            if (request.Body.Status == DOCSTATUS.CANCEL)
                            {
                                entryHeader.Status = DOCSTATUS.CANCEL;
                                entryHeader.StatusComment = request.Body.StatusComment;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;
                            }
                            else if (request.Body.Status == DOCSTATUS.SUBMIT)
                            {
                                entryHeader.Status = DOCSTATUS.SUBMIT;
                                entryHeader.StatusComment = request.Body.StatusComment;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;
                            }
                            
                            await _context.SaveChangesAsync();
                            transaction.Commit();
                        }else if (entryHeader.Status == DOCSTATUS.SUBMIT)
                        {
                            entryHeader.Status = DOCSTATUS.CLOSE;
                            entryHeader.StatusComment = request.Body.StatusComment;
                            entryHeader.ModifiedBy = request.Initiator.UserId;
                            entryHeader.ModifiedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError("Record can not be deleted.");
                        }

                    }

                    var response = new Response()
                    {
                        SalesQuotationId = request.Body.SalesQuotationId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError(ex.Message);
                }
            }


        }

    }
}
