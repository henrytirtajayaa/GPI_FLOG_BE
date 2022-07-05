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

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.PostFiscalPeriodDetail
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
            foreach (var item in request.Body)
            {
                var cekFiscalHeader = _context.FiscalPeriodHeaders.Where(x => x.FiscalHeaderId == item.FiscalHeaderId).FirstOrDefault();
                if (cekFiscalHeader == null)
                {
                    return ApiResult<Response>.ValidationError($"{nameof(item.FiscalHeaderId)} not found!");
                }

                var fiscal = await _context.FiscalPeriodDetails.FirstOrDefaultAsync(x => x.FiscalDetailId == item.FiscalDetailId);
                if (fiscal != null)
                {
                    fiscal.FiscalHeaderId = item.FiscalHeaderId;
                    fiscal.PeriodIndex = item.PeriodIndex;
                    fiscal.PeriodStart = item.PeriodStart;
                    fiscal.PeriodEnd = item.PeriodEnd;
                    fiscal.IsClosePurchasing = item.IsClosePurchasing;
                    fiscal.IsCloseSales = item.IsCloseSales;
                    fiscal.IsCloseInventory = item.IsCloseInventory;
                    fiscal.IsCloseFinancial = item.IsCloseFinancial;
                    fiscal.IsCloseAsset = item.IsCloseAsset;
                }
                else
                {
                    var fiscaldetail = new Entities.FiscalPeriodDetail()
                    {
                        FiscalDetailId = Guid.NewGuid(),
                        FiscalHeaderId = item.FiscalHeaderId,
                        PeriodIndex = item.PeriodIndex,
                        PeriodStart = item.PeriodStart,
                        PeriodEnd = item.PeriodEnd,
                        IsClosePurchasing = item.IsClosePurchasing,
                        IsCloseSales = item.IsCloseSales,
                        IsCloseInventory = item.IsCloseInventory,
                        IsCloseFinancial = item.IsCloseFinancial,
                        IsCloseAsset = item.IsCloseAsset
                    };
                    _context.FiscalPeriodDetails.Add(fiscaldetail);
                }
                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(new Response());
        }
    }
}
