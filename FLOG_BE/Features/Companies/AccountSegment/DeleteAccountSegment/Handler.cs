﻿using System;
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



namespace FLOG_BE.Features.Companies.AccountSegment.DeleteAccountSegment
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

            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE [account_segment]");
            foreach (var item in request.Body)
            {
                var record = _context.AccountSegments.FirstOrDefault(x => x.SegmentId == item.SegmentId);
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(new Response());
        }
    }
}
