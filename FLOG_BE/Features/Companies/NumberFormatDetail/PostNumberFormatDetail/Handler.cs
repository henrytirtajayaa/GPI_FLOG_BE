using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.PostNumberFormatDetail
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
            var numberformatdetail = new Entities.NumberFormatDetail()
            {
                FormatDetailId = Guid.NewGuid(),
                FormatHeaderId = Guid.Parse(request.Body.FormatHeaderId),
                SegmentNo = request.Body.SegmentNo,
                SegmentType = request.Body.SegmentType,
                SegmentLength = request.Body.SegmentLength,
                MaskFormat = request.Body.MaskFormat,
                StartingValue = request.Body.StartingValue,
                EndingValue = request.Body.EndingValue,
                Increase = request.Body.Increase
            };

            _context.NumberFormatDetails.Add(numberformatdetail);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                FormatDetailId = numberformatdetail.FormatDetailId.ToString(),
                FormatHeaderId = numberformatdetail.FormatHeaderId.ToString(),
                SegmentNo = numberformatdetail.SegmentNo,
                SegmentType = numberformatdetail.SegmentType,
                SegmentLength = numberformatdetail.SegmentLength,
                MaskFormat = numberformatdetail.MaskFormat,
                StartingValue = numberformatdetail.StartingValue,
                EndingValue = numberformatdetail.EndingValue,
                Increase = numberformatdetail.Increase
            });
        }
    }
}
