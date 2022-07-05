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
using Entities = FLOG_BE.Model.Companies.Entities;
using System.IO;

namespace FLOG_BE.Features.Companies.CompanySetup.PostCompanySetup
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
            if (await _context.CompanySetups.AnyAsync(x => x.CompanyId == request.Body.CompanyId && x.CompanyAddressId == Guid.Parse(request.Body.CompanyAddressId)))
                return ApiResult<Response>.ValidationError("Company Setup Address already exist.");

            var response = new Response()
            {
                CompanyId = request.Body.CompanyId
            };

            var companySetup = new Entities.CompanySetup()
            {
                CompanySetupId = Guid.NewGuid(),
                CompanyId = request.Body.CompanyId,
                CompanyName = request.Body.CompanyName,
                TaxRegistrationNo = request.Body.TaxRegistrationNo,
                CompanyTaxName = request.Body.CompanyTaxName,
                CompanyAddressId = Guid.Parse(request.Body.CompanyAddressId),
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            if (request.Body.LogoImage != null && request.Body.LogoImage.Length > 0)
            {
                IFormFile logoFile = request.Body.LogoImage;

                companySetup.LogoImageTitle = logoFile.FileName;
                companySetup.LogoImageType = logoFile.ContentType;

                MemoryStream ms = new MemoryStream();
                logoFile.CopyTo(ms);
                companySetup.LogoImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();
            }

            _context.CompanySetups.Add(companySetup);

            await _context.SaveChangesAsync();

            string imageBase64Data = string.Empty;
            string imageDataURL = string.Empty;

            if (companySetup.LogoImageData != null && companySetup.LogoImageData.Length > 0)
            {
                imageBase64Data = Convert.ToBase64String(companySetup.LogoImageData);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }

            return ApiResult<Response>.Ok(new Response()
            {
                CompanySetupId = companySetup.CompanySetupId.ToString(),
                CompanyId = companySetup.CompanyId,
                CompanyName = companySetup.CompanyName,
                CompanyAddressId = companySetup.CompanyAddressId.ToString(),
                TaxRegistrationNo = companySetup.TaxRegistrationNo,
                LogoImageUrl = imageDataURL,
            });
        }
    }
}
