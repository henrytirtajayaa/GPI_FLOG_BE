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
using Infrastructure;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Finance.APApply.DeleteApplyPayable
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
                PayableApplyId = request.Body.PayableApplyId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var record = await _context.APApplyHeaders.FirstOrDefaultAsync(x => x.PayableApplyId == request.Body.PayableApplyId);
                    if (record != null)
                    {
                        if (record.Status == DOCSTATUS.NEW)
                        {
                            //DELETE DETAILS
                            var mapping = _context.Model.FindEntityType(typeof(Entities.APApplyDetail)).Relational();
                            int isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE payable_apply_id = {0} ", request.Body.PayableApplyId);

                            //DELETE DISTRIBUTIONS
                            var resp = await _financeManager.DeleteDistributionJournalAsync(record.DocumentNo, FLOG.Core.TRX_MODULE.TRX_APPLY_PAYABLE);

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
                                    PayableApplyId = request.Body.PayableApplyId,
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
                            return ApiResult<Response>.ValidationError("Apply Payment can not be deleted.");

                        }
                    }
                    else{
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Apply Payment Transaction not found.");
                    }
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
