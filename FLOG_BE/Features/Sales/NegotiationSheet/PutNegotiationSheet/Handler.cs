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

namespace FLOG_BE.Features.Sales.NegotiationSheet.PutNegotiationSheet
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private Repository _repository;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _repository = new Repository(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                Entities.NegotiationSheetHeader nsHeader = await _repository.CreateOrUpdateHeader(request.Body, request.Initiator);

                if (nsHeader != null)
                {
                    var containers = await _repository.InsertContainers(request.Body);
                    var truckings = await _repository.InsertTruckingDetails(request.Body, nsHeader);
                    var selling = await _repository.InsertSellingDetails(request.Body, nsHeader);

                    nsHeader.TotalFuncSelling = selling.Sum(s => s.FunctionalExtendedAmount);
                    nsHeader.TotalFuncBuying = selling.Sum(s => s.NsBuyings.Sum(buy => buy.FunctionalExtendedAmount));
                    nsHeader.ModifiedBy = request.Initiator.UserId;
                    nsHeader.ModifiedDate = DateTime.Now;

                    //UPDATE HEADER
                    _context.NegotiationSheetHeaders.Update(nsHeader);

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        NegotiationSheetId = nsHeader.NegotiationSheetId,
                        DocumentNo = nsHeader.DocumentNo
                    });
                }
                else
                {
                    transaction.Rollback();

                    return ApiResult<Response>.ValidationError("Negotiation Sheet can not updated !");
                }
            }
        }

    }
}
