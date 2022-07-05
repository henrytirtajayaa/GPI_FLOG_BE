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

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PostFiscalPeriodHeader
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                var nCount = await _context.FiscalPeriodHeaders.Where(x=>x.PeriodYear== request.Body.PeriodYear).CountAsync();

                if(nCount <= 0)
                {
                    var fiscalperiod = new Entities.FiscalPeriodHeader()
                    {
                        FiscalHeaderId = Guid.NewGuid(),
                        PeriodYear = request.Body.PeriodYear,
                        TotalPeriod = request.Body.TotalPeriod,
                        StartDate = request.Body.StartDate,
                        EndDate = request.Body.EndDate,
                        ClosingYear = request.Body.ClosingYear,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now
                    };

                    _context.FiscalPeriodHeaders.Add(fiscalperiod);

                    await _context.SaveChangesAsync();

                    if (fiscalperiod.FiscalHeaderId != null && fiscalperiod.FiscalHeaderId != Guid.Empty)
                    {
                        if (request.Body.FiscalDetails.Count > 0)
                        {
                            foreach (var fDetailVM in request.Body.FiscalDetails)
                            {
                                var fiscaldetail = new Entities.FiscalPeriodDetail()
                                {
                                    FiscalDetailId = Guid.NewGuid(),
                                    FiscalHeaderId = fiscalperiod.FiscalHeaderId,
                                    PeriodIndex = fDetailVM.PeriodIndex,
                                    PeriodStart = fDetailVM.PeriodStart,
                                    PeriodEnd = fDetailVM.PeriodEnd,
                                    IsClosePurchasing = fDetailVM.IsClosePurchasing,
                                    IsCloseSales = fDetailVM.IsCloseSales,
                                    IsCloseInventory = fDetailVM.IsCloseInventory,
                                    IsCloseFinancial = fDetailVM.IsCloseFinancial,
                                    IsCloseAsset = fDetailVM.IsCloseAsset
                                };

                                _context.FiscalPeriodDetails.Add(fiscaldetail);
                            }

                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                FiscalHeaderId = fiscalperiod.FiscalHeaderId,
                                PeriodYear = fiscalperiod.PeriodYear,
                                TotalPeriod = fiscalperiod.TotalPeriod,
                                StartDate = fiscalperiod.StartDate,
                                EndDate = fiscalperiod.EndDate,
                                ClosingYear = fiscalperiod.ClosingYear,
                            });
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError("Fiscal Detail not exist.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Fiscal Period can not be stored.");
                    }
                }
                else
                {
                    return ApiResult<Response>.ValidationError("Period Year already Exist.");
                }                  
            }                
        }
    }
}
