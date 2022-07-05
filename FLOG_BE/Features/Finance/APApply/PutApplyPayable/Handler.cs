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

namespace FLOG_BE.Features.Finance.APApply.PutApplyPayable
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
                PayableApplyId = request.Body.PayableApplyId
            };
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    decimal exchangeRate = 1;
                    bool isMultiply = true;

                    if (request.Body.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
                    {
                        var cn = _context.PayableTransactionHeaders.Where(x => x.PayableTransactionId == request.Body.PayableTransactionId).FirstOrDefault();
                        if (cn != null)
                        {
                            exchangeRate = cn.ExchangeRate;
                            isMultiply = cn.IsMultiply;
                        }
                    }
                    else if (request.Body.DocumentType.Equals(DOCUMENTTYPE.PAYMENT, StringComparison.OrdinalIgnoreCase))
                    {
                        var pv = _context.ApPaymentHeaders.Where(x => x.PaymentHeaderId == request.Body.PaymentHeaderId).FirstOrDefault();
                        if (pv != null)
                        {
                            exchangeRate = pv.ExchangeRate;
                            isMultiply = pv.IsMultiply;
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

                    var header = await _context.APApplyHeaders.FirstOrDefaultAsync(x => x.PayableApplyId == request.Body.PayableApplyId);
                    if (header != null)
                    {
                        header.TransactionDate = request.Body.TransactionDate;
                        header.DocumentType = request.Body.DocumentType;
                        header.VendorId = request.Body.VendorId;
                        header.CheckbookTransactionId = request.Body.CheckbookTransactionId;
                        header.PayableTransactionId = request.Body.PayableTransactionId;
                        header.PaymentHeaderId = request.Body.PaymentHeaderId;
                        header.Description = request.Body.Description;
                        header.OriginatingTotalPaid = request.Body.OriginatingTotalPaid;
                        header.FunctionalTotalPaid = CALC.FunctionalAmount(isMultiply, request.Body.OriginatingTotalPaid, exchangeRate);
                        header.ModifiedBy = request.Initiator.UserId;
                        header.ModifiedDate = DateTime.Now;

                        _context.APApplyHeaders.Update(header);

                        var details = await InsertDetails(request.Body, header, exchangeRate, isMultiply);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        var resp = await _financeManager.CreateDistributionJournalAsync(header, details);

                        if (resp.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                            return ApiResult<Response>.Ok(new Response()
                            {
                                Message = "Apply Payable successfully updated."
                            });
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Apply Payable Journal can not be processed.");
                        }
                    }
                    return ApiResult<Response>.Ok(response);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    
                    return ApiResult<Response>.ValidationError("Update failed ! " + ex.Message);
                }
            }
        }
        private async Task<List<Entities.APApplyDetail>> InsertDetails(RequestPayment body, APApplyHeader header, decimal exchangeRate, bool isMultiply)
        {
            List<Entities.APApplyDetail> result = new List<Entities.APApplyDetail>();

            if (body.APApplyDetails != null)
            {
                //REMOVE EXISTING
               _context.APApplyDetails
               .Where(x => x.PayableApplyId == body.PayableApplyId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.APApplyDetails)
                {
                    var detail = new Entities.APApplyDetail()
                    {
                        PayableApplyDetailId = Guid.NewGuid(),
                        PayableApplyId = header.PayableApplyId,
                        PayableTransactionId = item.PayableTransactionId,
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
                    await _context.APApplyDetails.AddRangeAsync(result);
                }
            }

            return result;
        }

      
    }
}
