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
using System.IO;

namespace FLOG_BE.Features.Companies.CompanySetup.PutCompanySetup
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
            try
            {
                var companySetup = await _context.CompanySetups.FirstOrDefaultAsync(x => x.CompanySetupId == Guid.Parse(request.Body.CompanySetupId));
                if (companySetup != null)
                {
                    companySetup.CompanyId = request.Body.CompanyId;
                    companySetup.CompanyName = request.Body.CompanyName;
                    companySetup.TaxRegistrationNo = request.Body.TaxRegistrationNo;
                    companySetup.CompanyTaxName = request.Body.CompanyTaxName;
                    companySetup.CompanyAddressId = Guid.Parse(request.Body.CompanyAddressId);
                    companySetup.CompanyLogo = request.Body.CompanyLogo;

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

                    companySetup.ModifiedBy = request.Initiator.UserId;
                    companySetup.ModifiedDate = DateTime.Now;

                    _context.CompanySetups.Update(companySetup);

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
                        TaxRegistrationNo = companySetup.TaxRegistrationNo,
                        CompanyTaxName = companySetup.CompanyTaxName,
                        CompanyLogo = companySetup.CompanyLogo,
                        LogoImageUrl = imageDataURL,
                    }); 
                }
                else
                {
                    return ApiResult<Response>.ValidationError("Company Setup not found.");
                }
            }catch(Exception ex)
            {
                Console.WriteLine("********* ERROR ******** " + ex.StackTrace);
                return ApiResult<Response>.ValidationError("[ERROR] " + ex.Message);
            }
        }
    }
}
