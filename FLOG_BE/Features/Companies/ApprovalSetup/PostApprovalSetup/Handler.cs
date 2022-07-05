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


namespace FLOG_BE.Features.Companies.ApprovalSetup.PostApprovalSetup
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

            var Code = _context.ApprovalSetupHeaders
                .Where(x => x.ApprovalCode == request.Body.ApprovalCode)
                .Select(x => x.ApprovalCode).FirstOrDefault();

            if (Code != null)
                return ApiResult<Response>.ValidationError("Approval Code already exist");

            using (var transaction = _context.Database.BeginTransaction())
            {
                var app = new ApprovalSetupHeader()
                {
                    ApprovalSetupHeaderId = Guid.NewGuid(),
                    ApprovalCode = request.Body.ApprovalCode,
                    Description = request.Body.Description,
                    Status = request.Body.Status,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now

                };

                _context.ApprovalSetupHeaders.Add(app);
                await _context.SaveChangesAsync();


                if (app.ApprovalSetupHeaderId != null && app.ApprovalSetupHeaderId != Guid.Empty)
                {
                    if (request.Body.ApprovalSetupDetails.Count > 0)
                    {
                        foreach (var item in request.Body.ApprovalSetupDetails)
                        {

                            var ApprovalSetupDetails = new Entities.ApprovalSetupDetail()
                            {
                                ApprovalSetupDetailId = Guid.NewGuid(),
                                ApprovalSetupHeaderId = app.ApprovalSetupHeaderId,
                                Description = item.Description,
                                PersonId = (item.PersonId != Guid.Empty ? item.PersonId : Guid.Empty),
                                PersonCategoryId = (item.PersonCategoryId != Guid.Empty ? item.PersonCategoryId : Guid.Empty),
                                Level = item.Level,
                                HasLimit = item.HasLimit,
                                ApprovalLimit = item.ApprovalLimit,
                                CreatedBy = request.Initiator.UserId,
                                CreatedDate = DateTime.Now,
                            };
                            _context.ApprovalSetupDetails.Add(ApprovalSetupDetails);
                        }

                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return ApiResult<Response>.Ok(new Response()
                        {
                            ApprovalSetupHeaderId = app.ApprovalSetupHeaderId,
                            ApprovalCode = app.ApprovalCode,
                            Description = app.Description
                        });
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Approval setup not exist.");
                    }
                }
                else
                {

                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Approval setup can not be stored.");
                }
            }
        }
    }
}

