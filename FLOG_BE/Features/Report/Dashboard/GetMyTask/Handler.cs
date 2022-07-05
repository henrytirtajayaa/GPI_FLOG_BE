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

namespace FLOG_BE.Features.Report.Dashboard.GetMyTask
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

            var tasks = await ObtainMyTasks(request.Initiator.UserId);
            
            return ApiResult<Response>.Ok(new Response()
            {
                MyTasks = tasks,
                ListInfo = null
            });
        }

        private async Task<List<ResponseTask>> ObtainMyTasks(string personId)
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
                    //CHECKBOOK
                    var cb = query.Where(q => q.FormLink.Equals("/transaction/checkbook")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.CheckbookTransactionHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/checkbook",
                                IconClasses = string.Format("{0} bg-primary", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-bulb")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //RECEIVABLE
                    cb = query.Where(q => q.FormLink.Equals("/transaction/receivable")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.ReceivableTransactionHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/receivable",
                                IconClasses = string.Format("{0} bg-primary", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-diamond")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //PAYABLE
                    cb = query.Where(q => q.FormLink.Equals("/transaction/payable")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.PayableTransactionHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/payable",
                                IconClasses = string.Format("{0} bg-warning", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-bell")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //APPLY AP
                    cb = query.Where(q => q.FormLink.Equals("/transaction/payable-apply")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.APApplyHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/payable-apply",
                                IconClasses = string.Format("{0} bg-warning", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-bell")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //APPLY AR
                    cb = query.Where(q => q.FormLink.Equals("/transaction/receivable-apply")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.ARApplyHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/receivable-apply",
                                IconClasses = string.Format("{0} bg-warning", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-diamond")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //RECONCILE BANK
                    cb = query.Where(q => q.FormLink.Equals("/transaction/bank-reconcile")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.BankReconcileHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/bank-reconcile",
                                IconClasses = string.Format("{0} bg-primary", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-trophy")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //GJE
                    cb = query.Where(q => q.FormLink.Equals("/transaction/general-journal")).FirstOrDefault();
                    
                    if (cb != null)
                    {
                        taskCounter = _companyContext.JournalEntryHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();

                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/transaction/general-journal",
                                IconClasses = string.Format("{0} bg-secondary", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-trophy")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }


                    //QUOTATION
                    cb = query.Where(q => q.FormLink.Equals("/sales/quotation")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.SalesQuotationHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/sales/quotation",
                                IconClasses = string.Format("{0} bg-purple", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-tag")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                            taskCounter = 0;
                        }
                    }

                    //SALES ORDER
                    cb = query.Where(q => q.FormLink.Equals("/sales/order")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.SalesOrderHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/sales/order",
                                IconClasses = string.Format("{0} bg-purple", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-tag")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
                        }
                    }

                    //NEGOTIATION SHEET
                    cb = query.Where(q => q.FormLink.Equals("/sales/negotiation-sheet")).FirstOrDefault();

                    if (cb != null)
                    {
                        taskCounter = _companyContext.NegotiationSheetHeaders.Where(x => x.Status == DOCSTATUS.NEW).Count();
                        if (taskCounter > 0)
                        {
                            ResponseTask task = new ResponseTask
                            {
                                TaskUrl = "/sales/negotiation-sheet",
                                IconClasses = string.Format("{0} bg-purple", (!string.IsNullOrEmpty(cb.FormMenuIcon) ? cb.FormMenuIcon : "icon-tag")),
                                Title = cb.FormName,
                                Description = string.Format("{0}", cb.FormModule),
                                TaskCount = taskCounter
                            };

                            result.Add(task);
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
