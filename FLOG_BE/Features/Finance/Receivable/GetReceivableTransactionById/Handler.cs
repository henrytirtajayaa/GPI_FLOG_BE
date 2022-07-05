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

namespace FLOG_BE.Features.Finance.Receivable.GetReceivableTransactionById
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
            Console.WriteLine("[GetReceivableTransactionById] HELLO");
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            try
            {
                var GetReceiveTrans = await _context.ReceivableTransactionHeaders.FirstOrDefaultAsync(x => x.ReceiveTransactionId == request.ReceiveTransactionId);
                var GetCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == GetReceiveTrans.CustomerId);
                var GetAddressBill = await _context.CustomerAddresses.FirstOrDefaultAsync(x => x.AddressCode == GetReceiveTrans.BillToAddressCode);
                //var GetCityBill = await _context.Cities.FirstOrDefaultAsync(x => x.CityCode == GetAddressBill.CityCode);

                var ResponseReceive = new ReponseReceive()
                {
                    ReceiveTransactionId = GetReceiveTrans.ReceiveTransactionId,
                    DocumentNo = GetReceiveTrans.DocumentNo,
                    BranchCode = GetReceiveTrans.BranchCode,
                    DocumentType = GetReceiveTrans.DocumentType,
                    TransactionDate = GetReceiveTrans.TransactionDate,
                    CustomerName = GetCustomer.CustomerName,
                    CurrencyCode = GetReceiveTrans.CurrencyCode,
                    NsDocumentNo = GetReceiveTrans.NsDocumentNo,
                    SoDocumentNo = GetReceiveTrans.SoDocumentNo,
                    AddressBillToAddress = (GetAddressBill != null ? GetAddressBill.Address : ""),
                    //CityBillToAddress = (GetCityBill != null ? GetCityBill.CityName : ""),
                    Description = GetReceiveTrans.Description,
                    OriginatingExtendedAmount = GetReceiveTrans.SubtotalAmount,
                    DiscountAmount = GetReceiveTrans.DiscountAmount,
                    TaxAmount = GetReceiveTrans.TaxAmount,
                    SubtotalAmount = (GetReceiveTrans.SubtotalAmount - GetReceiveTrans.DiscountAmount + GetReceiveTrans.TaxAmount)
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
                }

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

                var GetReceiveTransDetail = await _context.ReceivableTransactionDetails.Where(x => x.ReceiveTransactionId == request.ReceiveTransactionId).ToListAsync();
                var ResponseReceiveDetail = new List<ReceivableTransactionDetail>();
                foreach (var item in GetReceiveTransDetail)
                {
                    var GetCharges = await _context.Charges.FirstOrDefaultAsync(x => x.ChargesId == item.ChargesId);
                    var ResReceiveDetail = new ReceivableTransactionDetail()
                    {
                        ChargesId = item.ChargesId,
                        ChargesName = GetCharges.ChargesName,
                        TransactionDetailId = item.TransactionDetailId,
                        OriginatingExtendedAmount = item.OriginatingExtendedAmount
                    };
                    ResponseReceiveDetail.Add(ResReceiveDetail);
                }

                var GetCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == GetReceiveTrans.CurrencyCode);
                var ResponseCurrency = new ResponseCurrency()
                {
                    CurrencyId = GetCurrency.CurrencyId,
                    CurrencyCode = GetReceiveTrans.CurrencyCode,
                    CurrencyUnit = GetCurrency.CurrencyUnit,
                    CurrencySubUnit = GetCurrency.CurrencySubUnit
                };

                return ApiResult<Response>.Ok(new Response()
                {
                    ReceiveTransaction = ResponseReceive,
                    CompanySetup = ResponseCompanySetup,
                    CompanyAddress = ResponseCompanyAddress,
                    ReceiveTransactionDetail = ResponseReceiveDetail,
                    Currency = ResponseCurrency
                });
            }
            catch (Exception ex)
            {
                return ApiResult<Response>.ValidationError(ex.Message);
            }
        }
    }
}
