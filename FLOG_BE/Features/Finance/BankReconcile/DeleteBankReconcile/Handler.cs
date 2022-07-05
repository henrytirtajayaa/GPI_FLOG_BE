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

namespace FLOG_BE.Features.Finance.BankReconcile.DeleteBankReconcile
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
                BankReconcileId = request.Body.BankReconcileId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var record = await _context.BankReconcileHeaders.FirstOrDefaultAsync(x => x.BankReconcileId == request.Body.BankReconcileId);

                    if (record != null)
                    {
                        if (record.Status == DOCSTATUS.NEW)
                        {
                            //DELETE DETAILS
                            var mapping = _context.Model.FindEntityType(typeof(BankReconcileDetail)).Relational();
                            int isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE bank_reconcile_id = {0} ", request.Body.BankReconcileId);

                            mapping = _context.Model.FindEntityType(typeof(BankReconcileAdjustment)).Relational();
                            isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE bank_reconcile_id = {0} ", request.Body.BankReconcileId);

                            //DELETE HEADER
                            _context.Attach(record);
                            _context.Remove(record);

                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(new Response()
                            {
                                BankReconcileId = request.Body.BankReconcileId,
                                Message = string.Format("# {0} successfully deleted.", record.DocumentNo)
                            });

                        }
                        else {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Bank Reconciliation can not be deleted.");

                        }
                    }
                    else{
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Bank Reconciliation Transaction not found.");
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
