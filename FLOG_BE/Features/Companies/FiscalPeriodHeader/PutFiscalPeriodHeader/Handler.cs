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

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PutFiscalPeriodHeader
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
                FiscalHeaderId = request.Body.FiscalHeaderId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var fiscal = await _context.FiscalPeriodHeaders.FirstOrDefaultAsync(x => x.FiscalHeaderId == request.Body.FiscalHeaderId);
                if (fiscal != null)
                {
                    fiscal.PeriodYear = request.Body.PeriodYear;
                    fiscal.StartDate = request.Body.StartDate;
                    fiscal.EndDate = request.Body.EndDate;
                    fiscal.ClosingYear = request.Body.ClosingYear;
                    fiscal.TotalPeriod = request.Body.TotalPeriod;

                    fiscal.ModifiedBy = request.Initiator.UserId;
                    fiscal.ModifiedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    response.FiscalHeaderId = fiscal.FiscalHeaderId;
                    response.PeriodYear = fiscal.PeriodYear;

                    //UPDATE DETAIL
                    bool valid = await UpdateFiscalDetail(_context, request.Body);

                    if (valid)
                    {
                        transaction.Commit();
                        return ApiResult<Response>.Ok(response);                                            
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Fiscal period detail cant be updated.");
                    }                    
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Fiscal period not found.");
                }
            }

        }

        private async Task<bool> UpdateFiscalDetail(CompanyContext ctx, RequestPutFiscalBody body)
        {
            if(body.FiscalDetails != null)
            {
                var fiscalDetails = await ctx.FiscalPeriodDetails.Where(x => x.FiscalHeaderId == body.FiscalHeaderId).ToListAsync();
                if(fiscalDetails.Count() > 0)
                {
                    foreach(var fdvm in body.FiscalDetails)
                    {
                        var fiscalExisting = fiscalDetails.Where(o => o.FiscalDetailId == fdvm.FiscalDetailId).FirstOrDefault();
                        if (fiscalExisting != null)
                        {
                            fiscalExisting.PeriodIndex = fdvm.PeriodIndex;
                            fiscalExisting.PeriodStart = fdvm.PeriodStart;
                            fiscalExisting.PeriodEnd = fdvm.PeriodEnd;
                            fiscalExisting.IsClosePurchasing = fdvm.IsClosePurchasing;
                            fiscalExisting.IsCloseSales = fdvm.IsCloseSales;
                            fiscalExisting.IsCloseInventory = fdvm.IsCloseInventory;
                            fiscalExisting.IsCloseFinancial = fdvm.IsCloseFinancial;
                            fiscalExisting.IsCloseAsset = fdvm.IsCloseAsset;
                        }                        
                    }

                    await ctx.SaveChangesAsync();
                }

                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
