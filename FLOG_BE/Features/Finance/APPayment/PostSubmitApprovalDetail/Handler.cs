using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;

namespace FLOG_BE.Features.Finance.ApPayment.PostSubmitApprovalDetail
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _Flogcontext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext Flogcontext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _Flogcontext = Flogcontext;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    var PaymentHeaders = await _context.ApPaymentHeaders.FirstOrDefaultAsync(x => x.PaymentHeaderId== request.Body.PaymentHeaderId);

                    string docNo = "";

                    if (PaymentHeaders != null)
                    {
                        docNo = PaymentHeaders.DocumentNo;
                        PaymentHeaders.Status = DOCSTATUS.PROCESS;
                        //PaymentHeaders.VoidBy = request.Initiator.UserId;
                        //PaymentHeaders.VoidDate = DateTime.Now;

                        _context.ApPaymentHeaders.Update(PaymentHeaders);
                        await _context.SaveChangesAsync();

                        var CekCheckbook = await _context.Checkbooks.FirstOrDefaultAsync(x => x.CheckbookCode == request.Body.CheckbookCode);
                        var getApprovalHeader = await _context.ApprovalSetupHeaders.FirstOrDefaultAsync(x => x.ApprovalCode == CekCheckbook.ApprovalCode);
                        var getApprovalDetail = _context.ApprovalSetupDetails.Where(x => x.ApprovalSetupHeaderId == getApprovalHeader.ApprovalSetupHeaderId);

                        _context.ApPaymentApprovals.Where(x => x.PaymentHeaderId == request.Body.PaymentHeaderId)
                         .ToList().ForEach(x => _context.ApPaymentApprovals.Remove(x));

                        foreach (var item in getApprovalDetail)
                        {
                            if (item.PersonId != null)
                            {
                                var transactionApproval = new Entities.ApPaymentApproval()
                                {
                                    PaymentApprovalId = Guid.NewGuid(),
                                    PaymentHeaderId = request.Body.PaymentHeaderId,
                                    Index = item.Level,
                                    PersonId = item.PersonId,
                                    PersonCategoryId = item.PersonCategoryId,
                                    Status = DOCSTATUS.NEW
                                };

                                _context.ApPaymentApprovals.Add(transactionApproval);
                            }
                            else
                            {
                                var getPersonId = _Flogcontext.Persons.Where(x => x.PersonCategoryId == item.PersonCategoryId.ToString());
                                foreach (var child in getPersonId)
                                {
                                    var transactionApproval = new Entities.ApPaymentApproval()
                                    {
                                        PaymentApprovalId = Guid.NewGuid(),
                                        PaymentHeaderId = request.Body.PaymentHeaderId,
                                        Index = item.Level,
                                        PersonId = Guid.Parse(child.PersonId),
                                        PersonCategoryId = item.PersonCategoryId,
                                        Status = DOCSTATUS.NEW
                                    };

                                    _context.ApPaymentApprovals.Add(transactionApproval);
                                }
                            }

                        }
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    var response = new Response()
                    {
                        PaymentHeaderId = request.Body.PaymentHeaderId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(PaymentHeaders.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.InternalServerError( ex.Message);
                }
            }
        }
    }
}
