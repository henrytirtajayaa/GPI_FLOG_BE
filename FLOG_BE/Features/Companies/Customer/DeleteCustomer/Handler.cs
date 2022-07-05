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

namespace FLOG_BE.Features.Companies.Customer.DeleteCustomer
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
            var record = _context.Customers.FirstOrDefault(x => x.CustomerId == request.Body.CustomerId);
            if(record != null)
            {
                var sales = _context.SalesQuotationHeaders.FirstOrDefault(x => x.CustomerId == record.CustomerId);
                var salesOrder = _context.SalesOrderHeaders.FirstOrDefault(x => x.CustomerId == record.CustomerId);
                var checkbook = _context.CheckbookTransactionHeaders.FirstOrDefault(x => x.SubjectCode == record.CustomerCode);
                var receivable = _context.ReceivableTransactionHeaders.FirstOrDefault(x => x.CustomerId == record.CustomerId);
                var receipt = _context.ArReceiptHeaders.FirstOrDefault(x => x.CustomerId == record.CustomerId);

                if (sales != null || salesOrder != null || checkbook != null || receivable != null || receipt != null)
                {
                    return ApiResult<Response>.ValidationError("Customer Already In Use");
                }
                else
                {
                    _context.Attach(record);
                    _context.Remove(record);
                    int isDeleted = await _context.SaveChangesAsync();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        CustomerId = request.Body.CustomerId,
                        Deleted = isDeleted
                    });
                }
            }
            else
            {
                return ApiResult<Response>.ValidationError("Customer not found.");
            }
        }
    }
}
