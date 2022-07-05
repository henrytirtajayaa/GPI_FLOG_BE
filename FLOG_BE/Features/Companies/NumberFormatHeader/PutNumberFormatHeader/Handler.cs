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

namespace FLOG_BE.Features.Companies.NumberFormatHeader.PutNumberFormatHeader
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
                DocumentId = request.Body.DocumentId,
                Description = request.Body.Description,
                LastGeneratedNo = request.Body.LastGeneratedNo,
                NumberFormat = request.Body.NumberFormat,
                InActive = request.Body.InActive,
                IsMonthlyReset = request.Body.IsMonthlyReset,
                IsYearlyReset = request.Body.IsYearlyReset
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var numberFormat = await _context.NumberFormatHeaders.FirstOrDefaultAsync(x => x.FormatHeaderId == Guid.Parse(request.Body.FormatHeaderId));
                if (numberFormat != null)
                {
                    numberFormat.DocumentId = request.Body.DocumentId;
                    numberFormat.Description = request.Body.Description;
                    numberFormat.LastGeneratedNo = request.Body.LastGeneratedNo;
                    numberFormat.NumberFormat = request.Body.NumberFormat;
                    numberFormat.InActive = request.Body.InActive;
                    numberFormat.IsMonthlyReset = request.Body.IsMonthlyReset;
                    numberFormat.IsYearlyReset = request.Body.IsYearlyReset;

                    numberFormat.ModifiedBy = request.Initiator.UserId;
                    numberFormat.ModifiedDate = DateTime.Now;

                    _context.NumberFormatHeaders.Update(numberFormat);

                    await _context.SaveChangesAsync();

                    response.DocumentId = numberFormat.DocumentId;

                    //UPDATE DETAIL
                    bool valid = await InsertNumberFormatDetails(_context, request.Body);

                    if (valid)
                    {
                        transaction.Commit();
                        return ApiResult<Response>.Ok(response);
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Fiscal period detail cant be updated.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Fiscal period not found.");
                }
            }
        }

        private async Task<bool> InsertNumberFormatDetails(CompanyContext ctx, RequestBodyUpdateNumberFormatHeader body)
        {
            if (body.NumberFormatDetails != null)
            {
                //REMOVE EXISTING
                var formatDetails = await ctx.NumberFormatDetails.Where(x => x.FormatHeaderId == Guid.Parse(body.FormatHeaderId)).ToListAsync();
                ctx.NumberFormatDetails.RemoveRange(formatDetails);

                await ctx.SaveChangesAsync();

                //INSERT NEW ROWS
                foreach (var fDetailVM in body.NumberFormatDetails)
                {
                    var numberformatdetail = new Model.Companies.Entities.NumberFormatDetail()
                    {
                        FormatDetailId = Guid.NewGuid(),
                        FormatHeaderId = Guid.Parse(body.FormatHeaderId),
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

                await ctx.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
