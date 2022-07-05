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

namespace FLOG_BE.Features.Companies.CompanyAddress.PostCompanyAddress
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
            if (await _context.CompanyAddresses.AnyAsync(x => x.AddressCode == request.Body.AddressCode))
            {
                return ApiResult<Response>.ValidationError("Company Address Code already exist");
            }
            var Listcompanyaddress = await _context.CompanyAddresses.ToListAsync();

            if (request.Body.Default)
            {
                //update default to false
                foreach (var item in Listcompanyaddress)
                {
                    item.Default = false;
                    _context.CompanyAddresses.Update(item);
                }
            }

            var companyaddress = new Entities.CompanyAddress()
            {
                CompanyAddressId = Guid.NewGuid(),
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

            _context.CompanyAddresses.Add(companyaddress);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            { 
                CompanyAddressId = companyaddress.CompanyAddressId.ToString(),
                AddressCode = companyaddress.AddressCode,
                AddressName = companyaddress.AddressName,
                ContactPerson = companyaddress.ContactPerson,
                Address = companyaddress.Address,
                Handphone = companyaddress.Handphone,
                Phone1 = companyaddress.Phone1,
                Extension1 = companyaddress.Extension1,
                Phone2 = companyaddress.Phone2,
                Extension2 = companyaddress.Extension2,
                Fax = companyaddress.Fax,
                EmailAddress = companyaddress.EmailAddress,
                HomePage = companyaddress.HomePage,
                Neighbourhood = companyaddress.Neighbourhood,
                Hamlet = companyaddress.Hamlet,
                UrbanVillage = companyaddress.UrbanVillage,
                SubDistrict = companyaddress.SubDistrict,
                CityCode = companyaddress.CityCode,
                PostCode = companyaddress.PostCode,
                IsSameAddress = companyaddress.IsSameAddress,
                TaxAddressId = companyaddress.TaxAddressId,
                Default = companyaddress.Default
            });
        }
    }
}
