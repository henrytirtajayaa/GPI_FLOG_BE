using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;
using FLOG.Core;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.Payable.GetPayableTransactionById
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var GetPayableTrans = await _context.PayableTransactionHeaders.FirstOrDefaultAsync(x => x.PayableTransactionId == request.PayableTransactionId);
            var GetVendor = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorId == GetPayableTrans.VendorId);
            var GetCreated = await _contextCentral.Persons.FirstOrDefaultAsync(x => x.PersonId == GetPayableTrans.CreatedBy);
            //var GetOriginating = await _context.PayableTransactionDetails.FirstOrDefaultAsync(x => x.PayableTransactionId == request.PayableTransactionId);

            var ReponsePayable = new ReponsePayable()
            {
                PayableTransactionId = GetPayableTrans.PayableTransactionId,
                DocumentNo = GetPayableTrans.DocumentNo,
                BranchCode = GetPayableTrans.BranchCode,
                TransactionDate = GetPayableTrans.TransactionDate,
                CurrencyCode = GetPayableTrans.CurrencyCode,
                CreatedBy = GetPayableTrans.CreatedBy,
                CreatedByName = GetCreated.PersonFullName,
                CreatedDate = GetPayableTrans.CreatedDate,
                Description = GetPayableTrans.Description,
                ModifiedBy = GetPayableTrans.ModifiedBy,
                ModifiedByName = GetPayableTrans.ModifiedByName,
                ModifiedDate = GetPayableTrans.ModifiedDate,
                Status = GetPayableTrans.Status,
                TransactionType = GetPayableTrans.TransactionType,
                VendorId = GetPayableTrans.VendorId,
                VendorName = GetVendor.VendorName,
                DiscountAmount = GetPayableTrans.DiscountAmount,
                NsDocumentNo = GetPayableTrans.NsDocumentNo,
                OriginatingExtendedAmount = GetPayableTrans.SubtotalAmount,
                SubtotalAmount = (GetPayableTrans.SubtotalAmount - GetPayableTrans.DiscountAmount + GetPayableTrans.TaxAmount),
                TaxAmount = GetPayableTrans.TaxAmount,
            };

            var GetCompanySetup = await _context.CompanySetups.FirstOrDefaultAsync();

            var ResponseCompanySetup = new ResponseCompanySetup();

            if (GetCompanySetup != null)
            {
                ResponseCompanySetup.CompanySetupId = GetCompanySetup.CompanySetupId;
                ResponseCompanySetup.CompanyAddressId = GetCompanySetup.CompanyAddressId;
                ResponseCompanySetup.CompanyId = Guid.Parse(GetCompanySetup.CompanyId);
                ResponseCompanySetup.CompanyName = GetCompanySetup.CompanyName;
                ResponseCompanySetup.LogoImageUrl = Helper.ImageBlob.ToImageUrl(GetCompanySetup.LogoImageData);
            };

            var GetCompanyAddress = await _context.CompanyAddresses.FirstOrDefaultAsync(x => x.CompanyAddressId == GetCompanySetup.CompanyAddressId);

            //if (GetCompanyAddress == null)
            //    GetCompanyAddress = new CompanyAddress();

            var ResponseCompanyAddress = new ResponseCompanyAddress()
            {
                CompanyAddressId = GetCompanyAddress.CompanyAddressId,
                AddressCode = GetCompanyAddress.AddressCode,
                AddressName = GetCompanyAddress.AddressName,
                Address = GetCompanyAddress.Address,
                Fax = GetCompanyAddress.Fax,
                Phone1 = GetCompanyAddress.Phone1,
                Phone2 = GetCompanyAddress.Phone2
            };

            var GetPayableDetail = await _context.PayableTransactionDetails.Where(x => x.PayableTransactionId == request.PayableTransactionId).ToListAsync();
            var ResponsePayableDetail = new List<PayableTransactionDetail>();
            foreach (var item in GetPayableDetail)
            {
                var GetCharges = await _context.Charges.FirstOrDefaultAsync(x => x.ChargesId == item.ChargesId);
                var ResPayableDetail = new PayableTransactionDetail()
                {
                    ChargesId = item.ChargesId,
                    ChargesName = GetCharges.ChargesName,
                    TransactionDetailId = item.TransactionDetailId,
                    OriginatingExtendedAmount = item.OriginatingExtendedAmount
                };
                ResponsePayableDetail.Add(ResPayableDetail);
            }

            var GetCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == GetPayableTrans.CurrencyCode);
            var ResponseCurrency = new ResponseCurrency()
            {
                CurrencyId = GetCurrency.CurrencyId,
                CurrencyCode = GetPayableTrans.CurrencyCode,
                CurrencyUnit = GetCurrency.CurrencyUnit,
                CurrencySubUnit = GetCurrency.CurrencySubUnit
            };

            return ApiResult<Response>.Ok(new Response()
            {
                PayableTransaction = ReponsePayable,
                CompanySetup = ResponseCompanySetup,
                CompanyAddress = ResponseCompanyAddress,
                PayableTransactionDetails = ResponsePayableDetail,
                Currency = ResponseCurrency
            });
        }
    }
}
