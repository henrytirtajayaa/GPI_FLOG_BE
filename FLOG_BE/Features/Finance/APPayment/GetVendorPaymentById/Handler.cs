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

namespace FLOG_BE.Features.Finance.ApPayment.GetVendorPaymentById
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

            var GetVendorPayment = await _context.ApPaymentHeaders.FirstOrDefaultAsync(x => x.PaymentHeaderId == request.PaymentHeaderId);
            var GetVendor = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorId == GetVendorPayment.VendorId);
            var GetCreated = await _contextCentral.Persons.FirstOrDefaultAsync(x => x.PersonId == GetVendorPayment.CreatedBy);
            var ReponsePayment = new ReponsePayment()
            {
                PaymentHeaderId = GetVendorPayment.PaymentHeaderId,
                DocumentNo = GetVendorPayment.DocumentNo,
                TransactionDate = GetVendorPayment.TransactionDate,
                CurrencyCode = GetVendorPayment.CurrencyCode,
                CreatedBy = GetVendorPayment.CreatedBy,
                CreatedByName = GetCreated.PersonFullName,
                CreatedDate = GetVendorPayment.CreatedDate,
                Description = GetVendorPayment.Description,
                FunctionalTotalPaid = GetVendorPayment.FunctionalTotalPaid,
                ModifiedBy = GetVendorPayment.ModifiedBy,
                ModifiedByName = GetVendorPayment.ModifiedByName,
                ModifiedDate = GetVendorPayment.ModifiedDate,
                OriginatingTotalPaid = GetVendorPayment.OriginatingTotalPaid,
                AppliedTotalPaid = (from v in _context.ApPaymentDetails
                                    join a in _context.ApPaymentHeaders on v.PaymentHeaderId equals a.PaymentHeaderId
                                    where a.PaymentHeaderId == GetVendorPayment.PaymentHeaderId
                                    select v).Sum(v => v.OriginatingPaid),
                Status = GetVendorPayment.Status,
                TransactionType = GetVendorPayment.TransactionType,
                VendorCode = GetVendorPayment.VendorCode,
                VendorId = GetVendorPayment.VendorId,
                VendorName = GetVendor.VendorName
            };

            var GetCompanySetup = await _context.CompanySetups.FirstOrDefaultAsync();
            var ResponseCompanySetup = new ResponseCompanySetup()
            {
                CompanySetupId = GetCompanySetup.CompanySetupId,
                CompanyAddressId = GetCompanySetup.CompanyAddressId,
                CompanyId = Guid.Parse(GetCompanySetup.CompanyId),
                CompanyName = GetCompanySetup.CompanyName,
                LogoImageUrl = Helper.ImageBlob.ToImageUrl(GetCompanySetup.LogoImageData),
            };

            var GetCompanyAddress = await _context.CompanyAddresses.FirstOrDefaultAsync(x => x.CompanyAddressId == GetCompanySetup.CompanyAddressId);
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

            var GetPaymentDetail = await _context.ApPaymentDetails.Where(x => x.PaymentHeaderId == request.PaymentHeaderId).ToListAsync();
            var ResponsePaymentDetail = new List<PaymentDetail>();
            foreach (var item in GetPaymentDetail)
            {
                var GetDocNo = await _context.PayableTransactionHeaders.FirstOrDefaultAsync(x => x.PayableTransactionId == item.PayableTransactionId); 
                var ResPaymentDetail = new PaymentDetail()
                {
                   PayableTransactionId = item.PayableTransactionId,
                   DocumentNo = GetDocNo.DocumentNo,
                   OriginatingPaid = item.OriginatingPaid,
                   NsDocumentNo = item.NsDocumentNo,
                   MasterNo = item.MasterNo,
                   AgreementNo = item.AgreementNo
                };
                ResponsePaymentDetail.Add(ResPaymentDetail);
            }

            var GetCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == GetVendorPayment.CurrencyCode);
            var ResponseCurrency = new ResponseCurrency()
            {
                CurrencyId = GetCurrency.CurrencyId,
                CurrencyCode = GetVendorPayment.CurrencyCode,
                CurrencyUnit = GetCurrency.CurrencyUnit,
                CurrencySubUnit = GetCurrency.CurrencySubUnit
            };

            return ApiResult<Response>.Ok(new Response()
            {
                VendorPayment = ReponsePayment,
                CompanySetup = ResponseCompanySetup,
                CompanyAddress = ResponseCompanyAddress,
                ApPaymentDetails = ResponsePaymentDetail,
                Currency = ResponseCurrency
            });
        }
    }
}
