using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG_BE.Helper;

namespace FLOG_BE.Features.Companies.AccountSegment.PostAccountSegment
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _contextCentral;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection, FlogContext contextCentral)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _contextCentral = contextCentral;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var cek = await Helper.Unit.Validate(_contextCentral, _context, request);
            if(cek.HttpStatusCode != 200)
            {
                return ApiResult<Response>.ValidationError(cek.ErrorDescription);
            }

            if (await _context.Accounts.AnyAsync())
            {
                return ApiResult<Response>.ValidationError("Account Segments update not allowed. Accounts already exist");
            }

            //IF ANY THEN UPDATE
            if(await _context.AccountSegments.AnyAsync())
            {
                //REMOVE EXISTING
                _context.AccountSegments.ToList().ForEach(p => _context.Remove(p));
            }

            foreach (var item in request.Body)
            {
                var AccountSegment = new Entities.AccountSegment()
                {
                    SegmentId = Guid.NewGuid().ToString(),
                    SegmentNo = item.SegmentNo,
                    Description = item.Description,
                    IsMainAccount = item.IsMainAccount,
                    Length = item.Length,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now,
                };
                _context.AccountSegments.Add(AccountSegment);
            }

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response());
        }
    }
}
