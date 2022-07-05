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

namespace FLOG_BE.Features.Companies.VendorAddress.PostVendorAddress
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
            if (await _context.VendorAddresses.AnyAsync(x => x.AddressCode == request.Body.AddressCode && x.VendorId == request.Body.VendorId))
            {
                return ApiResult<Response>.ValidationError("Vendor Address Code for this Vendor already exist");
            }

            var vendoraddress = new Entities.VendorAddress()
            {
                VendorAddressId = Guid.NewGuid(),
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
                Default = request.Body.Default,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.VendorAddresses.Add(vendoraddress);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                VendorId = vendoraddress.VendorId,
                VendorAddressId = vendoraddress.VendorAddressId.ToString(),
                AddressCode = vendoraddress.AddressCode,
                AddressName = vendoraddress.AddressName,
                ContactPerson = vendoraddress.ContactPerson,
                Address = vendoraddress.Address,
                Handphone = vendoraddress.Handphone,
                Phone1 = vendoraddress.Phone1,
                Extension1 = vendoraddress.Extension1,
                Phone2 = vendoraddress.Phone2,
                Extension2 = vendoraddress.Extension2,
                Fax = vendoraddress.Fax,
                EmailAddress = vendoraddress.EmailAddress,
                HomePage = vendoraddress.HomePage,
                Neighbourhood = vendoraddress.Neighbourhood,
                Hamlet = vendoraddress.Hamlet,
                UrbanVillage = vendoraddress.UrbanVillage,
                SubDistrict = vendoraddress.SubDistrict,
                CityCode = vendoraddress.CityCode,
                PostCode = vendoraddress.PostCode,
                IsSameAddress = vendoraddress.IsSameAddress,
                TaxAddressId = vendoraddress.TaxAddressId,
                Default = vendoraddress.Default
            }) ;
        }
    }
}
