using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.Checkbook.PostCheckbook
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly CompanyContext _context;
        public readonly ILogin _login;
        public readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (await _context.Checkbooks.AnyAsync(x => x.CheckbookCode == request.Body.CheckbookCode))
            {
                return ApiResult<Response>.ValidationError("Checkbook Code already exist");
            }

            var checkbook = new Entities.Checkbook()
            {
                CheckbookId = Guid.NewGuid(),
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
                InActive = request.Body.InActive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Checkbooks.Add(checkbook);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CheckbookId = checkbook.CheckbookId.ToString(),
                CheckbookCode = checkbook.CheckbookCode,
                CheckbookName = checkbook.CheckbookName,
                CurrencyCode = checkbook.CurrencyCode,
                BankAccountCode = checkbook.BankAccountCode,
                BankCode = checkbook.BankCode,
                CheckbookAccountNo = checkbook.CheckbookCode,
                HasCheckoutApproval = checkbook.HasCheckoutApproval,
                ApprovalCode = checkbook.ApprovalCode,
                CheckbookInDocNo = checkbook.CheckbookInDocNo,
                CheckbookOutDocNo = checkbook.CheckbookOutDocNo,
                ReceiptDocNo = checkbook.ReceiptDocNo,
                PaymentDocNo = checkbook.PaymentDocNo,
                ReconcileDocNo = checkbook.ReconcileDocNo,
                IsCash = checkbook.IsCash,
                InActive = checkbook.InActive
            });
        }
    }
}
