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
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.ApprovalSetup.PutApprovalSetup
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
                ApprovalCode = request.Body.ApprovalCode,
                Description = request.Body.Description,
                Status = request.Body.Status
            };

            var ApprovalSetupHeaders = await _context.ApprovalSetupHeaders.FirstOrDefaultAsync(x => x.ApprovalSetupHeaderId == request.Body.ApprovalSetupHeaderId);
        
            if (ApprovalSetupHeaders != null)
            {

                _context.ApprovalSetupDetails.Where(x => x.ApprovalSetupHeaderId == request.Body.ApprovalSetupHeaderId)
                 .ToList().ForEach(x => _context.ApprovalSetupDetails.Remove(x));

                ApprovalSetupHeaders.ApprovalCode = request.Body.ApprovalCode;
                ApprovalSetupHeaders.Description = request.Body.Description;
                ApprovalSetupHeaders.Status = request.Body.Status;
                ApprovalSetupHeaders.ModifiedBy = request.Initiator.UserId;
                ApprovalSetupHeaders.ModifiedDate = DateTime.Now;

               
            }


            foreach (var item in request.Body.ApprovalSetupDetails)
            {
                var ApprovalSetupDetail = new Entities.ApprovalSetupDetail()
                {
                    ApprovalSetupDetailId = Guid.NewGuid(),
                    ApprovalSetupHeaderId = ApprovalSetupHeaders.ApprovalSetupHeaderId,
                    Description = item.Description,
                    PersonId = item.PersonId,
                    PersonCategoryId = item.PersonCategoryId,
                    Level = item.Level,
                    HasLimit = item.HasLimit,
                    ApprovalLimit = item.ApprovalLimit,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = request.Initiator.UserId,
                    ModifiedDate = DateTime.Now,


                };
                _context.ApprovalSetupDetails.Add(ApprovalSetupDetail);
            }
            
            await _context.SaveChangesAsync();
            response.ApprovalCode = ApprovalSetupHeaders.ApprovalCode;

            return ApiResult<Response>.Ok(response);
        }
    }
}
