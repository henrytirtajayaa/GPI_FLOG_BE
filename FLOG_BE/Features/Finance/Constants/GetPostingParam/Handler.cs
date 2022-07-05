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
using FLOG.Core.Finance;

namespace FLOG_BE.Features.Finance.Constants.GetPostingParam
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
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            List<ResponseHeader> headers = new List<ResponseHeader>();
            
            List<ResponseItem> listClosing = new List<ResponseItem>();
            var datas = _context.FNPostingParams.ToList();
            var accounts = _context.Accounts.ToList();

            //CLOSING CURRENT
            var param = datas.Where(x => x.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_CURRENT).FirstOrDefault();
            listClosing.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.GL_RETAIN_EARNING_CURRENT,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Retained Earnings Current Period (Close Month)") : "Retained Earnings Current Period (Close Month)"),
                AccountDesc = (param != null ? (accounts.Where(x=>x.AccountId==param.AccountId).Select(s=>s.Description).FirstOrDefault()) : "")
            });

            //CLOSING PREVIOUS (yearly)
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_PREVIOUS).FirstOrDefault();
            listClosing.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.GL_RETAIN_EARNING_PREVIOUS,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Retained Earnings Previous Period (Close Year)") : "Retained Earnings Previous Period (Close Year)"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            headers.Add(new ResponseHeader { Group = "CLOSING PERIOD", Params = listClosing });

            List<ResponseItem> listAR = new List<ResponseItem>();
            //AR ADVANCE RECEIPT
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.AR_ADVANCE_RECEIPT).FirstOrDefault();
            listAR.Add(new ResponseItem { 
                PostingKey = POSTING_PARAM.AR_ADVANCE_RECEIPT,
                ParamId = (param != null ? param.ParamId : 0),                
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param!= null ? (param.Description != "" ? param.Description : "Advance Receipt") : "Advance Receipt"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            //AR DISCOUNT
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.AR_DISCOUNT).FirstOrDefault();
            listAR.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.AR_DISCOUNT,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Discount") : "Discount"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            //AR WRITEOFF
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.AR_WRITEOFF).FirstOrDefault();
            listAR.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.AR_WRITEOFF,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Write-Off") : "Write-Off"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            headers.Add(new ResponseHeader { Group = "RECEIVABLE", Params = listAR });

            List<ResponseItem> listAP = new List<ResponseItem>();
            //AP ADVANCE PAYMENT
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.AP_ADVANCE_PAYMENT).FirstOrDefault();
            listAP.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.AP_ADVANCE_PAYMENT,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Advance Payment") : "Advance Payment"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            //AP DISCOUNT
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.AP_DISCOUNT).FirstOrDefault();
            listAP.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.AP_DISCOUNT,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Discount") : "Discount"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            //AP WRITEOFF
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.AP_WRITEOFF).FirstOrDefault();
            listAP.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.AP_WRITEOFF,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Write-Off") : "Write-Off"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            headers.Add(new ResponseHeader { Group = "PAYABLE", Params = listAP });

            List<ResponseItem> listCB = new List<ResponseItem>();
            //CHECKBOOK ADVANCE RECEIPT
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.FIN_ADVANCE_RECEIPT).FirstOrDefault();
            listCB.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.FIN_ADVANCE_RECEIPT,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Advance Receipt") : "Advance Receipt"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });

            //CHECKBOOK ADVANCE PAYMENT
            param = datas.Where(x => x.PostingKey == POSTING_PARAM.FIN_ADVANCE_PAYMENT).FirstOrDefault();
            listCB.Add(new ResponseItem
            {
                PostingKey = POSTING_PARAM.FIN_ADVANCE_PAYMENT,
                ParamId = (param != null ? param.ParamId : 0),
                AccountId = (param != null ? param.AccountId : ""),
                Description = (param != null ? (param.Description != "" ? param.Description : "Advance Payment") : "Advance Payment"),
                AccountDesc = (param != null ? (accounts.Where(x => x.AccountId == param.AccountId).Select(s => s.Description).FirstOrDefault()) : "")
            });
            
            headers.Add(new ResponseHeader { Group = "CHECKBOOK", Params = listCB });

            return ApiResult<Response>.Ok(new Response()
            {
                PostingParams = headers, 
                ListInfo = null
            });
        }
    }
}
