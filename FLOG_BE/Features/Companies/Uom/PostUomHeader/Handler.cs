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

namespace FLOG_BE.Features.Companies.Uom.PostUomHeader
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
            if (await _context.UOMHeaders.AnyAsync(x => x.UomScheduleCode.Trim().ToUpper() == request.Body.UomScheduleCode.Trim().ToUpper() && !x.Inactive))
                  return ApiResult<Response>.ValidationError("Uom Schedule Code already exist.");

                using (var trx = _context.Database.BeginTransaction())
                {
                    var header = new Entities.UOMHeader()
                    {
                        UomHeaderId = Guid.NewGuid(),
                        UomScheduleCode = request.Body.UomScheduleCode.Trim().ToUpper(),
                        UomScheduleName = request.Body.UomScheduleName,
                        UomBaseId = request.Body.UomBaseId,
                        ModifiedBy = request.Initiator.UserId,
                        ModifiedDate = DateTime.Now,
                        Inactive = false,
                    };

                    _context.UOMHeaders.Add(header);

                    List<Entities.UOMDetail> listDetails = new List<Entities.UOMDetail>();
                    foreach(var det in request.Body.UomDetails)
                    {
                        var uomDetail = new Entities.UOMDetail()
                        {
                            UomDetailId = Guid.NewGuid(),
                            UomHeaderId = header.UomHeaderId,
                            UomCode = det.UomCode,
                            UomName = det.UomName,
                            EquivalentQty = det.EquivalentQty
                        };
                        listDetails.Add(uomDetail);
                    }

                    if(listDetails.Count > 0)
                    {
                        await _context.UOMDetails.AddRangeAsync(listDetails);

                        await _context.SaveChangesAsync();

                        trx.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            UomHeaderId = header.UomHeaderId,
                            UomScheduleCode = header.UomScheduleCode,
                            UomScheduleName = header.UomScheduleName
                        });
                    }
                    else
                    {
                        trx.Rollback();

                        return ApiResult<Response>.ValidationError("UOM Schedule details can not empty !");
                    }
                }
                
            }
                
        }
    
}
