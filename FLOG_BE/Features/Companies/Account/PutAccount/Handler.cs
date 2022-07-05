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

namespace FLOG_BE.Features.Companies.Account.PutAccount
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
                AccountId = request.Body.AccountId,
                Description = request.Body.Description
            };

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId == request.Body.AccountId);
            if (account != null)
            {
                account.AccountId = request.Body.AccountId;
                account.Segment1 = request.Body.Segment1;
                account.Segment2 = request.Body.Segment2;
                account.Segment3 = request.Body.Segment3;
                account.Segment4 = request.Body.Segment4;
                account.Segment5 = request.Body.Segment5;
                account.Segment6 = request.Body.Segment6;
                account.Segment7 = request.Body.Segment7;
                account.Segment8 = request.Body.Segment8;
                account.Segment9 = request.Body.Segment9;
                account.Segment10 = request.Body.Segment10;
                account.Description = request.Body.Description;
                account.PostingType = request.Body.PostingType;
                account.NormalBalance = request.Body.NormalBalance;
                account.NoDirectEntry = request.Body.NoDirectEntry;
                account.Revaluation = request.Body.NoDirectEntry;
                account.Inactive = request.Body.Inactive;
                account.ModifiedBy = request.Initiator.UserId;
                account.ModifiedDate = DateTime.Now;

                _context.Accounts.Update(account);

                await _context.SaveChangesAsync();
                response.AccountId = account.AccountId;
                response.Description = account.Description;
            }
            else {
                return ApiResult<Response>.ValidationError("Account not found.");
            }

            return ApiResult<Response>.Ok(response);
            
        }
    }
}
