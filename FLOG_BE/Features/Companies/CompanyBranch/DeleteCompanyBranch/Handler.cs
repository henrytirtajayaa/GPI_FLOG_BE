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

namespace FLOG_BE.Features.Companies.CompanyBranch.DeleteCompanyBranch
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
            var record = _context.CompanyBranchs.FirstOrDefault(x => x.CompanyBranchId == request.Body.CompanyBranchId);
            if (record != null)
            {
                var journal = _context.DistributionJournalDetails.FirstOrDefault(x => x.BranchCode == record.BranchCode);
                
                if (journal != null)
                {
                    return ApiResult<Response>.ValidationError("Company Branch already used ! Deletion not allowed");
                }
                else
                {
                    _context.Attach(record);
                    _context.Remove(record);
                    await _context.SaveChangesAsync();

                    return ApiResult<Response>.Ok(new Response()
                    {
                        CompanyBranchId = record.CompanyBranchId
                    });
                }
            }
            else{
                return ApiResult<Response>.ValidationError("Company Branch not exist !");
            }
            
        }

    }
}