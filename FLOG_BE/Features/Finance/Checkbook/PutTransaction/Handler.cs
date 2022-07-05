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

namespace FLOG_BE.Features.Finance.Checkbook.PutTransaction
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
                CheckbookTransactionId = request.Body.CheckbookTransactionId,
                DocumentNo = request.Body.DocumentNo,
                DocumentType = request.Body.DocumentType
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var CheckbookHeaders = await _context.CheckbookTransactionHeaders.FirstOrDefaultAsync(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId);

                    if (CheckbookHeaders != null)
                    {
                        _context.CheckbookTransactionDetails.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId)
                         .ToList().ForEach(x => _context.CheckbookTransactionDetails.Remove(x));

                        CheckbookHeaders.BranchCode = request.Body.BranchCode;
                        CheckbookHeaders.CheckbookCode = request.Body.CheckbookCode;
                        CheckbookHeaders.TransactionDate = request.Body.TransactionDate;
                        CheckbookHeaders.TransactionType = request.Body.TransactionType;
                        CheckbookHeaders.CurrencyCode = request.Body.CurrencyCode;
                        CheckbookHeaders.ExchangeRate = request.Body.ExchangeRate;
                        CheckbookHeaders.IsVoid = request.Body.IsVoid;
                        CheckbookHeaders.VoidDocumentNo = request.Body.VoidDocumentNo;
                        CheckbookHeaders.PaidSubject = request.Body.PaidSubject;
                        CheckbookHeaders.Description = request.Body.Description;
                        CheckbookHeaders.OriginatingTotalAmount = request.Body.OriginatingTotalAmount;
                        CheckbookHeaders.FunctionalTotalAmount = request.Body.FunctionalTotalAmount;
                        CheckbookHeaders.Status = request.Body.Status;
                        CheckbookHeaders.StatusComment = request.Body.StatusComment;
                        CheckbookHeaders.ModifiedBy = request.Initiator.UserId;
                        CheckbookHeaders.ModifiedDate = DateTime.Now;
                        CheckbookHeaders.IsMultiply = request.Body.IsMultiply;

                        if (request.Body.RequestCheckbookDetails.Count > 0)
                        {
                            List<Entities.CheckbookTransactionDetail> details = new List<Entities.CheckbookTransactionDetail>();

                            foreach (var item in request.Body.RequestCheckbookDetails)
                            {
                                var CheckbookDetail = new CheckbookTransactionDetail()
                                {
                                    TransactionDetailId = Guid.NewGuid(),
                                    CheckbookTransactionId = CheckbookHeaders.CheckbookTransactionId,
                                    ChargesId = item.ChargesId,
                                    ChargesDescription = item.ChargesDescription,
                                    OriginatingAmount = item.OriginatingAmount,
                                    FunctionalAmount = item.FunctionalAmount,
                                    Status = item.Status,
                                    RowIndex = item.RowIndex,
                                };
                                details.Add(CheckbookDetail);
                            }

                            await _context.CheckbookTransactionDetails.AddRangeAsync(details);

                            //CREATE DISTRIBUTION JOURNAL HERE
                            var resp = await _financeManager.CreateDistributionJournalAsync(CheckbookHeaders, details);

                            if (resp.Valid)
                            {
                                await _context.SaveChangesAsync();

                                transaction.Commit();

                                return ApiResult<Response>.Ok(response);
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Checkbook Journal can not be updated.");
                            }
                        }
                        else
                        {
                            return ApiResult<Response>.ValidationError("Checkbook Transaction Details not exist.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Checkbook Transaction  not exist.");
                    }                    
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Checkbook Transaction can not be updated.");
                }
            }
        }
    }
}
