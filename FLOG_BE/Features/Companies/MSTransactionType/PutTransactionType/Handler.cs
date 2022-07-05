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

namespace FLOG_BE.Features.Companies.MSTransactionType.PutTransactionType
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
                TransactionType = request.Body.TransactionType,
                TransactionName = request.Body.TransactionName,
                PaymentCondition = request.Body.PaymentCondition,
                RequiredSubject = request.Body.RequiredSubject,
                InActive = request.Body.InActive,
            };

            var Type = await _context.MSTransactionTypes.FirstOrDefaultAsync(x => x.TransactionTypeId == Guid.Parse(request.Body.TransactionTypeId));
            if (Type != null)
            {
                Type.TransactionType = request.Body.TransactionType;
                Type.TransactionName = request.Body.TransactionName;
                Type.PaymentCondition = request.Body.PaymentCondition;
                Type.RequiredSubject = request.Body.RequiredSubject;
                Type.InActive = request.Body.InActive;

                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(response);

        }
    }
}
