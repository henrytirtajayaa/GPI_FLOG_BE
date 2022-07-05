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

namespace FLOG_BE.Features.Companies.Charges.PutCharges
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
                ChargesId = request.Body.ChargesId,
                ChargesCode = request.Body.ChargesCode,
                ChargesName = request.Body.ChargesName
            };

            using (var trans = _context.Database.BeginTransaction())
            {
                var charges = await _context.Charges.FirstOrDefaultAsync(x => x.ChargesId == Guid.Parse(request.Body.ChargesId));
                if (charges != null)
                {
                    charges.ChargesId = Guid.Parse(request.Body.ChargesId);
                    charges.ChargesCode = request.Body.ChargesCode;
                    charges.ChargeGroupCode = request.Body.ChargeGroupCode;
                    charges.ChargesName = request.Body.ChargesName;
                    charges.TransactionType = request.Body.TransactionType;
                    charges.IsPurchasing = request.Body.IsPurchasing;
                    charges.IsSales = request.Body.IsSales;
                    charges.IsInventory = request.Body.IsInventory;
                    charges.IsFinancial = request.Body.IsFinancial;
                    charges.IsAsset = request.Body.IsAsset;
                    charges.IsDeposit = request.Body.IsDeposit;
                    charges.RevenueAccountNo = request.Body.RevenueAccountNo;
                    charges.TempRevenueAccountNo = request.Body.TempRevenueAccountNo;
                    charges.CostAccountNo = request.Body.CostAccountNo;
                    charges.TaxScheduleCode = request.Body.TaxScheduleCode;
                    charges.ShippingLineType = request.Body.ShippingLineType;
                    charges.HasCosting = request.Body.HasCosting;
                    charges.InActive = request.Body.InActive;
                    charges.ModifiedBy = request.Initiator.UserId;
                    charges.ModifiedDate = DateTime.Now;

                    _context.Charges.Update(charges);

                    var details = await InsertDetails(request.Body, charges.ChargesId);

                    await _context.SaveChangesAsync();

                    trans.Commit();

                    response.ChargesId = charges.ChargesId.ToString();
                    response.ChargesName = charges.ChargesName;
                }
                else
                {
                    trans.Rollback();
                    return ApiResult<Response>.ValidationError("Charges Id not found.");
                }
            }

            return ApiResult<Response>.Ok(response);
            
        }

        private async Task<List<Entities.ChargesDetail>> InsertDetails(RequestPutChargeBody body, Guid headerId)
        {
            List<Entities.ChargesDetail> entryDetails = new List<Entities.ChargesDetail>();

            //REMOVE EXISTING
            _context.ChargesDetails
            .Where(x => x.ChargesId == headerId).ToList().ForEach(p => _context.Remove(p));

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
