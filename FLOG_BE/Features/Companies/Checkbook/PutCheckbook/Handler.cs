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

namespace FLOG_BE.Features.Companies.Checkbook.PutCheckbook
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
                CheckbookCode = request.Body.CheckbookCode,
                CheckbookName = request.Body.CheckbookName,
                CurrencyCode = request.Body.CurrencyCode,
                BankAccountCode = request.Body.BankAccountCode,
                BankCode = request.Body.BankCode,
                CheckbookAccountNo = request.Body.CheckbookAccountNo,
                HasCheckoutApproval = request.Body.HasCheckoutApproval,
                ApprovalCode = request.Body.ApprovalCode,
                CheckbookInDocNo = request.Body.CheckbookInDocNo,
                CheckbookOutDocNo = request.Body.CheckbookOutDocNo,
                ReceiptDocNo = request.Body.ReceiptDocNo,
                PaymentDocNo = request.Body.PaymentDocNo,
                ReconcileDocNo = request.Body.ReconcileDocNo,
                IsCash = request.Body.IsCash,
                InActive = request.Body.InActive
            };

            var checkbook = await _context.Checkbooks.FirstOrDefaultAsync(x => x.CheckbookId == Guid.Parse(request.Body.CheckbookId));
            if (checkbook != null)
            {
                checkbook.CheckbookCode = request.Body.CheckbookCode;
                checkbook.CheckbookName = request.Body.CheckbookName;
                checkbook.CurrencyCode = request.Body.CurrencyCode;
                checkbook.BankAccountCode = request.Body.BankAccountCode;
                checkbook.BankCode = request.Body.BankCode;
                checkbook.CheckbookAccountNo = request.Body.CheckbookAccountNo;
                checkbook.HasCheckoutApproval = request.Body.HasCheckoutApproval;
                checkbook.ApprovalCode = request.Body.ApprovalCode;
                checkbook.CheckbookInDocNo = request.Body.CheckbookInDocNo;
                checkbook.CheckbookOutDocNo = request.Body.CheckbookOutDocNo;
                checkbook.ReceiptDocNo = request.Body.ReceiptDocNo;
                checkbook.PaymentDocNo = request.Body.PaymentDocNo;
                checkbook.ReconcileDocNo = request.Body.ReconcileDocNo;
                checkbook.IsCash = request.Body.IsCash;
                checkbook.InActive = request.Body.InActive;
                checkbook.ModifiedBy = request.Initiator.UserId;
                checkbook.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.CheckbookName = checkbook.CheckbookName;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
