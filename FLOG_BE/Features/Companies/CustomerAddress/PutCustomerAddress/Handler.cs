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

namespace FLOG_BE.Features.Companies.CustomerAddress.PutCustomerAddress
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

            var customeraddress = await _context.CustomerAddresses.FirstOrDefaultAsync(x => x.CustomerAddressId == Guid.Parse(request.Body.CustomerAddressId));
            if (customeraddress != null)
            {
                customeraddress.AddressCode = request.Body.AddressCode;
                customeraddress.AddressName = request.Body.AddressName;
                customeraddress.ContactPerson = request.Body.ContactPerson;
                customeraddress.Address = request.Body.Address;
                customeraddress.Handphone = request.Body.Handphone;
                customeraddress.Phone1 = request.Body.Phone1;
                customeraddress.Extension1 = request.Body.Extension1;
                customeraddress.Phone2 = request.Body.Phone2;
                customeraddress.Extension2 = request.Body.Extension2;
                customeraddress.Fax = request.Body.Fax;
                customeraddress.EmailAddress = request.Body.EmailAddress;
                customeraddress.HomePage = request.Body.HomePage;
                customeraddress.Neighbourhood = request.Body.Neighbourhood;
                customeraddress.Hamlet = request.Body.Hamlet;
                customeraddress.UrbanVillage = request.Body.UrbanVillage;
                customeraddress.SubDistrict = request.Body.SubDistrict;
                customeraddress.CityCode = request.Body.CityCode;
                customeraddress.PostCode = request.Body.PostCode;
                customeraddress.IsSameAddress = request.Body.IsSameAddress;
                customeraddress.TaxAddressId = request.Body.TaxAddressId;
                customeraddress.Default = request.Body.Default;
                customeraddress.ModifiedBy = request.Initiator.UserId;
                customeraddress.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.AddressName = customeraddress.AddressName;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
