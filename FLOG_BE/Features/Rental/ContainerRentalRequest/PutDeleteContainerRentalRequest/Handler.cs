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

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutDeleteContainerRentalRequest
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
            var response = new Response()
            { 
                ContainerRentalRequestHeaderId = request.Body.ContainerRentalRequestHeaderId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var RentalRequest = await _context.ContainerRentalRequestHeaders.FirstOrDefaultAsync(x => x.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId);
                    if (RentalRequest != null)
                    {
                        if (RentalRequest.Status == DOCSTATUS.OPEN || RentalRequest.Status == DOCSTATUS.NEW)
                        {
                            _context.ContainerRentalRequestHeaders.Where(x => x.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId).ToList().ForEach(p => _context.Remove(p));
                            _context.ContainerRentalRequestDetails.Where(x => x.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId).ToList().ForEach(p => _context.Remove(p));

                            _docGenerator.DocNoDelete(RentalRequest.DocumentNo, transaction.GetDbTransaction());

                            await _context.SaveChangesAsync();

                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Only OPEN record can be deleted!");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Container Rental not found!");
                    }

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
