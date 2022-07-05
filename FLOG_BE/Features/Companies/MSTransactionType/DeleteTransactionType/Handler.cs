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

namespace FLOG_BE.Features.Companies.MSTransactionType.DeleteTransactionType
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
            var record = _context.MSTransactionTypes.FirstOrDefault(x => x.TransactionTypeId == request.Body.TransactionTypeId);
            
            if(record != null)
            {
                var charges = _context.Charges.Where(x => x.TransactionType == record.TransactionType).Any();
                var financial = _context.FinancialSetups.Where(x => x.CheckbookChargesType == record.TransactionType).Any();
                var salesQuot = _context.SalesQuotationHeaders.Where(x => x.TransactionType == record.TransactionType).Any();
                var salesOrder = _context.SalesOrderHeaders.Where(x => x.TransactionType == record.TransactionType).Any();
                var receivable = _context.ReceivableTransactionHeaders.Where(x => x.TransactionType == record.TransactionType).Any();
                var payable = _context.PayableTransactionHeaders.Where(x => x.TransactionType == record.TransactionType).Any();

                if (charges || financial || salesQuot || salesOrder || receivable || payable)
                {
                    return ApiResult<Response>.ValidationError("Transaction Type already In Used !");
                }
                else
                {
                    record.InActive = true;
                }
            }
            
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                TransactionTypeId = request.Body.TransactionTypeId
            });
        }
    }
}
