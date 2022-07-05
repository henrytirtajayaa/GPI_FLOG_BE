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
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.VendorAddress.PutVendorAddress
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
                VendorId = request.Body.VendorId,
                AddressCode = request.Body.AddressCode,
                AddressName = request.Body.AddressName,
                ContactPerson = request.Body.ContactPerson,
                Address = request.Body.Address,
                Handphone = request.Body.Handphone,
                Phone1 = request.Body.Phone1,
                Extension1 = request.Body.Extension1,
                Phone2 = request.Body.Phone2,
                Extension2 = request.Body.Extension2,
                Fax = request.Body.Fax,
                EmailAddress = request.Body.EmailAddress,
                HomePage = request.Body.HomePage,
                Neighbourhood = request.Body.Neighbourhood,
                Hamlet = request.Body.Hamlet,
                UrbanVillage = request.Body.UrbanVillage,
                SubDistrict = request.Body.SubDistrict,
                CityCode = request.Body.CityCode,
                PostCode = request.Body.PostCode,
                IsSameAddress = request.Body.IsSameAddress,
                TaxAddressId = request.Body.TaxAddressId,
                Default = request.Body.Default
            };

            var companyaddress = await _context.VendorAddresses.FirstOrDefaultAsync(x => x.VendorAddressId == Guid.Parse(request.Body.VendorAddressId));
            if (companyaddress != null)
            {
                companyaddress.VendorId = request.Body.VendorId;
                companyaddress.AddressCode = request.Body.AddressCode;
                companyaddress.AddressName = request.Body.AddressName;
                companyaddress.ContactPerson = request.Body.ContactPerson;
                companyaddress.Address = request.Body.Address;
                companyaddress.Handphone = request.Body.Handphone;
                companyaddress.Phone1 = request.Body.Phone1;
                companyaddress.Extension1 = request.Body.Extension1;
                companyaddress.Phone2 = request.Body.Phone2;
                companyaddress.Extension2 = request.Body.Extension2;
                companyaddress.Fax = request.Body.Fax;
                companyaddress.EmailAddress = request.Body.EmailAddress;
                companyaddress.HomePage = request.Body.HomePage;
                companyaddress.Neighbourhood = request.Body.Neighbourhood;
                companyaddress.Hamlet = request.Body.Hamlet;
                companyaddress.UrbanVillage = request.Body.UrbanVillage;
                companyaddress.SubDistrict = request.Body.SubDistrict;
                companyaddress.CityCode = request.Body.CityCode;
                companyaddress.PostCode = request.Body.PostCode;
                companyaddress.IsSameAddress = request.Body.IsSameAddress;
                companyaddress.TaxAddressId = request.Body.TaxAddressId;
                companyaddress.Default = request.Body.Default;
                companyaddress.ModifiedBy = request.Initiator.UserId;
                companyaddress.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.AddressName = companyaddress.AddressName;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
