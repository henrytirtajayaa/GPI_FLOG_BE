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

namespace FLOG_BE.Features.Finance.DepositSettlement.GetDepositSettlementById
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

            //var GetCustomerReceipt = await _context.ArReceiptHeaders.FirstOrDefaultAsync(x => x.SettlementHeaderId == request.SettlementHeaderId);
            var GetCreated = _contextCentral.Persons.ToList();

            var ResponseDeposit = (from cr in _context.DepositSettlementHeaders
                                   join cust in _context.Customers on cr.CustomerId equals cust.CustomerId
                                   where cr.SettlementHeaderId == request.SettlementHeaderId
                                   select new ResponseDeposit
                                   {
                                       SettlementHeaderId = cr.SettlementHeaderId,
                                       DocumentNo = cr.DocumentNo,
                                       DepositNo = cr.DepositNo,
                                       TransactionDate = cr.TransactionDate,
                                       CurrencyCode = cr.CurrencyCode,
                                       CreatedBy = cr.CreatedBy,
                                       CreatedByName = GetCreated.Where(x => x.PersonId == cr.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                                       CreatedDate = cr.CreatedDate,
                                       Description = cr.Description,
                                       FunctionalTotalPaid = cr.FunctionalTotalPaid,
                                       ModifiedBy = cr.ModifiedBy,
                                       ModifiedByName = cr.ModifiedByName,
                                       ModifiedDate = cr.ModifiedDate,
                                       OriginatingTotalPaid = cr.OriginatingTotalPaid,
                                       Status = cr.Status,
                                       DocumentType = cr.DocumentType,
                                       CustomerCode = cr.CurrencyCode,
                                       CustomerId = cr.CustomerId,
                                       CustomerName = cust.CustomerName
                                   }).FirstOrDefault();

            var GetCompanySetup = await _context.CompanySetups.FirstOrDefaultAsync();

            if (GetCompanySetup == null)
                GetCompanySetup = new CompanySetup();

            var ResponseCompanySetup = new ResponseCompanySetup()
            {
                CompanySetupId = GetCompanySetup.CompanySetupId,
                CompanyAddressId = GetCompanySetup.CompanyAddressId,
                CompanyId = Guid.Parse(GetCompanySetup.CompanyId),
                CompanyName = GetCompanySetup.CompanyName,
                LogoImageUrl = Helper.ImageBlob.ToImageUrl(GetCompanySetup.LogoImageData),
            };

            var GetCompanyAddress = await _context.CompanyAddresses.FirstOrDefaultAsync(x => x.CompanyAddressId == GetCompanySetup.CompanyAddressId);

            if (GetCompanyAddress == null)
                GetCompanyAddress = new CompanyAddress();

            var ResponseCompanyAddress = new ResponseCompanyAddress()
            {
                CompanyAddressId = GetCompanyAddress.CompanyAddressId,
                AddressCode = GetCompanyAddress.AddressCode,
                AddressName = GetCompanyAddress.AddressName,
                Fax = GetCompanyAddress.Fax,
                Phone1 = GetCompanyAddress.Phone1,
                Phone2 = GetCompanyAddress.Phone2
            };

            var GetReceiptDetail = await _context.DepositSettlementDetails.Where(x => x.SettlementHeaderId == request.SettlementHeaderId).ToListAsync();

            var ResponseDepositDetail = new List<DepositSettlementDetail>();
            foreach (var item in GetReceiptDetail)
            {
                var GetDocNo = await _context.ReceivableTransactionHeaders.FirstOrDefaultAsync(x => x.ReceiveTransactionId == item.ReceiveTransactionId);
                var ResReceiptDetail = new DepositSettlementDetail()
                {
                    SettlementDetailId = item.SettlementDetailId,
                    ReceiveTransactionId = item.ReceiveTransactionId,
                    DocumentNo = GetDocNo.DocumentNo,
                    OriginatingPaid = item.OriginatingPaid
                };
                ResponseDepositDetail.Add(ResReceiptDetail);
            }

            return ApiResult<Response>.Ok(new Response()
            {
                DepositSettlement = ResponseDeposit,
                CompanySetup = ResponseCompanySetup,
                CompanyAddress = ResponseCompanyAddress,
                DepositSettlementDetails = ResponseDepositDetail
            });
        }
    }
}
