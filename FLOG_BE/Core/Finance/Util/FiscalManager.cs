using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Log;
using Microsoft.Extensions.Configuration;
using FLOG_BE.Model.Companies;

namespace FLOG.Core.Finance.Util
{
    public interface IFiscalManager
    {
        DateTime MinInputDate(int trxModule);
        Dictionary<decimal, bool> CurrentExchangeRate(string currencyCode, DateTime trxDate, int exchangeType);
    }

    public class FiscalManager : IFiscalManager
    {
        private readonly CompanyContext _context;

        public FiscalManager(CompanyContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Current Exchange Rate
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="trxDate"></param>
        /// <returns></returns>
        public Dictionary<decimal, bool> CurrentExchangeRate(string currencyCode, DateTime trxDate, int exchangeType = EXC_RATE_TYPE.FINANCIAL)
        {
            Dictionary<decimal, bool> excRate = new Dictionary<decimal, bool>();
            excRate.Add(0, true);

            var exchanges = (from ecd in _context.ExchangeRateDetails
                             join ech in _context.ExchangeRateHeaders on ecd.ExchangeRateHeaderId equals ech.ExchangeRateHeaderId
                             where ech.Status == DOCSTATUS.NEW && ech.CurrencyCode.ToUpper() == currencyCode.ToUpper() && ech.RateType == exchangeType
                                  && (trxDate.Date >= ecd.RateDate.Date && trxDate.Date <= ecd.ExpiredDate.Date)
                             orderby ecd.RateDate descending
                             select new
                             {
                                 ExchangeRateAmount = ecd.RateAmount,
                                 CalcMethod = ech.CalculationType
                             }).AsNoTracking().Take(1).ToList();

            if (exchanges != null && exchanges.Count() > 0)
            {
                excRate.Clear();
                excRate.Add(exchanges.ElementAt(0).ExchangeRateAmount, (exchanges.ElementAt(0).CalcMethod == CALC_METHOD.DIVISION ? false : true));
            }               
                
            return excRate;
        }

        /// <summary>
        /// OBTAIN MINIMUM FISCAL AVAILABLE FOR INPUT
        /// </summary>
        /// <returns></returns>
        public DateTime MinInputDate(int trxModule = TRX_MODULE.TRX_GENERAL_JOURNAL)
        {
            DateTime minInputDate = new DateTime(DateTime.Now.Year, 1, 1);

            if(trxModule == TRX_MODULE.TRX_CHECKBOOK || trxModule == TRX_MODULE.TRX_GENERAL_JOURNAL)
            {
                var periods = (from fpd in _context.FiscalPeriodDetails
                              join fph in _context.FiscalPeriodHeaders on fpd.FiscalHeaderId equals fph.FiscalHeaderId
                              where fph.PeriodYear == DateTime.Now.Year && !fph.ClosingYear && !fpd.IsCloseFinancial
                              orderby fph.ClosingYear ascending, fpd.PeriodIndex ascending
                              select new
                              {
                                  PeriodStart = fpd.PeriodStart
                              }).AsNoTracking().Take(1).ToList();

                if (periods.Count > 0)
                    minInputDate = periods.ElementAt(0).PeriodStart;
            }else if (trxModule == TRX_MODULE.TRX_PAYABLE)
            {
                var periods = (from fpd in _context.FiscalPeriodDetails
                               join fph in _context.FiscalPeriodHeaders on fpd.FiscalHeaderId equals fph.FiscalHeaderId
                               where fph.PeriodYear == DateTime.Now.Year && !fph.ClosingYear && !fpd.IsClosePurchasing
                               orderby fph.ClosingYear ascending, fpd.PeriodIndex ascending
                               select new
                               {
                                   PeriodStart = fpd.PeriodStart
                               }).AsNoTracking().Take(1).ToList();

                if (periods.Count > 0)
                    minInputDate = periods.ElementAt(0).PeriodStart;
            }
            else if (trxModule == TRX_MODULE.TRX_RECEIVABLE)
            {
                var periods = (from fpd in _context.FiscalPeriodDetails
                               join fph in _context.FiscalPeriodHeaders on fpd.FiscalHeaderId equals fph.FiscalHeaderId
                               where fph.PeriodYear == DateTime.Now.Year && !fph.ClosingYear && !fpd.IsCloseSales
                               orderby fph.ClosingYear ascending, fpd.PeriodIndex ascending
                               select new
                               {
                                   PeriodStart = fpd.PeriodStart
                               }).AsNoTracking().Take(1).ToList();

                if (periods.Count > 0)
                    minInputDate = periods.ElementAt(0).PeriodStart;
            }
            else if (trxModule == TRX_MODULE.TRX_ASSET)
            {
                var periods = (from fpd in _context.FiscalPeriodDetails
                               join fph in _context.FiscalPeriodHeaders on fpd.FiscalHeaderId equals fph.FiscalHeaderId
                               where fph.PeriodYear == DateTime.Now.Year && !fph.ClosingYear && !fpd.IsCloseAsset
                               orderby fph.ClosingYear ascending, fpd.PeriodIndex ascending
                               select new
                               {
                                   PeriodStart = fpd.PeriodStart
                               }).AsNoTracking().Take(1).ToList();

                if (periods.Count > 0)
                    minInputDate = periods.ElementAt(0).PeriodStart;
            }
            else if (trxModule == TRX_MODULE.TRX_INVENTORY)
            {
                var periods = (from fpd in _context.FiscalPeriodDetails
                               join fph in _context.FiscalPeriodHeaders on fpd.FiscalHeaderId equals fph.FiscalHeaderId
                               where fph.PeriodYear == DateTime.Now.Year && !fph.ClosingYear && !fpd.IsCloseInventory
                               orderby fph.ClosingYear ascending, fpd.PeriodIndex ascending
                               select new
                               {
                                   PeriodStart = fpd.PeriodStart
                               }).AsNoTracking().Take(1).ToList();

                if (periods.Count > 0)
                    minInputDate = periods.ElementAt(0).PeriodStart;
            }
            else
            {
                var periods = (from fpd in _context.FiscalPeriodDetails
                               join fph in _context.FiscalPeriodHeaders on fpd.FiscalHeaderId equals fph.FiscalHeaderId
                               where fph.PeriodYear == DateTime.Now.Year && !fph.ClosingYear && !fpd.IsCloseFinancial
                               orderby fph.ClosingYear ascending, fpd.PeriodIndex ascending
                               select new
                               {
                                   PeriodStart = fpd.PeriodStart
                               }).AsNoTracking().Take(1).ToList();

                if (periods.Count > 0)
                    minInputDate = periods.ElementAt(0).PeriodStart;
            }

            return minInputDate;
        }


    }
}