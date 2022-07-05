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

namespace FLOG_BE.Features.Finance.ArReceipt.GetCustomerReceiptById
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

            //var GetCustomerReceipt = await _context.ArReceiptHeaders.FirstOrDefaultAsync(x => x.ReceiptHeaderId == request.ReceiptHeaderId);
            var GetCreated = _contextCentral.Persons.ToList();

            var ResponseReceipt = (from cr in _context.ArReceiptHeaders
                                      join cust in _context.Customers on cr.CustomerId equals cust.CustomerId
                                      where cr.ReceiptHeaderId == request.ReceiptHeaderId
                                      select new ResponseReceipt
                                      {
                                          ReceiptHeaderId = cr.ReceiptHeaderId,
                                          DocumentNo = cr.DocumentNo,
                                          TransactionDate = cr.TransactionDate,
                                          CurrencyCode = cr.CurrencyCode,
                                          CreatedBy = cr.CreatedBy,
                                          CreatedByName = GetCreated.Where(x=>x.PersonId == cr.CreatedBy).Select(p=>p.PersonFullName).FirstOrDefault(),
                                          CreatedDate = cr.CreatedDate,
                                          Description = cr.Description,
                                          FunctionalTotalPaid = cr.FunctionalTotalPaid,
                                          ModifiedBy = cr.ModifiedBy,
                                          ModifiedByName = cr.ModifiedByName,
                                          ModifiedDate = cr.ModifiedDate,
                                          OriginatingTotalPaid = cr.OriginatingTotalPaid,
                                          AppliedTotalPaid = (from v in _context.ArReceiptDetails
                                                              join a in _context.ArReceiptHeaders on v.ReceiptHeaderId equals a.ReceiptHeaderId
                                                              where a.ReceiptHeaderId == cr.ReceiptHeaderId
                                                              select v).Sum(v => v.OriginatingPaid),
                                          Status = cr.Status,
                                          TransactionType = cr.TransactionType,
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
                Address = GetCompanyAddress.Address,
                Fax = GetCompanyAddress.Fax,
                Phone1 = GetCompanyAddress.Phone1,
                Phone2 = GetCompanyAddress.Phone2
            };

            var GetReceiptDetail = await _context.ArReceiptDetails.Where(x => x.ReceiptHeaderId == request.ReceiptHeaderId).ToListAsync();

            var ResponseReceiptDetail = new List<ReceiptDetail>();
            foreach (var item in GetReceiptDetail)
            {
                var GetDocNo = await _context.ReceivableTransactionHeaders.FirstOrDefaultAsync(x => x.ReceiveTransactionId == item.ReceiveTransactionId); 
                var ResReceiptDetail = new ReceiptDetail()
                {
                   ReceiveTransactionId = item.ReceiveTransactionId,
                   DocumentNo = GetDocNo.DocumentNo,
                   OriginatingPaid = item.OriginatingPaid,
                   NsDocumentNo = item.NsDocumentNo,
                   MasterNo = item.MasterNo,
                   AgreementNo = item.AgreementNo
                };
                ResponseReceiptDetail.Add(ResReceiptDetail);
            }

            var GetCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == ResponseReceipt.CurrencyCode);
            var ResponseCurrency = new ResponseCurrency()
            {
                CurrencyId = GetCurrency.CurrencyId,
                CurrencyCode = ResponseReceipt.CurrencyCode,
                CurrencyUnit = GetCurrency.CurrencyUnit,
                CurrencySubUnit = GetCurrency.CurrencySubUnit
            };

            return ApiResult<Response>.Ok(new Response()
            {
                CustomerReceipt = ResponseReceipt,
                CompanySetup = ResponseCompanySetup,
                CompanyAddress = ResponseCompanyAddress,
                ReceiptDetails = ResponseReceiptDetail,
                Currency = ResponseCurrency
            });
        }
    }
}
