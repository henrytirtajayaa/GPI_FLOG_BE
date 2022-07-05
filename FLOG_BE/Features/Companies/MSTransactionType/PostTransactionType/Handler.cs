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

namespace FLOG_BE.Features.Companies.MSTransactionType.PostTransactionType
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
            if (await _context.MSTransactionTypes.AnyAsync(x => x.TransactionType.Equals(request.Body.TransactionType, StringComparison.OrdinalIgnoreCase) && !x.InActive))
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.TransactionType)} already exist.");

            var Type = new Entities.MSTransactionType()
            {
                TransactionTypeId = Guid.NewGuid(),
                TransactionType = request.Body.TransactionType,
                TransactionName = request.Body.TransactionName,
                PaymentCondition = request.Body.PaymentCondition,
                RequiredSubject = request.Body.RequiredSubject,
                InActive = false
            };

            _context.MSTransactionTypes.Add(Type);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response() 
            {
                TransactionTypeId = Type.TransactionTypeId,
                TransactionType = Type.TransactionType,
                TransactionName = Type.TransactionName,
                PaymentCondition = Type.PaymentCondition,
                RequiredSubject = Type.RequiredSubject,
                InActive = Type.InActive
            });
        }
    }
}
