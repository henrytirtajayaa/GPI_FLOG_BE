using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Log;
using Microsoft.Extensions.Configuration;
using FLOG_BE.Model.Companies;
using FLOG_BE.Helper;
using System.Data;
using System.Text;
using Entities = FLOG_BE.Model.Companies.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace FLOG.Core.Smartview
{
   public class SmartviewQuery : ISmartviewQuery
    {
        private readonly CompanyContext _context;
        private IConfiguration _config;
        private LogWriter _logger;

        public SmartviewQuery(CompanyContext context)
        {
            _context = context;
        }

        public SmartviewQuery(CompanyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

            _logger = new LogWriter(_config);
        }
        public DataTable DefaultSmartview(String tableName, DbTransaction trans = null)
        {
            string qry = string.Format("SELECT TOP 5 * FROM {0}", tableName);
            var queries = RawQuery.SelectRawSql(_context.Database, qry, trans);

            return queries;
        }
        public DataTable ResultSmartview(String tableName, String Filter, DbTransaction trans = null)
        {
            string query = "SELECT * FROM {0} {1}";

            string qry = string.Format(query, tableName, Filter);
            var queries = RawQuery.SelectRawSql(_context.Database, qry, trans);

            return queries;
        }
    }
}
