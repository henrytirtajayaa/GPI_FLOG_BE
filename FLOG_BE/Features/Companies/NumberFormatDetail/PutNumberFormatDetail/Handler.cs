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


namespace FLOG_BE.Features.Companies.NumberFormatDetail.PutNumberFormatDetail
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
                FormatHeaderId = request.Body.FormatHeaderId,
                SegmentNo = request.Body.SegmentNo,
                SegmentType = request.Body.SegmentType,
                SegmentLength = request.Body.SegmentLength,
                MaskFormat = request.Body.MaskFormat,
                StartingValue = request.Body.StartingValue,
                EndingValue = request.Body.EndingValue,
                Increase = request.Body.Increase
            };

            var numberformatdetail = await _context.NumberFormatDetails.FirstOrDefaultAsync(x => x.FormatDetailId == Guid.Parse(request.Body.FormatDetailId));
            if (numberformatdetail != null)
            {
                numberformatdetail.FormatHeaderId = Guid.Parse(request.Body.FormatHeaderId);
                numberformatdetail.SegmentNo = request.Body.SegmentNo;
                numberformatdetail.SegmentType = request.Body.SegmentType;
                numberformatdetail.SegmentLength = request.Body.SegmentLength;
                numberformatdetail.MaskFormat = request.Body.MaskFormat;
                numberformatdetail.StartingValue = request.Body.StartingValue;
                numberformatdetail.EndingValue = request.Body.EndingValue;
                numberformatdetail.Increase = request.Body.Increase;

                await _context.SaveChangesAsync();

                response.FormatHeaderId = numberformatdetail.FormatHeaderId.ToString();
            }

            return ApiResult<Response>.Ok(response);

        }
    }
}
