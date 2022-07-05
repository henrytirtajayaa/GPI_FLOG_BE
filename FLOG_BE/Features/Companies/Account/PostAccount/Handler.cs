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

namespace FLOG_BE.Features.Companies.Account.PostAccount
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

            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();
            if (await _context.Accounts.AnyAsync(x => x.AccountId == request.Body.AccountId))
                  return ApiResult<Response>.ValidationError("Account already exist.");
            
            var Account = new Entities.Account()
            {
                AccountId = request.Body.AccountId,
                Segment1 = request.Body.Segment1,
                Segment2 = request.Body.Segment2,
                Segment3 = request.Body.Segment3,
                Segment4 = request.Body.Segment4,
                Segment5 = request.Body.Segment5,
                Segment6 = request.Body.Segment6,
                Segment7 = request.Body.Segment7,
                Segment8 = request.Body.Segment8,
                Segment9 = request.Body.Segment9,
                Segment10 = request.Body.Segment10,
                Description = request.Body.Description,
                PostingType = request.Body.PostingType,
                NormalBalance = request.Body.NormalBalance,
                NoDirectEntry = request.Body.NoDirectEntry,
                Revaluation = request.Body.Revaluation,
                Inactive = request.Body.Inactive,     
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };
                        
            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
               AccountId = Account.AccountId.ToString(),
               Description = Account.Description
            });
        }
    }
}
