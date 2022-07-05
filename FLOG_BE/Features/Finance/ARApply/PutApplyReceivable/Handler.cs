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
using FLOG.Core;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Finance.ARApply.PutApplyReceivable
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
                ReceivableApplyId = request.Body.ReceivableApplyId
            };
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    decimal exchangeRate = 1;
                    bool isMultiply = true;
                    if (request.Body.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
                    {
                        var cn = _context.ReceivableTransactionHeaders.Where(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId).FirstOrDefault();
                        if (cn != null)
                        {
                            exchangeRate = cn.ExchangeRate;
                            isMultiply = cn.IsMultiply;
                        }
                    }else if (request.Body.DocumentType.Equals(DOCUMENTTYPE.RECEIPT, StringComparison.OrdinalIgnoreCase))
                    {
                        var rv = _context.ArReceiptHeaders.Where(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId).FirstOrDefault();
                        if (rv != null)
                        {
                            exchangeRate = rv.ExchangeRate;
                            isMultiply = rv.IsMultiply;
                        }
                    }
                    else
                    {
                        var cb = _context.CheckbookTransactionHeaders.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId).FirstOrDefault();
                        if (cb != null)
                        {
                            exchangeRate = cb.ExchangeRate;
                            isMultiply = cb.IsMultiply;
                        }
                    }

                    var header = await _context.ARApplyHeaders.FirstOrDefaultAsync(x => x.ReceivableApplyId == request.Body.ReceivableApplyId);
                    if (header != null)
                    {
                        header.TransactionDate = request.Body.TransactionDate;
                        header.DocumentType = request.Body.DocumentType;
                        header.CustomerId = request.Body.CustomerId;
                        header.CheckbookTransactionId = request.Body.CheckbookTransactionId;
                        header.ReceiveTransactionId = request.Body.ReceiveTransactionId;
                        header.ReceiptHeaderId = request.Body.ReceiptHeaderId;
                        header.Description = request.Body.Description;
                        header.OriginatingTotalPaid = request.Body.OriginatingTotalPaid;
                        header.FunctionalTotalPaid = CALC.FunctionalAmount(isMultiply, request.Body.OriginatingTotalPaid, exchangeRate);
                        header.ModifiedBy = request.Initiator.UserId;
                        header.ModifiedDate = DateTime.Now;

                        _context.ARApplyHeaders.Update(header);

                        var details = await InsertDetails(request.Body, header, exchangeRate, isMultiply);

                        await _context.SaveChangesAsync();

                        //CREATE DISTRIBUTION JOURNAL HERE
                        var resp = await _financeManager.CreateDistributionJournalAsync(header, details);

                        if (resp.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                            return ApiResult<Response>.Ok(new Response()
                            {
                                Message = "Apply Receivable successfully updated."
                            });
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Apply Receivable Journal can not be processed.");
                        }

                    }
                    return ApiResult<Response>.Ok(response);

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        private async Task<List<Entities.ARApplyDetail>> InsertDetails(RequestPayment body, ARApplyHeader header, decimal exchangeRate, bool isMultiply)
        {
            List<Entities.ARApplyDetail> result = new List<Entities.ARApplyDetail>();

            if (body.ARApplyDetails != null)
            {
                //REMOVE EXISTING
               _context.ARApplyDetails
               .Where(x => x.ReceivableApplyId == body.ReceivableApplyId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ARApplyDetails)
                {
                    var detail = new Entities.ARApplyDetail()
                    {
                        ReceivableApplyDetailId = Guid.NewGuid(),
                        ReceivableApplyId = header.ReceivableApplyId,
                        ReceiveTransactionId = item.ReceiveTransactionId,
                        Description = item.Description,
                        OriginatingBalance = item.OriginatingBalance,
                        FunctionalBalance = CALC.FunctionalAmount(isMultiply, item.OriginatingBalance, exchangeRate),
                        OriginatingPaid = item.OriginatingPaid,
                        FunctionalPaid = CALC.FunctionalAmount(isMultiply, item.OriginatingPaid, exchangeRate),
                        Status = item.Status,
                    };

                    result.Add(detail);
                }
                
                if(result.Count > 0)
                {
                    await _context.ARApplyDetails.AddRangeAsync(result);
                }
            }

            return result;
        }

      
    }
}
