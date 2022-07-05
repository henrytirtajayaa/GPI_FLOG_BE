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

namespace FLOG_BE.Features.Finance.GLClosing.PostGLClosingMonth
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string funcCurrencyCode = _context.FinancialSetups.Select(f => f.FuncCurrencyCode).FirstOrDefault();

                if (!string.IsNullOrEmpty(funcCurrencyCode))
                {
                    var closing = await _financeManager.CloseMonth(funcCurrencyCode, request.Body.PeriodYear, request.Body.PeriodIndex, request.Initiator.UserId);

                    if (closing.Valid)
                    {
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            Message = string.Format("Close Period Of {0}/{1} success !", request.Body.PeriodYear, request.Body.PeriodIndex)
                        });
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError(closing.ErrorMessage);
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Functional Currency Code not valid !");
                }
                       
            }
        }

    }
}
