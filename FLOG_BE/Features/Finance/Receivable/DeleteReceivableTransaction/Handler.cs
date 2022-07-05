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
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.Receivable.DeleteReceivableTransaction
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
            var record = _context.ReceivableTransactionHeaders.FirstOrDefault(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId);

            if(record.Status == FLOG.Core.DOCSTATUS.NEW)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    //DELETE DETAILS
                    var mapping = _context.Model.FindEntityType(typeof(ReceivableTransactionDetail)).Relational();
                    int isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName  + " WHERE receive_transaction_id = {0} ", request.Body.ReceiveTransactionId);

                    mapping = _context.Model.FindEntityType(typeof(ReceivableTransactionTax)).Relational();
                    isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE receive_transaction_id = {0} ", request.Body.ReceiveTransactionId);

                    //DELETE DISTRIBUTIONS
                    var resp = await _financeManager.DeleteDistributionJournalAsync(record.DocumentNo, FLOG.Core.TRX_MODULE.TRX_RECEIVABLE);

                    //DELETE HEADER
                    if (resp.Valid)
                    {
                        _context.Attach(record);
                        _context.Remove(record);

                        //UPDATE LAST NO
                        _docGenerator.DocNoDelete(record.DocumentNo, transaction.GetDbTransaction());

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            ReceiveTransactionId = request.Body.ReceiveTransactionId,
                            Message = string.Format("# {0} successfully deleted.", record.DocumentNo)
                        });
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Record can not be deleted !");
                    }                    
                }                 

            }
            else
            {
                return ApiResult<Response>.ValidationError("Only NEW record can be deleted !");
            }            
        }
    }
}
