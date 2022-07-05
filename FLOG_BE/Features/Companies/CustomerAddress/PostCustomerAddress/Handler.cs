using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.CustomerAddress.PostCustomerAddress
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly CompanyContext _context;
        public readonly ILogin _login;
        public readonly HATEOASLinkCollection _linkCollection;


        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (await _context.CustomerAddresses.AnyAsync(x => x.AddressCode == request.Body.AddressCode && x.CustomerId.ToString().Equals(request.Body.CustomerId, StringComparison.OrdinalIgnoreCase)))
            {
                return ApiResult<Response>.ValidationError(string.Format("{0} Address Code already exist !", request.Body.AddressCode));
            }

            var customeraddress = new Entities.CustomerAddress()
            {
                CustomerAddressId = Guid.NewGuid(),
                CustomerId = Guid.Parse(request.Body.CustomerId),
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
                Default = request.Body.Default,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.CustomerAddresses.Add(customeraddress);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            { 
                CompanyAddressId = customeraddress.CustomerAddressId.ToString(),
                CustomerId = customeraddress.CustomerId.ToString(),
                AddressCode = customeraddress.AddressCode,
                AddressName = customeraddress.AddressName,
                ContactPerson = customeraddress.ContactPerson,
                Address = customeraddress.Address,
                Handphone = customeraddress.Handphone,
                Phone1 = customeraddress.Phone1,
                Extension1 = customeraddress.Extension1,
                Phone2 = customeraddress.Phone2,
                Extension2 = customeraddress.Extension2,
                Fax = customeraddress.Fax,
                EmailAddress = customeraddress.EmailAddress,
                HomePage = customeraddress.HomePage,
                Neighbourhood = customeraddress.Neighbourhood,
                Hamlet = customeraddress.Hamlet,
                UrbanVillage = customeraddress.UrbanVillage,
                SubDistrict = customeraddress.SubDistrict,
                CityCode = customeraddress.CityCode,
                PostCode = customeraddress.PostCode,
                IsSameAddress = customeraddress.IsSameAddress,
                TaxAddressId = customeraddress.TaxAddressId,
                Default = customeraddress.Default
            });
        }
    }
}
