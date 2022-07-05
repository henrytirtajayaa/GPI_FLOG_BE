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

namespace FLOG_BE.Features.Companies.Currency.DeleteCurrency
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
            var record = _context.Currencies.FirstOrDefault(x => x.CurrencyId == request.Body.CurrencyId);
            if (record != null)
            {
                var financial = _context.FinancialSetups.FirstOrDefault(x => x.FuncCurrencyCode == record.CurrencyCode);
                var sales = _context.SalesOrderBuyings.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);
                var receivable = _context.ReceivableTransactionHeaders.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);
                var payable = _context.PayableTransactionHeaders.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);
                var payment = _context.ApPaymentHeaders.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);
                var receipt = _context.ArReceiptHeaders.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);
                var checkbook = _context.CheckbookTransactionHeaders.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);
                var checkbookMaster = _context.Checkbooks.FirstOrDefault(x => x.CurrencyCode == record.CurrencyCode);

                if (financial != null || sales != null || receivable != null || payable != null || payment != null || receipt !=null || checkbook != null || checkbookMaster != null)
                {
                    return ApiResult<Response>.ValidationError("Currency Already In Use");
                }
                else
                {
                    _context.Attach(record);
                    _context.Remove(record);
                    await _context.SaveChangesAsync();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        CurrencyId = request.Body.CurrencyId
                    });
                }
            }
            else
            {
                return ApiResult<Response>.ValidationError("Currency not found.");
            }
        }
    }
}
