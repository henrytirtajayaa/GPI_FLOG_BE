using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Companies;


namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.DeleteTaxRefferenceNumber
{
    public class Handler : IAsyncRequestHandler<Request,Response>
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
            var record = _context.TaxRefferenceNumbers.FirstOrDefault(x => x.TaxRefferenceId == Guid.Parse(request.Body.TaxRefferenceId));

            if ((request.Body.StartDate).Date <= DateTime.Now && record.LastNo > 0)
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.StartDate)} before Today cannot be deleted!");

            _context.Attach(record);
            _context.Remove(record);
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                TaxRefferenceId = request.Body.TaxRefferenceId
            });
        }
    }
}
