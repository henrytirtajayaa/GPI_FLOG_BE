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
using FLOG.Core;
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Finance.GLStatement.PostGLStatementDetail
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                var header = new Entities.GLStatementDetail()
                {
                    SubCategoryId = request.Body.SubCategoryId,
                    AccountName =request.Body.AccountName,
                    PosIndex = request.Body.PosIndex,
                    IsCashflow = request.Body.IsCashflow,
                    IsCashflowDynamic = request.Body.IsCashflowDynamic,
                    ShowZeroValue = request.Body.ShowZeroValue                
                };
                
                _context.GLStatementDetails.Add(header);

                await _context.SaveChangesAsync();

                if(header.DetailId > 0)
                {
                    if(request.Body.AccountIds.Count > 0)
                    {
                        List<Entities.GLStatementDetailSub> subs = new List<Entities.GLStatementDetailSub>();
                        foreach(string accountId in request.Body.AccountIds)
                        {
                            Entities.GLStatementDetailSub sub = new Entities.GLStatementDetailSub();
                            sub.DetailId = header.DetailId;
                            sub.AccountId = accountId;

                            subs.Add(sub);
                        }

                        await _context.GLStatementDetailSubs.AddRangeAsync(subs);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        DetailId = header.DetailId
                    });
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Statement Detail can not be processed !");
                }
            }
        }
    }
}
