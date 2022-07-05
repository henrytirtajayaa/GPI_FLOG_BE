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
using LinqKit;
using FLOG.Core;
using FLOG_BE.Model.Central;
using System.Globalization;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Report.Dashboard.GetMyApprovalList
{ 
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _flogContext;
        private readonly CompanyContext _companyContext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext flog, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _flogContext = flog;
            _companyContext = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var tasks = await ObtainMyApprovalList(request.Initiator.UserId);
            
            return ApiResult<Response>.Ok(new Response()
            {
                MyApprovals = tasks,
                ListInfo = null
            });
        }

        private async Task<List<ResponseTask>> ObtainMyApprovalList(string personId)
        {
            List<ResponseTask> result = new List<ResponseTask>();

            try
            {
                if (!string.IsNullOrEmpty(personId))
                {
                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                    var query = (from form in _flogContext.Forms
                                 join access in _flogContext.SecurityRoleAccesses on form.FormId equals access.FormId
                                 join security in _flogContext.CompanySecurities on access.SecurityRoleId equals security.SecurityRoleId
                                 join session in _flogContext.SessionStates on security.CompanySecurityId equals session.CompanySecurityId
                                 where session.PersonId.ToUpper() == personId.ToUpper() && form.IsVisible
                                 select new
                                 {
                                     FormLink = form.FormLink.ToLower(),
                                     FormName = ti.ToTitleCase(form.FormName),
                                     FormMenuIcon = form.MenuIcon,
                                     FormModule = form.Module.ToUpper().Trim()
                                 }).AsNoTracking().AsQueryable();

                    int taskCounter = 0;

                    //NEGOTIATION SHEET
                    var cb = query.Where(q => q.FormLink.Equals("/sales/negotiation-sheet")).FirstOrDefault();

                    if (cb != null)
                    {
                        var waitingApprovals = (from t in _companyContext.TrxDocumentApprovals
                                       join ns in _companyContext.NegotiationSheetHeaders on t.TransactionId equals ns.NegotiationSheetId
                                       where ns.Status == DOCSTATUS.PROCESS
                                       && t.TrxModule == TRX_MODULE.TRX_SALES && t.TransactionType == ns.TransactionType
                                       && t.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET && t.ModeStatus == DOCSTATUS.NEW
                                       && t.Status == DOCSTATUS.NEW && t.PersonId.ToString().Equals(personId, StringComparison.OrdinalIgnoreCase)
                                       orderby t.Index ascending
                                       select t).Distinct().AsQueryable();

                        foreach(var wa in waitingApprovals)
                        {
                            var nextApprovalIndex = (from t in _companyContext.TrxDocumentApprovals
                                                where t.TrxModule == TRX_MODULE.TRX_SALES && t.TransactionType == wa.TransactionType
                                       && t.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET && t.ModeStatus == DOCSTATUS.NEW
                                       && t.Status == DOCSTATUS.NEW
                                                orderby t.Index ascending
                                                select t.Index).FirstOrDefault();
                            if(wa.Index == nextApprovalIndex)
                            {
                                taskCounter++;
                            }
                        }

                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/sales/negotiation-sheet",
                                IconClasses = string.Format("{0} bg-warning text-dark", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-directions")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("[ObtainMyTasks] " + ex.Message);
                Console.WriteLine("[ObtainMyTasks] " + ex.StackTrace);
            }            

            return result;
        }
    }
}
