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
using FLOG.Core;

namespace FLOG_BE.Features.Finance.DepositSettlement.DeleteDepositSettlement
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
                SettlementHeaderId = request.Body.SettlementHeaderId
            };

            using (var deposit = _context.Database.BeginTransaction())
            {
                try
                {
                    var Deposit = await _context.DepositSettlementHeaders.FirstOrDefaultAsync(x => x.SettlementHeaderId == request.Body.SettlementHeaderId);
                    if (Deposit != null)
                    {
                        if (Deposit.Status == DOCSTATUS.NEW)
                        {
                            _context.DepositSettlementDetails.Where(x => x.SettlementHeaderId == request.Body.SettlementHeaderId).ToList().ForEach(p => _context.Remove(p));
                            _context.DepositSettlementHeaders.Where(x => x.SettlementHeaderId == request.Body.SettlementHeaderId).ToList().ForEach(p => _context.Remove(p));
                            await _context.SaveChangesAsync();
                            deposit.Commit();
                        }
                        else
                        {
                            deposit.Rollback();
                            return ApiResult<Response>.ValidationError("Deposit Settlement can not be deleted.");

                        }
                    }
                    else
                    {
                        deposit.Rollback();
                        return ApiResult<Response>.ValidationError("Deposit Settlement not found!");

                    }

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception)
                {
                    deposit.Rollback();
                    throw;
                }
            }
        }
    }
}
