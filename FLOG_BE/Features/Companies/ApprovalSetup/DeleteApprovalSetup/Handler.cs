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

namespace FLOG_BE.Features.Companies.ApprovalSetup.DeleteApprovalSetup
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
            var record = _context.ApprovalSetupHeaders
                .FirstOrDefault(x => x.ApprovalSetupHeaderId == request.Body.ApprovalSetupHeaderId);

            if(record != null)
            {
                //IF EXIST IN CHECKBOOK
                var hasCheckbookApproval = await _context.Checkbooks.Where(x => x.ApprovalCode.Equals(record.ApprovalCode, StringComparison.OrdinalIgnoreCase)).AnyAsync();

                if (hasCheckbookApproval)
                {
                    return ApiResult<Response>.ValidationError(string.Format("Approval Code already used in Checkbook."));
                }

                //IF EXIST IN DOC NO SETUP APPROVAL
                var hasDocNoApprovals = await _context.FNDocNumberSetupApprovals.Where(x => x.ApprovalCode.Equals(record.ApprovalCode, StringComparison.OrdinalIgnoreCase)).AnyAsync();

                if (hasDocNoApprovals)
                {
                    return ApiResult<Response>.ValidationError(string.Format("Approval Code already used in Document No Setup approval."));
                }

                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();


                _context.ApprovalSetupDetails
                 .Where(x => x.ApprovalSetupHeaderId == request.Body.ApprovalSetupHeaderId).ToList().ForEach(p => _context.Remove(p));

                await _context.SaveChangesAsync();


                return ApiResult<Response>.Ok(new Response()
                {
                    ApprovalSetupHeaderId = request.Body.ApprovalSetupHeaderId
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError(string.Format("Approval Code not exist !"));
            }
        }
            
    }
}
