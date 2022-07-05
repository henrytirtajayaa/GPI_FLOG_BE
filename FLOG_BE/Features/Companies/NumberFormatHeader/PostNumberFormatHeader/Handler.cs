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

namespace FLOG_BE.Features.Companies.NumberFormatHeader.PostNumberFormatHeader
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
            if (await _context.NumberFormatHeaders.AnyAsync(x => x.DocumentId == request.Body.DocumentId))
            {
                return ApiResult<Response>.ValidationError("Number Format Header already exist");
            }
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var numberformatheader = new Entities.NumberFormatHeader()
                    {
                        FormatHeaderId = Guid.NewGuid(),
                        DocumentId = request.Body.DocumentId,
                        Description = request.Body.Description,
                        LastGeneratedNo = request.Body.LastGeneratedNo,
                        NumberFormat = request.Body.NumberFormat,
                        InActive = request.Body.InActive,
                        IsMonthlyReset = request.Body.IsMonthlyReset,
                        IsYearlyReset = request.Body.IsYearlyReset,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now
                    };

                    _context.NumberFormatHeaders.Add(numberformatheader);

                    await _context.SaveChangesAsync();

                    if (numberformatheader.FormatHeaderId != null && numberformatheader.FormatHeaderId != Guid.Empty)
                    {
                        if (request.Body.NumberFormatDetails.Count > 0)
                        {
                            foreach (var fDetailVM in request.Body.NumberFormatDetails)
                            {
                                var numberformatdetail = new Entities.NumberFormatDetail()
                                {
                                    FormatDetailId = Guid.NewGuid(),
                                    FormatHeaderId = numberformatheader.FormatHeaderId,
                                    SegmentNo = fDetailVM.SegmentNo,
                                    SegmentType = fDetailVM.SegmentType,
                                    SegmentLength = fDetailVM.SegmentLength,
                                    MaskFormat = fDetailVM.MaskFormat,
                                    StartingValue = fDetailVM.StartingValue,
                                    EndingValue = fDetailVM.EndingValue,
                                    Increase = fDetailVM.Increase
                                };

                                _context.NumberFormatDetails.Add(numberformatdetail);
                            }

                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                FormatHeaderId = numberformatheader.FormatHeaderId.ToString(),
                                DocumentId = numberformatheader.DocumentId,
                                Description = numberformatheader.Description,
                                LastGeneratedNo = numberformatheader.LastGeneratedNo,
                                NumberFormat = numberformatheader.NumberFormat,
                                InActive = numberformatheader.InActive,
                                IsMonthlyReset = numberformatheader.IsMonthlyReset,
                                IsYearlyReset = numberformatheader.IsYearlyReset
                            });
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError("Number Format Details not exist.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Number Format can not be processed.");
                    }
                }
            }       
        }
    }
}
