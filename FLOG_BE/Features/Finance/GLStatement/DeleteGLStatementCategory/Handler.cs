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
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.GLStatement.DeleteGLStatementCategory
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
                try
                {
                    var record = await _context.GLStatementCategories.FirstOrDefaultAsync(x => x.CategoryId == request.Body.CategoryId);

                    if (record != null)
                    {
                        //REMOVE SUB CATEGORY
                        _context.GLStatementSubCategories.Where(x=>x.CategoryId == record.CategoryId).ToList().ForEach(p => _context.Remove(p));

                        //DELETE HEADER
                        _context.Attach(record);
                        _context.Remove(record);

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            CategoryId = request.Body.CategoryId,
                            Message = string.Format("# {0} successfully deleted.", record.CategoryKey)
                        });
                    }
                    else{
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Statement Category can not be found.");
                    }

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
