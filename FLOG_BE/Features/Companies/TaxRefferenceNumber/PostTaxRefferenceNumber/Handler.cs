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

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.PostTaxRefferenceNumber
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
            var taxrefference = new Entities.TaxRefferenceNumber()
            {
                TaxRefferenceId = Guid.NewGuid(),
                StartDate = request.Body.StartDate,
                ReffNoStart = request.Body.ReffNoStart,
                ReffNoEnd = request.Body.ReffNoEnd,
                DocLength = request.Body.DocLength,
                LastNo = request.Body.LastNo,
                Status = request.Body.Status,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now,
            };

            _context.TaxRefferenceNumbers.Add(taxrefference);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                TaxRefferenceId = taxrefference.TaxRefferenceId.ToString(),
                StartDate = taxrefference.StartDate,
                ReffNoStart = taxrefference.ReffNoStart,
                ReffNoEnd = taxrefference.ReffNoEnd,
                DocLength = taxrefference.DocLength,
                LastNo = taxrefference.LastNo,
                Status = taxrefference.Status,
            });
        }
    }
}
