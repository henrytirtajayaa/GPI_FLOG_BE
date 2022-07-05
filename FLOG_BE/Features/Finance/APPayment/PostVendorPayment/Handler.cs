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
using FLOG.Core.DocumentNo;
using FLOG.Core.Finance.Util;
using Infrastructure;

namespace FLOG_BE.Features.Finance.ApPayment.PostVendorPayment
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IDocumentGenerator _docGenerator;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(_context);
            _financeManager = new FinanceManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByCheckbook(request.Body.TransactionDate, request.Body.CheckbookCode, DOCNO_FEATURE.CHECKBOOK_PAYMENT, transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var paymentheader = new Entities.ApPaymentHeader()
                    {
                        PaymentHeaderId = Guid.NewGuid(),
                        TransactionDate = request.Body.TransactionDate,
                        TransactionType = request.Body.TransactionType,
                        DocumentNo = documentUniqueNo,
                        CurrencyCode = request.Body.CurrencyCode,
                        ExchangeRate = request.Body.ExchangeRate,
                        IsMultiply = request.Body.IsMultiply,
                        CheckbookCode = request.Body.CheckbookCode,
                        VendorId = request.Body.VendorId,
                        Description = request.Body.Description,
                        OriginatingTotalPaid = request.Body.OriginatingTotalPaid,
                        FunctionalTotalPaid = request.Body.FunctionalTotalPaid,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now,
                        Status = DOCSTATUS.NEW,
                        StatusComment = request.Body.StatusComment,
                    };

                    _context.ApPaymentHeaders.Add(paymentheader);

                    if (paymentheader.PaymentHeaderId != null && paymentheader.PaymentHeaderId != Guid.Empty)
                    {

                        var details = await InsertPaymentDetails(request.Body, paymentheader);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        JournalResponse jResponse = await _financeManager.CreateDistributionJournalAsync(paymentheader, details);

                        if (jResponse.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                PaymentHeaderId = paymentheader.PaymentHeaderId,
                                DocumentNo = paymentheader.DocumentNo
                            });
                        }
                        else
                        {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError(jResponse.ErrorMessage);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Payable Payment can not be created !");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Doc No can not be created. Please check Doc No setup !");
                }
            }
        }

        private async Task<List<Entities.ApPaymentDetail>> InsertPaymentDetails(RequestPaymentBody body, Entities.ApPaymentHeader header)
        {
            List<Entities.ApPaymentDetail> result = new List<Entities.ApPaymentDetail>();

            if (body.ApPaymentDetails != null)
            {
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

                if (result.Count > 0)
                {
                    await _context.ApPaymentDetails.AddRangeAsync(result);
                }
            }

            return result;
        }


    }
}
