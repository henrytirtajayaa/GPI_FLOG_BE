using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.Constants.PutPostingParam
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
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            List<ResponseItem> result = new List<ResponseItem>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                bool valid = true;

                foreach(var req in request.PostingParams)
                {
                    if(req.ParamId > 0)
                    {
                        //UPDATE
                        var spec = _context.FNPostingParams.Find(req.ParamId);
                        spec.AccountId = req.AccountId;
                        spec.Description = req.Description;
                        spec.PostingKey = req.PostingKey;

                        _context.FNPostingParams.Update(spec);

                        result.Add(new ResponseItem { ParamId = spec.ParamId, AccountId = spec.AccountId, Description = spec.Description, PostingKey = spec.PostingKey });
                    }
                    else
                    {
                        //CREATE NEW
                        Entities.FNPostingParam spec = new Entities.FNPostingParam();
                        spec.AccountId = req.AccountId;
                        spec.Description = req.Description;
                        spec.PostingKey = req.PostingKey;

                        _context.FNPostingParams.Add(spec);

                        await _context.SaveChangesAsync();

                        if (spec.ParamId > 0)
                        {
                            result.Add(new ResponseItem { ParamId = spec.ParamId, AccountId = spec.AccountId, Description = spec.Description, PostingKey = spec.PostingKey });
                        }                        
                    }
                }

                if (valid)
                {
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ApiResult<Response>.Ok(new Response { Success = true, Message = "Success", PostingParams = result });
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Posting Parameters can not be updated !");
                }                
            }            
        }
    }
}
