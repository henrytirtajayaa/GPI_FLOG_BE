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

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.PutTaxRefferenceNumber
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                StartDate = request.Body.StartDate,
                ReffNoStart = request.Body.ReffNoStart,
                ReffNoEnd = request.Body.ReffNoEnd,
                DocLength = request.Body.DocLength,
                LastNo = request.Body.LastNo,
                Status = request.Body.Status,
            };

            var taxrefference = await _context.TaxRefferenceNumbers.FirstOrDefaultAsync(x => x.TaxRefferenceId == Guid.Parse(request.Body.TaxRefferenceId));
            if (taxrefference != null)
            {
                taxrefference.StartDate = request.Body.StartDate;
                taxrefference.ReffNoStart = request.Body.ReffNoStart;
                taxrefference.ReffNoEnd = request.Body.ReffNoEnd;
                taxrefference.DocLength = request.Body.DocLength;
                taxrefference.LastNo = request.Body.LastNo;
                taxrefference.Status = request.Body.Status;
                taxrefference.ModifiedBy = request.Initiator.UserId;
                taxrefference.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.StartDate = taxrefference.StartDate;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
