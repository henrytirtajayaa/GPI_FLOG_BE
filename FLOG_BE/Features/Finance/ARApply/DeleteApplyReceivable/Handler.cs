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
using FLOG.Core.Finance.Util;
using FLOG_BE.Model.Companies.Entities;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.ARApply.DeleteApplyReceivable
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _docGenerator = new DocumentGenerator(_context);
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
                    var record = await _context.ARApplyHeaders.FirstOrDefaultAsync(x => x.ReceivableApplyId == request.Body.ReceivableApplyId);
                    if (record != null)
                    {
                        if (record.Status == DOCSTATUS.NEW)
                        {
                            //DELETE DETAILS
                            var mapping = _context.Model.FindEntityType(typeof(ARApplyDetail)).Relational();
                            int isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE receivable_apply_id = {0} ", request.Body.ReceivableApplyId);

                            //DELETE DISTRIBUTIONS
                            var resp = await _financeManager.DeleteDistributionJournalAsync(record.DocumentNo, FLOG.Core.TRX_MODULE.TRX_APPLY_RECEIPT);

                            //DELETE HEADER
                            if (resp.Valid)
                            {
                                _context.Attach(record);
                                _context.Remove(record);

                                _docGenerator.DocNoDelete(record.DocumentNo, transaction.GetDbTransaction());

                                await _context.SaveChangesAsync();

                                transaction.Commit();

                                return ApiResult<Response>.Ok(new Response()
                                {
                                    ReceivableApplyId = request.Body.ReceivableApplyId,
                                    Message = string.Format("# {0} successfully deleted.", record.DocumentNo)
                                });
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Journal Details can not be deleted !");
                            }

                        }
                        else {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Apply Receivable can not be deleted.");

                        }
                    }
                    else{
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Apply Receivable Transaction not found.");
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
    }
}
