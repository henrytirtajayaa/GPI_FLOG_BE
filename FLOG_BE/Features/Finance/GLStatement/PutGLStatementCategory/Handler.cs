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
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Finance.GLStatement.PutGLStatementCategory
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var header = _context.GLStatementCategories.Find(request.Body.CategoryId);

                if(header != null)
                {
                    //header.CategoryKey = request.Body.CategoryKey;
                    header.CategoryCaption = request.Body.CategoryCaption;
                    
                    _context.GLStatementCategories.Update(header);

                    var subs = await this.InsertSubCategories(header, request.Body);

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return ApiResult<Response>.Ok(new Response()
                    {
                        CategoryId = header.CategoryId,
                        Message = string.Format("Category #{0} successfully updated.", header.CategoryKey)
                    });
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Statement Category can not be found !");
                }               
            }
        }

        public async Task<List<Entities.GLStatementSubCategory>> InsertSubCategories(Entities.GLStatementCategory ctg, RequestFormBody body)
        {
            List<Entities.GLStatementSubCategory> subs = new List<Entities.GLStatementSubCategory>();

            if (body.SubCategories.Count > 0)
            {
                int i = 1;
                foreach (var item in body.SubCategories)
                {
                    if(item.Status > 0)
                    {
                        if(item.SubCategoryId > 0)
                        {
                            var sub = _context.GLStatementSubCategories.Find(item.SubCategoryId);
                            if(sub != null)
                            {
                                //sub.CategoryId = ctg.CategoryId;
                                //sub.SubCategoryKey = string.Format("{0}_{1}", ctg.CategoryKey, i.ToString());
                                sub.PosIndex = item.PosIndex;
                                sub.SubCategoryCaption = item.SubCategoryCaption;
                                sub.IsParamTotal = item.IsParamTotal;
                                sub.Inflow = item.Inflow;

                                _context.GLStatementSubCategories.Update(sub);

                                subs.Add(sub);
                            }                            
                        }
                        else
                        {
                            var sub = new Entities.GLStatementSubCategory
                            {
                                CategoryId = ctg.CategoryId,
                                SubCategoryKey = string.Format("{0}_{1}", ctg.CategoryKey, i.ToString()),
                                PosIndex = item.PosIndex,
                                SubCategoryCaption = item.SubCategoryCaption,
                                IsParamTotal = item.IsParamTotal,
                                Inflow = item.Inflow
                            };

                            await _context.GLStatementSubCategories.AddAsync(sub);

                            subs.Add(sub);
                        }                        
                    }
                    else
                    {
                        //REMOVE EXISTING
                        var layoutDetails = _context.GLStatementDetails.Where(x => x.SubCategoryId == item.SubCategoryId).ToList();

                        foreach(var lDetail in layoutDetails)
                        {
                            _context.GLStatementDetailSubs
                                    .Where(x => x.DetailId == lDetail.DetailId).ToList().ForEach(p => _context.Remove(p));
                        }

                        _context.GLStatementDetails
                        .Where(x => x.SubCategoryId == item.SubCategoryId).ToList().ForEach(p => _context.Remove(p));

                        _context.GLStatementSubCategories
                        .Where(x => x.CategoryId == body.CategoryId && x.SubCategoryId == item.SubCategoryId).ToList().ForEach(p => _context.Remove(p));
                    }
                    
                    i++;
                }
            }

            return subs;
        }

    }
}
