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

namespace FLOG_BE.Features.Finance.GLStatement.PostGLStatementCategory
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
            var exist = _context.GLStatementCategories.Where(x => x.CategoryKey.Equals(request.Body.CategoryKey.ToUpper().Trim())).Any();

            if (exist)
                return ApiResult<Response>.ValidationError("Statement Category already exist !");

            using (var transaction = _context.Database.BeginTransaction())
            {
                var header = new Entities.GLStatementCategory()
                {
                    StatementType = request.Body.StatementType,
                    CategoryKey = request.Body.CategoryKey.ToUpper().Trim(),
                    CategoryCaption = request.Body.CategoryCaption,                     
                };
                
                _context.GLStatementCategories.Add(header);

                await _context.SaveChangesAsync();

                if (header.CategoryId > 0)
                {
                    var subs = await this.InsertSubCategories(header, request.Body);

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        CategoryId = header.CategoryId
                    });
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Statement Category can not be registered !");
                }                
            }
        }

        public async Task<List<Entities.GLStatementSubCategory>> InsertSubCategories(Entities.GLStatementCategory ctg, RequestFormBody body)
        {
            List<Entities.GLStatementSubCategory> subs = new List<Entities.GLStatementSubCategory>();

            if (body.SubCategories.Count > 0)
            {
                foreach(var item in body.SubCategories)
                {
                    var sub = new Entities.GLStatementSubCategory
                    {
                        CategoryId = ctg.CategoryId,
                        SubCategoryKey = item.SubCategoryKey,
                        SubCategoryCaption = item.SubCategoryCaption,
                        IsParamTotal = item.IsParamTotal,
                        Inflow = item.Inflow,
                        PosIndex = item.PosIndex,
                    };

                    subs.Add(sub);
                }

                if(subs.Count > 0)
                {
                    await _context.GLStatementSubCategories.AddRangeAsync(subs);
                }
            }

            return subs;
        }
    }
}
