using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Companies;
using Entities = FLOG_BE.Model.Companies.Entities;
using Infrastructure;
using System.Collections.Generic;
using FLOG.Core;
using ViewEntities = FLOG_BE.Model.Companies.View;
using FLOG_BE.Helper;
using Infrastructure;

namespace FLOG.Core.Finance
{
    public class FinanceCatalog
    {
        private readonly CompanyContext _context;

        public FinanceCatalog(CompanyContext context)
        {
            _context = context;
        }
        
        public IQueryable<ViewEntities.BankActivity> GetBankActivities(string checkbookCode, DateTime transactionDate, Guid bankRecconcileId, System.Data.Common.DbTransaction trans = null)
        {
            List<ViewEntities.BankActivity> result = new List<ViewEntities.BankActivity>();

            if (bankRecconcileId == null)
                bankRecconcileId = Guid.Empty;

            if (!string.IsNullOrEmpty(checkbookCode))
            {
                string query = string.Format("SELECT * FROM [dbo].[fxnBankActivities]('{0}','{1}','{2}')", checkbookCode, transactionDate.ToString(DATE_FORMAT.YMD), bankRecconcileId);
                //Console.WriteLine("[GetBankActivities] *****QUERY****** " + query);
                result = RawQuery.Select<ViewEntities.BankActivity>(_context.Database, query, trans);
            }
            
            return result.AsQueryable();
        }

        public IQueryable<ViewEntities.BankActivity> GetBankReconcileDetailById(Guid bankRecconcileId, System.Data.Common.DbTransaction trans = null)
        {
            List<ViewEntities.BankActivity> result = new List<ViewEntities.BankActivity>();

            if (bankRecconcileId == null)
                bankRecconcileId = Guid.Empty;

            if (bankRecconcileId != Guid.Empty)
            {
                string query = string.Format("SELECT * FROM [dbo].[fxnBankReconcileDetailById]('{0}')", bankRecconcileId);
                Console.WriteLine("[GetBankReconcileDetailById] *****QUERY****** " + query);
                result = RawQuery.Select<ViewEntities.BankActivity>(_context.Database, query, trans);
            }

            return result.AsQueryable();
        }

    }
}
