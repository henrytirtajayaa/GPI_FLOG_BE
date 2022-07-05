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

namespace FLOG_BE.Features.Finance.Checkbook.GetCheckbookTransactionById
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

            var GetCheckbookTrans = await _context.CheckbookTransactionHeaders.FirstOrDefaultAsync(x => x.CheckbookTransactionId == request.CheckbookTransactionId);
            var GetCreated = await _contextCentral.Persons.FirstOrDefaultAsync(x => x.PersonId == GetCheckbookTrans.CreatedBy);
            var GetCheckbook = await _context.Checkbooks.FirstOrDefaultAsync(c => c.CheckbookCode == GetCheckbookTrans.CheckbookCode);
            String SubjectName;

            if(GetCheckbookTrans.DocumentType == "IN")
            {
                var GetCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerCode == GetCheckbookTrans.SubjectCode);
                SubjectName = (GetCustomer != null ? GetCustomer.CustomerName : "");
            }
            else
            {
                var GetVendor = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorCode == GetCheckbookTrans.SubjectCode);
                SubjectName = (GetVendor != null ? GetVendor.VendorName : "");
            }

            var ReponseCheckbook = new ReponseCheckbook()
            {
                CheckbookTransactionId = GetCheckbookTrans.CheckbookTransactionId,
                DocumentNo = GetCheckbookTrans.DocumentNo,
                BranchCode = GetCheckbookTrans.BranchCode,
                TransactionDate = GetCheckbookTrans.TransactionDate,
                CurrencyCode = GetCheckbookTrans.CurrencyCode,
                CreatedBy = GetCheckbookTrans.CreatedBy,
                CreatedByName = GetCreated.PersonFullName,
                CreatedDate = GetCheckbookTrans.CreatedDate,
                Description = GetCheckbookTrans.Description,
                ModifiedBy = GetCheckbookTrans.ModifiedBy,
                ModifiedDate = GetCheckbookTrans.ModifiedDate,
                TransactionType = GetCheckbookTrans.TransactionType,
                CheckbookCode = GetCheckbookTrans.CheckbookCode,
                BankAccountCode = GetCheckbook.BankAccountCode,
                DocumentType = GetCheckbookTrans.DocumentType,
                BankCode = GetCheckbook.BankCode,
                BankName = GetCheckbook.BankName,
                CheckbookName = GetCheckbook.CheckbookName,
                SubjectCode = GetCheckbookTrans.SubjectCode,
                SubjectName = SubjectName,
                OriginatingTotalAmount = GetCheckbookTrans.OriginatingTotalAmount
            };

            var GetCompanySetup = await _context.CompanySetups.FirstOrDefaultAsync();
            var ResponseCompanySetup = new ResponseCompanySetup()
            {
                CompanySetupId = GetCompanySetup.CompanySetupId,
                CompanyAddressId = GetCompanySetup.CompanyAddressId,
                CompanyId = Guid.Parse(GetCompanySetup.CompanyId),
                CompanyName = GetCompanySetup.CompanyName,
                LogoImageUrl = Helper.ImageBlob.ToImageUrl(GetCompanySetup.LogoImageData)
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

            var GetCheckbookDetail = await _context.CheckbookTransactionDetails.Where(x => x.CheckbookTransactionId == request.CheckbookTransactionId).ToListAsync();
            var ResponseCheckbookDetail = new List<CheckbookDetail>();
            foreach (var item in GetCheckbookDetail)
            {
                var GetCharges = await _context.Charges.FirstOrDefaultAsync(x => x.ChargesId == item.ChargesId);
                var ResCheckbookDetail = new CheckbookDetail()
                {
                    ChargesName = GetCharges.ChargesName,
                    TransactionDetailId = item.TransactionDetailId,
                    OriginatingAmount = item.OriginatingAmount
                };
                ResponseCheckbookDetail.Add(ResCheckbookDetail);
            };

            var GetCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == GetCheckbookTrans.CurrencyCode);
            var ResponseCurrency = new ResponseCurrency()
            {
                CurrencyId = GetCurrency.CurrencyId,
                CurrencyCode = GetCheckbookTrans.CurrencyCode,
                CurrencyUnit = GetCurrency.CurrencyUnit,
                CurrencySubUnit = GetCurrency.CurrencySubUnit
            };

            return ApiResult<Response>.Ok(new Response()
            {
                CheckbookTransaction = ReponseCheckbook,
                CompanySetup = ResponseCompanySetup,
                CompanyAddress = ResponseCompanyAddress,
                CheckbookDetails = ResponseCheckbookDetail,
                Currency = ResponseCurrency
            });
        }
    }
}
