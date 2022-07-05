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

namespace FLOG_BE.Features.Finance.ApPayment.PutVendorPayment
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
                PaymentHeaderId = request.Body.PaymentHeaderId
            };
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var paymentHeader = await _context.ApPaymentHeaders.FirstOrDefaultAsync(x => x.PaymentHeaderId == request.Body.PaymentHeaderId);
                    if (paymentHeader != null)
                    {
                        paymentHeader.TransactionDate = request.Body.TransactionDate;
                        paymentHeader.TransactionType = request.Body.TransactionType;
                        paymentHeader.CurrencyCode = request.Body.CurrencyCode;
                        paymentHeader.ExchangeRate = request.Body.ExchangeRate;
                        paymentHeader.VendorId = request.Body.VendorId;
                        paymentHeader.Description = request.Body.Description;
                        paymentHeader.OriginatingTotalPaid = request.Body.OriginatingTotalPaid;
                        paymentHeader.FunctionalTotalPaid = request.Body.FunctionalTotalPaid;
                        //paymentHeader.Status = DOCSTATUS.NEW;
                        //paymentHeader.StatusComment = request.Body.StatusComment;
                        paymentHeader.ModifiedBy = request.Initiator.UserId;
                        paymentHeader.ModifiedDate = DateTime.Now;

                        var details = await InsertPaymentDetails(request.Body, paymentHeader);

                        JournalResponse jResponse = await _financeManager.CreateDistributionJournalAsync(paymentHeader, details);

                        if (jResponse.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                PaymentHeaderId = paymentHeader.PaymentHeaderId,
                                Message = " Vendor Payment successfully updated."
                            });
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
                    return ApiResult<Response>.ValidationError($"Vendor Payment error : " + e.Message);
                }
            }
        }
        private async Task<List<Entities.ApPaymentDetail>> InsertPaymentDetails(RequestPayment body, ApPaymentHeader header)
        {
            List<Entities.ApPaymentDetail> result = new List<Entities.ApPaymentDetail>();

            if (body.ApPaymentDetails != null)
            {
                //REMOVE EXISTING
               _context.ApPaymentDetails
               .Where(x => x.PaymentHeaderId == body.PaymentHeaderId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ApPaymentDetails)
                {
                    var payableDetail = new Entities.ApPaymentDetail()
                    {
                        PaymentDetailId = Guid.NewGuid(),
                        PaymentHeaderId = header.PaymentHeaderId,
                        PayableTransactionId = item.PayableTransactionId,
                        NsDocumentNo = item.NsDocumentNo,
                        MasterNo = item.MasterNo,
                        AgreementNo = item.AgreementNo,
                        Description = item.Description,
                        OriginatingBalance = item.OriginatingBalance,
                        FunctionalBalance = item.FunctionalBalance,
                        OriginatingPaid = item.OriginatingPaid,
                        FunctionalPaid = item.FunctionalPaid,
                        Status = item.Status,
                    };

                    result.Add(payableDetail);
                }
                
                if(result.Count > 0)
                {
                    await _context.ApPaymentDetails.AddRangeAsync(result);
                }
            }

            return result;
        }

      
    }
}
