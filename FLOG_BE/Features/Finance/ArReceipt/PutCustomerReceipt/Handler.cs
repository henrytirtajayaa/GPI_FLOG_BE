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
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Finance.ArReceipt.PutCustomerReceipt
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
            var response = new Response()
            {
                ReceiptHeaderId = request.Body.ReceiptHeaderId,
                TransactionType = request.Body.TransactionType,
                DocumentNo = request.Body.DocumentNo
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var receiptHeader = await _context.ArReceiptHeaders.FirstOrDefaultAsync(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId);
                    if (receiptHeader != null)
                    {
                        receiptHeader.TransactionDate = request.Body.TransactionDate;
                        receiptHeader.TransactionType  = request.Body.TransactionType;
                        receiptHeader.CurrencyCode  = request.Body.CurrencyCode;
                        receiptHeader.ExchangeRate  = request.Body.ExchangeRate;
                        receiptHeader.CheckbookCode  = request.Body.CheckbookCode;
                        receiptHeader.CustomerId  = request.Body.CustomerId;
                        receiptHeader.Description  = request.Body.Description;
                        receiptHeader.OriginatingTotalPaid  = request.Body.OriginatingTotalPaid;
                        receiptHeader.FunctionalTotalPaid  = request.Body.FunctionalTotalPaid;
                        receiptHeader.ModifiedBy  = request.Initiator.UserId;
                        receiptHeader.ModifiedDate  = DateTime.Now;

                        var receivableDetails = await InsertArReceiptDetails(_context, request.Body);

                        JournalResponse jResponse = await _financeManager.CreateDistributionJournalAsync(receiptHeader, receivableDetails);
                        
                        if (jResponse.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(response);
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(jResponse.ErrorMessage);
                        }
                    }
                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError($"AR Receipt error : " + e.Message);
                }
            }
        }

        private async Task<List<Entities.ArReceiptDetail>> InsertArReceiptDetails(CompanyContext ctx, RequestCustomerReceiptBody body)
        {
            List<Entities.ArReceiptDetail> result = new List<Entities.ArReceiptDetail>();

            if (body.ArReceiptDetails != null)
            {
                //REMOVE EXISTING
                ctx.ArReceiptDetails
               .Where(x => x.ReceiptHeaderId == body.ReceiptHeaderId).ToList().ForEach(p => ctx.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ArReceiptDetails)
                {
                    var receiptDetail = new Entities.ArReceiptDetail()
                    {
                        ReceiptDetailId = Guid.NewGuid(),
                        ReceiptHeaderId = body.ReceiptHeaderId,
                        ReceiveTransactionId = item.ReceiveTransactionId,
                        NsDocumentNo = item.NsDocumentNo,
                        MasterNo = item.MasterNo,
                        AgreementNo = item.AgreementNo,
                        Description = item.Description,
                        OriginatingBalance = item.OriginatingBalance,
                        FunctionalBalance = item.FunctionalBalance,
                        OriginatingPaid = item.OriginatingPaid,
                        FunctionalPaid = item.FunctionalPaid,
                        Status = FLOG.Core.DOCSTATUS.NEW,
                    };

                    result.Add(receiptDetail);
                }

                await _context.ArReceiptDetails.AddRangeAsync(result);

                await ctx.SaveChangesAsync();                    
            }

            return result;
        }
    }
}
