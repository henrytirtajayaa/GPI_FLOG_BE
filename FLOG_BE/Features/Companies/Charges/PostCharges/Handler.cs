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

namespace FLOG_BE.Features.Companies.Charges.PostCharges
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

            if (await _context.Charges.AnyAsync(x => x.ChargesCode == request.Body.ChargesCode && x.TransactionType == request.Body.TransactionType))
                return ApiResult<Response>.ValidationError(string.Format("{0} with {1} already exist.", request.Body.TransactionType, request.Body.ChargesCode));

            using (var trans = _context.Database.BeginTransaction())
            {
                var charges = new Entities.Charges()
                {
                    ChargesId = Guid.NewGuid(),
                    ChargesCode = request.Body.ChargesCode,
                    ChargeGroupCode = request.Body.ChargeGroupCode,
                    ChargesName = request.Body.ChargesName,
                    TransactionType = request.Body.TransactionType,
                    IsPurchasing = request.Body.IsPurchasing,
                    IsSales = request.Body.IsSales,
                    IsInventory = request.Body.IsInventory,
                    IsFinancial = request.Body.IsFinancial,
                    IsAsset = request.Body.IsAsset,
                    IsDeposit = request.Body.IsDeposit,
                    RevenueAccountNo = request.Body.RevenueAccountNo,
                    TempRevenueAccountNo = request.Body.TempRevenueAccountNo,
                    CostAccountNo = request.Body.CostAccountNo,
                    TaxScheduleCode = request.Body.TaxScheduleCode,
                    ShippingLineType = request.Body.ShippingLineType,
                    HasCosting = request.Body.HasCosting,
                    InActive = request.Body.InActive,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now
                };

                _context.Charges.Add(charges);

                var details = await InsertDetails(request.Body, charges.ChargesId);

                await _context.SaveChangesAsync();

                trans.Commit();

                return ApiResult<Response>.Ok(new Response()
                {
                    ChargesId = charges.ChargesId.ToString(),
                    ChargesCode = charges.ChargesCode,
                    ChargesName = charges.ChargesName
                });
            }                
        }

        private async Task<List<Entities.ChargesDetail>> InsertDetails(RequestChargesBody body, Guid headerId)
        {
            List<Entities.ChargesDetail> entryDetails = new List<Entities.ChargesDetail>();

            if (body.ChargesDetails != null)
            {
                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ChargesDetails)
                {
                    var chargeDetail = new Entities.ChargesDetail()
                    {
                        ChargesDetailId = Guid.NewGuid(),
                        ChargesId = headerId,
                        TrxModule = FLOG.Core.TRX_MODULE.TRX_SHIPPING,
                        ShippingLineId = item.ShippingLineId,
                        RevenueAccountNo = item.RevenueAccountNo,
                        CostAccountNo = item.CostAccountNo,
                        TempRevenueAccountNo = item.TempRevenueAccountNo,
                    };
                    entryDetails.Add(chargeDetail);
                }

                await _context.ChargesDetails.AddRangeAsync(entryDetails.ToArray());

            }

            return entryDetails;

        }

    }
}
