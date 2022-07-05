using FLOG_BE.Model.Central;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Log;
using Microsoft.Extensions.Configuration;

namespace FLOG_BE.Model.Fetcher
{
    public class CompanyFetcher : ICompanyFetcher
    {
        private IHttpContextAccessor _httpContextAccessor { get; set; }
        private readonly FlogContext _context;
        private IConfiguration _config;
        private LogWriter _logger;
        public CompanyFetcher(IHttpContextAccessor httpContextAccessor, FlogContext context, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _config = config;

            _logger = new LogWriter(_config);
        }

        public string GetCompanyConnection()
        {
            string connStr = "";
            
            if (_httpContextAccessor.HttpContext.User.Claims != null)
            {
                string companyId = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Equals("CompanyId")).FirstOrDefault().Value;
                var person = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Name));

                //_logger.Write(LoggerType.Info, "[companyId] " + companyId + " [person] " + person.FirstOrDefault().Value);

                if (!string.IsNullOrEmpty(companyId))
                {
                    var database = (from ss in _context.SessionStates
                                    join cp in _context.Companies on ss.CompanyId equals cp.CompanyId
                                    where ss.PersonId == person.FirstOrDefault().Value && ss.CompanyId == companyId
                                    select cp).Distinct().AsNoTracking().ToList();

                    if (database != null)
                    {
                        connStr = "Data Source=" + database.FirstOrDefault().DatabaseAddress + ";Initial Catalog=" + database.FirstOrDefault().DatabaseId + ";User ID=sa" + ";Password=" + database.FirstOrDefault().DatabasePassword;
                    }
                }
                else
                {
                    var database = (from ss in _context.SessionStates
                                    join cp in _context.Companies on ss.CompanyId equals cp.CompanyId
                                    where ss.PersonId == person.FirstOrDefault().Value
                                    select cp).Distinct().AsNoTracking().ToList();

                    if (database != null)
                    {
                        connStr = "Data Source=" + database.FirstOrDefault().DatabaseAddress + ";Initial Catalog=" + database.FirstOrDefault().DatabaseId + ";User ID=sa" + ";Password=" + database.FirstOrDefault().DatabasePassword;
                    }
                }
            }
            else
            {
                _logger.Write(LoggerType.Info, "[GetCompanyConnection] User Claims is NULL ");
            }
            
            return connStr;
        }
    }
}
