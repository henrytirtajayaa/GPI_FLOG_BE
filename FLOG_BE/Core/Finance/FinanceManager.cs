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
using FLOG_BE.Model.Companies.Entities;
using System.Text;
using FLOG_BE.Helper;
using System.Data;
using Infrastructure;

namespace FLOG.Core.Finance.Util
{
    public interface IFinanceManager
    {
        Task<JournalResponse> PostJournalAsync(string documentNo, int trxModule, string userId, DateTime postingDate, bool includeTransaction = true);
        Task<JournalResponse> VoidJournalAsync(string documentNo, int trxModule,  string userId, DateTime postingDate, bool includeTransaction = true);
        Task<JournalResponse> DeleteDistributionJournalAsync(string documentNo, int trxModule, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(JournalEntryHeader header, List<JournalEntryDetail> details, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(PayableTransactionHeader header, List<PayableTransactionDetail> details, List<PayableTransactionTax> taxes, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(ReceivableTransactionHeader header, List<ReceivableTransactionDetail> details, List<ReceivableTransactionTax>  taxes, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(CheckbookTransactionHeader header, List<CheckbookTransactionDetail> details, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(ApPaymentHeader header, List<ApPaymentDetail> details, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(ArReceiptHeader header, List<ArReceiptDetail> details, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(APApplyHeader header, List<APApplyDetail> details, bool includeTransaction = true);

        Task<JournalResponse> CreateDistributionJournalAsync(ARApplyHeader header, List<ARApplyDetail> details, bool includeTransaction = true);
        
        Task<JournalResponse> CreateDistributionJournalAsync(DepositSettlementHeader header, List<DepositSettlementDetail> details, bool includeTransaction = true);

        Task<string> GetPostingAccount(int postingParam);

        Task<JournalResponse> CloseMonth(string currencyCode, int periodYear, int periodIndex, string userId, bool includeTransaction = true);

        Task<JournalResponse> UncloseMonth(string currencyCode, int periodYear, int periodIndex, string userId, bool includeTransaction = true);
    }

    public class FinanceManager : IFinanceManager
    {
        private readonly CompanyContext _context;
        private List<Account> _coasActive;

        public FinanceManager(CompanyContext context)
        {
            _context = context;
            this.GetActiveCOA();
        }

        private void GetActiveCOA()
        {
            _coasActive = _context.Accounts.Where(x => x.Inactive == false).ToList();
        }

        #region Account Check

        public bool IsActiveAccountId(string accountId)
        {
            return _coasActive.Where(x => x.AccountId.Equals(accountId, StringComparison.OrdinalIgnoreCase)).Any();
        }

        #endregion Account Check

        #region Distribution Journal

        /// <summary>
        /// Distribution Journal for Journal Entry
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="includeTransaction"></param>
        /// <returns></returns>
        public async Task<JournalResponse> CreateDistributionJournalAsync(JournalEntryHeader header, List<JournalEntryDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (details != null && details.Count > 0)
            {
                if (!string.IsNullOrEmpty(header.DocumentNo))
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_GENERAL_JOURNAL).FirstOrDefault();
                                        
                    if(edit != null)
                    {
                        var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                        //REMOVE EXISTING
                        await _context.Database.ExecuteSqlCommandAsync("DELETE FROM "+ mapping.TableName  + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                        //UPDATE HEADER
                        
                        edit.CurrencyCode = header.CurrencyCode;
                        edit.Description = header.Description;
                        edit.DocumentDate = header.TransactionDate;
                        edit.ExchangeRate = header.ExchangeRate;
                        edit.IsMultiply = header.IsMultiply;
                        edit.TransactionId = header.JournalEntryHeaderId;
                        edit.OriginatingTotal = details.Sum(x => x.OriginatingDebit);
                        edit.FunctionalTotal = details.Sum(x => x.FunctionalDebit);                        
                        edit.ModifiedBy = header.ModifiedBy;
                        edit.ModifiedDate = DateTime.Now;
                        edit.Status = header.Status;

                        _context.Update(edit);

                        distHeader = edit;
                    }
                    else
                    {
                        //UPDATE HEADER
                        distHeader.DistributionHeaderId = Guid.NewGuid();
                        distHeader.TrxModule = TRX_MODULE.TRX_GENERAL_JOURNAL;
                        distHeader.TransactionId = header.JournalEntryHeaderId;
                        distHeader.DocumentDate = header.TransactionDate;                        
                        distHeader.DocumentNo = header.DocumentNo;
                        distHeader.CurrencyCode = header.CurrencyCode;
                        distHeader.ExchangeRate = header.ExchangeRate;
                        distHeader.IsMultiply = header.IsMultiply;
                        distHeader.Description = header.Description;
                        distHeader.OriginatingTotal = details.Sum(x => x.OriginatingDebit);
                        distHeader.FunctionalTotal = details.Sum(x => x.FunctionalDebit);
                        distHeader.CreatedBy = header.CreatedBy;
                        distHeader.CreatedDate = DateTime.Now;
                        distHeader.Status = header.Status;

                        _context.DistributionJournalHeaders.Add(distHeader);

                    }

                    //INSERT NEW DISTRIBUTION JOURNAL
                    if (distHeader.DistributionHeaderId != Guid.Empty)
                    {
                        List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();
                        foreach (JournalEntryDetail jed in details)
                        {
                            var distributionJournalDetail = new DistributionJournalDetail()
                            {
                                DistributionDetailId = Guid.NewGuid(),
                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                AccountId = jed.AccountId,
                                ChargesId = Guid.Empty,
                                JournalNote = jed.Description,
                                OriginatingDebit = jed.OriginatingDebit,
                                OriginatingCredit = jed.OriginatingCredit,
                                FunctionalDebit = jed.FunctionalDebit,
                                FunctionalCredit = jed.FunctionalCredit,
                                Status = DOCSTATUS.NEW,
                                JournalDate = header.TransactionDate,
                                BranchCode = header.BranchCode
                            };

                            distDetails.Add(distributionJournalDetail);
                        }

                        _context.DistributionJournalDetails.AddRange(distDetails);
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Header GUID can not be generated";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Document No is not valid";
                }
            }

            return resp;
        }

        /// <summary>
        /// Distribution Journal for Payable Entry
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="taxes"></param>
        /// <param name="includeTransaction"></param>
        /// <returns></returns>
        public async Task<JournalResponse> CreateDistributionJournalAsync(PayableTransactionHeader header, List<PayableTransactionDetail> details, List<PayableTransactionTax> taxes, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (details != null && details.Count > 0)
            {
                if (!string.IsNullOrEmpty(header.DocumentNo))
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_PAYABLE).FirstOrDefault();

                    if (edit != null)
                    {
                        var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                        //REMOVE EXISTING
                        await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                        //UPDATE HEADER
                        edit.CurrencyCode = header.CurrencyCode;
                        edit.Description = header.Description;
                        edit.DocumentDate = header.TransactionDate;
                        edit.ExchangeRate = header.ExchangeRate;
                        edit.IsMultiply = header.IsMultiply;
                        edit.TrxModule = TRX_MODULE.TRX_PAYABLE;
                        edit.TransactionId = header.PayableTransactionId;
                        edit.OriginatingTotal = (header.SubtotalAmount - header.DiscountAmount + header.TaxAmount);
                        edit.FunctionalTotal = CALC.FunctionalAmount(header.IsMultiply, edit.OriginatingTotal, header.ExchangeRate);
                        edit.ModifiedBy = header.ModifiedBy;
                        edit.ModifiedDate = DateTime.Now;
                        edit.Status = header.Status;

                        _context.Update(edit);

                        distHeader = edit;
                    }
                    else
                    {
                        //UPDATE HEADER
                        distHeader.DistributionHeaderId = Guid.NewGuid();
                        distHeader.TrxModule = TRX_MODULE.TRX_PAYABLE;
                        distHeader.TransactionId = header.PayableTransactionId;
                        distHeader.DocumentDate = header.TransactionDate;
                        distHeader.DocumentNo = header.DocumentNo;
                        distHeader.CurrencyCode = header.CurrencyCode;
                        distHeader.ExchangeRate = header.ExchangeRate;
                        distHeader.IsMultiply = header.IsMultiply;
                        distHeader.Description = header.Description;
                        distHeader.OriginatingTotal = (header.SubtotalAmount - header.DiscountAmount + header.TaxAmount);
                        distHeader.FunctionalTotal = CALC.FunctionalAmount(header.IsMultiply, distHeader.OriginatingTotal, header.ExchangeRate);
                        distHeader.CreatedBy = header.CreatedBy;
                        distHeader.CreatedDate = DateTime.Now;
                        distHeader.Status = header.Status;

                        _context.DistributionJournalHeaders.Add(distHeader);

                    }

                    //INSERT NEW DISTRIBUTION JOURNAL
                    if (distHeader.DistributionHeaderId != Guid.Empty)
                    {
                        bool isTaxDetail = true;
                        decimal originatingSubtotal = 0;

                        if (taxes != null)
                        {
                            if (taxes.Count > 0)
                                isTaxDetail = false;
                        }

                        var recDetails = (from det in details
                                          join ch in _context.Charges on det.ChargesId equals ch.ChargesId
                                          select new ChargeDetail
                                          {
                                              AccountId = ch.CostAccountNo,
                                              ChargesId = det.ChargesId,
                                              ChargesName = ch.ChargesName,
                                              JournalNote = det.ChargesDescription,
                                              OriginatingAmount = det.OriginatingAmount,
                                              OriginatingDiscount = det.OriginatingDiscount,
                                              OriginatingTax = det.OriginatingTax,
                                              OriginatingExtended = det.OriginatingExtendedAmount,
                                              FunctionalAmount = CALC.FunctionalAmount(header.IsMultiply, det.OriginatingAmount, header.ExchangeRate),
                                              FunctionalDiscount = det.FunctionalDiscount,
                                              FunctionalTax = det.FunctionalTax,
                                              FunctionalExtended = det.FunctionalExtendedAmount,
                                              TaxSchedule = (from tax in _context.TaxSchedules
                                                             where tax.TaxScheduleId == det.TaxScheduleId
                                                             select tax).FirstOrDefault()
                                          }).ToList();

                        //OBTAIN CUSTOM CHARGE DETAIL ACCOUNT IF FROM NEGOTIATIONSHEET
                        if (!string.IsNullOrEmpty(header.NsDocumentNo))
                        {
                            Guid? shippingLineId = _context.NegotiationSheetHeaders.Where(x => x.DocumentNo.Equals(header.NsDocumentNo, StringComparison.OrdinalIgnoreCase)).Select(ns => ns.ShippingLineId).FirstOrDefault();

                            if (shippingLineId != null && shippingLineId != Guid.Empty)
                            {
                                foreach (var ch in recDetails)
                                {
                                    string alternateAccountId = _context.ChargesDetails.Where(x => x.ChargesId == ch.ChargesId && x.ShippingLineId == shippingLineId).Select(sl => sl.CostAccountNo).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(alternateAccountId))
                                    {
                                        ch.AccountId = alternateAccountId;
                                    }
                                }
                            }
                        }

                        List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                        List<TaxScheduleAccount> chargeTaxAccount = new List<TaxScheduleAccount>();

                        foreach (var jed in recDetails)
                        {
                            //CHARGES DETAILS
                            if (!string.IsNullOrEmpty(jed.AccountId) && IsActiveAccountId(jed.AccountId))
                            {
                                var distributionJournalDetail = new DistributionJournalDetail()
                                {
                                    DistributionDetailId = Guid.NewGuid(),
                                    DistributionHeaderId = distHeader.DistributionHeaderId,
                                    AccountId = jed.AccountId,
                                    ChargesId = jed.ChargesId,
                                    JournalNote = jed.JournalNote,
                                    OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : (jed.OriginatingAmount - jed.OriginatingDiscount)),
                                    OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? (jed.OriginatingAmount - jed.OriginatingDiscount) : 0),
                                    FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : (jed.FunctionalAmount - jed.FunctionalDiscount)),
                                    FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? (jed.FunctionalAmount - jed.FunctionalDiscount) : 0),
                                    Status = DOCSTATUS.NEW,
                                    JournalDate = header.TransactionDate,
                                    BranchCode = header.BranchCode,
                                };

                                distDetails.Add(distributionJournalDetail);

                                originatingSubtotal += (jed.OriginatingAmount - jed.OriginatingDiscount);

                                if (isTaxDetail)
                                {
                                    //TAX DETAILS
                                    if (jed.OriginatingTax > 0)
                                    {
                                        if (jed.TaxSchedule != null)
                                        {
                                            if (IsActiveAccountId(jed.TaxSchedule.TaxAccountNo))
                                            {
                                                var taxAccount = chargeTaxAccount.Where(x => x.AccountId.Equals(jed.TaxSchedule.TaxAccountNo)).FirstOrDefault();

                                                if (taxAccount == null)
                                                {
                                                    chargeTaxAccount.Add(new TaxScheduleAccount { AccountId = jed.TaxSchedule.TaxAccountNo, TaxAmount = jed.OriginatingTax, TaxSchedule = jed.TaxSchedule });
                                                }
                                                else
                                                {
                                                    taxAccount.TaxAmount += jed.OriginatingTax;
                                                }
                                            }
                                            else
                                            {
                                                resp.Valid = false;
                                                resp.ErrorMessage = string.Format("{0} Account is not valid or inactive !", jed.TaxSchedule.TaxScheduleCode);

                                                break;
                                            }                                            
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("{0} Tax Schedule is not valid or empty !", jed.ChargesName);

                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("{0} Cost Account No is not valid or empty !", jed.ChargesName);

                                break;
                            }
                        }

                        //DISCOUNT FOOTER
                        if (header.DiscountAmount > 0 && resp.Valid)
                        {
                            var discountAcccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AP_DISCOUNT).Select(s => s.AccountId).FirstOrDefault();

                            if (!string.IsNullOrEmpty(discountAcccountId) && IsActiveAccountId(discountAcccountId))
                            {
                                decimal functionalDiscount = CALC.FunctionalAmount(header.IsMultiply, header.DiscountAmount, header.ExchangeRate);

                                var discountJournalDetail = new DistributionJournalDetail()
                                {
                                    DistributionDetailId = Guid.NewGuid(),
                                    DistributionHeaderId = distHeader.DistributionHeaderId,
                                    AccountId = discountAcccountId,
                                    ChargesId = Guid.Empty,
                                    JournalNote = "Discount " + header.DocumentNo,
                                    OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? header.DiscountAmount : 0),
                                    OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : header.DiscountAmount),
                                    FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? functionalDiscount : 0),
                                    FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : functionalDiscount),
                                    Status = DOCSTATUS.NEW,
                                    JournalDate = header.TransactionDate,
                                    BranchCode = header.BranchCode,
                                };

                                distDetails.Add(discountJournalDetail);
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("Posting Setup AR Discount Account No is not valid or empty !");

                            }
                        }

                        //TAX FOOTER
                        decimal originatingTax = 0;

                        if (resp.Valid)
                        {
                            if (!isTaxDetail)
                            {
                                if (taxes.Count > 0)
                                {
                                    var taxDetails = (from det in taxes
                                                      join tax in _context.TaxSchedules on det.TaxScheduleId equals tax.TaxScheduleId
                                                      select new
                                                      {
                                                          AccountId = tax.TaxAccountNo,
                                                          JournalNote = tax.TaxScheduleCode,
                                                          OriginatingTax = det.OriginatingTaxAmount,
                                                          FunctionalTax = CALC.FunctionalAmount(header.IsMultiply, det.OriginatingTaxAmount, header.ExchangeRate, tax),
                                                          TaxSchedule = tax
                                                      }).Where(x => x.OriginatingTax > 0).ToList();

                                    foreach (var tax in taxDetails)
                                    {
                                        if (IsActiveAccountId(tax.AccountId))
                                        {
                                            var taxJournalDetail = new DistributionJournalDetail();

                                            taxJournalDetail.DistributionDetailId = Guid.NewGuid();
                                            taxJournalDetail.DistributionHeaderId = distHeader.DistributionHeaderId;
                                            taxJournalDetail.AccountId = tax.AccountId;
                                            taxJournalDetail.ChargesId = Guid.Empty;
                                            taxJournalDetail.JournalNote = tax.JournalNote + " " + header.DocumentNo;
                                            taxJournalDetail.Status = DOCSTATUS.NEW;
                                            taxJournalDetail.JournalDate = header.TransactionDate;
                                            taxJournalDetail.BranchCode = header.BranchCode;

                                            if (tax.TaxSchedule.WithHoldingTax)
                                            {
                                                taxJournalDetail.OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.OriginatingTax);
                                                taxJournalDetail.OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.OriginatingTax : 0);
                                                taxJournalDetail.FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.FunctionalTax);
                                                taxJournalDetail.FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.FunctionalTax : 0);
                                            }
                                            else
                                            {
                                                taxJournalDetail.OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.OriginatingTax : 0);
                                                taxJournalDetail.OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.OriginatingTax);
                                                taxJournalDetail.FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.FunctionalTax : 0);
                                                taxJournalDetail.FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.FunctionalTax);
                                            }

                                            distDetails.Add(taxJournalDetail);

                                            originatingTax += tax.OriginatingTax;
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("{0} for Tax Schedule is not valid or inactive !", tax.AccountId);
                                            
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (chargeTaxAccount.Count > 0)
                                {
                                    foreach (var tax in chargeTaxAccount)
                                    {
                                        if (IsActiveAccountId(tax.AccountId))
                                        {
                                            decimal functionalTax = CALC.FunctionalAmount(header.IsMultiply, tax.TaxAmount, header.ExchangeRate, tax.TaxSchedule);

                                            var taxJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = tax.AccountId,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Charge Tax " + header.DocumentNo,
                                                OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.TaxAmount),
                                                OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.TaxAmount : 0),
                                                FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : functionalTax),
                                                FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? functionalTax : 0),
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = header.TransactionDate,
                                                BranchCode = header.BranchCode
                                            };

                                            distDetails.Add(taxJournalDetail);

                                            originatingTax += tax.TaxAmount;
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("{0} for Tax Schedule is not valid or inactive !", tax.AccountId);

                                            break;
                                        }
                                    }
                                }
                            }

                        }

                        if (resp.Valid)
                        {
                            decimal originatingAP = Math.Abs(distDetails.Sum(x => x.OriginatingCredit) - distDetails.Sum(x => x.OriginatingDebit));

                            decimal functionalAP = Math.Abs(distDetails.Sum(x => x.FunctionalCredit) - distDetails.Sum(x => x.FunctionalDebit));

                            if (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE) //IF CREDIT NOTE ONLY
                            {
                                string advanceAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AP_ADVANCE_PAYMENT).Select(s => s.AccountId).FirstOrDefault();
                                if (string.IsNullOrEmpty(advanceAccountId))
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("Advance Payment Account must be set !");
                                }

                                if (IsActiveAccountId(advanceAccountId))
                                {
                                    var apJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = advanceAccountId,
                                        ChargesId = Guid.Empty,
                                        JournalNote = header.DocumentNo,
                                        OriginatingDebit = originatingAP,
                                        OriginatingCredit = 0,
                                        FunctionalDebit = functionalAP,
                                        FunctionalCredit = 0,
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = header.TransactionDate,
                                        BranchCode = header.BranchCode,
                                    };

                                    distDetails.Add(apJournalDetail);
                                }
                                else
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("Advance Payment Account is not valid or inactive !");
                                }
                            }
                            else //INVOICE, DEBIT NOTE
                            {
                                var vendor = _context.Vendors.Where(x => x.VendorId == header.VendorId && !string.IsNullOrEmpty(x.PayableAccountNo)).OrderByDescending(o => o.Inactive).FirstOrDefault();

                                if (vendor != null)
                                {
                                    if (IsActiveAccountId(vendor.PayableAccountNo))
                                    {
                                        var arJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = vendor.PayableAccountNo,
                                            ChargesId = Guid.Empty,
                                            JournalNote = header.DocumentNo,
                                            OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? originatingAP : 0),
                                            OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : originatingAP),
                                            FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? functionalAP : 0),
                                            FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : functionalAP),
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = header.TransactionDate,
                                            BranchCode = header.BranchCode,
                                        };

                                        distDetails.Add(arJournalDetail);
                                    }
                                    else
                                    {
                                        resp.Valid = false;
                                        resp.ErrorMessage = string.Format("Payable Account No {0} is not valid or inactive !", vendor.VendorName);
                                    }
                                }
                                else
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("Please check {0} Payable Account No !", vendor.VendorName);
                                }
                            }                                
                        }

                        if (resp.Valid)
                        {
                            _context.DistributionJournalDetails.AddRange(distDetails);
                        }
                        else
                        {
                            resp.Valid = false;
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Header id not found";
                    }
                }
            }

            return resp;
        }

        /// <summary>
        /// Distribution Journal for Receivable Entry
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="taxes"></param>
        /// <param name="includeTransaction"></param>
        /// <returns></returns>
        public async Task<JournalResponse> CreateDistributionJournalAsync(ReceivableTransactionHeader header, List<ReceivableTransactionDetail> details, List<ReceivableTransactionTax> taxes, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (details != null && details.Count > 0)
            {
                if (!string.IsNullOrEmpty(header.DocumentNo))
                {
                    try
                    {
                        DistributionJournalHeader distHeader = new DistributionJournalHeader();

                        var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_RECEIVABLE).FirstOrDefault();

                        if (edit != null)
                        {
                            var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                            //REMOVE EXISTING
                            await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                            //UPDATE HEADER
                            edit.CurrencyCode = header.CurrencyCode;
                            edit.Description = header.Description;
                            edit.DocumentDate = header.TransactionDate;
                            edit.ExchangeRate = header.ExchangeRate;
                            edit.IsMultiply = header.IsMultiply;
                            edit.TrxModule = TRX_MODULE.TRX_RECEIVABLE;
                            edit.TransactionId = header.ReceiveTransactionId;
                            edit.OriginatingTotal = (header.SubtotalAmount - header.DiscountAmount + header.TaxAmount);
                            edit.FunctionalTotal = CALC.FunctionalAmount(header.IsMultiply, edit.OriginatingTotal, header.ExchangeRate);
                            edit.ModifiedBy = header.ModifiedBy;
                            edit.ModifiedDate = DateTime.Now;
                            edit.Status = header.Status;

                            _context.Update(edit);

                            distHeader = edit;
                        }
                        else
                        {
                            //UPDATE HEADER
                            distHeader.DistributionHeaderId = Guid.NewGuid();
                            distHeader.TrxModule = TRX_MODULE.TRX_RECEIVABLE;
                            distHeader.TransactionId = header.ReceiveTransactionId;
                            distHeader.DocumentDate = header.TransactionDate;
                            distHeader.DocumentNo = header.DocumentNo;
                            distHeader.CurrencyCode = header.CurrencyCode;
                            distHeader.ExchangeRate = header.ExchangeRate;
                            distHeader.IsMultiply = header.IsMultiply;
                            distHeader.Description = header.Description;
                            distHeader.OriginatingTotal = (header.SubtotalAmount - header.DiscountAmount + header.TaxAmount);
                            distHeader.FunctionalTotal = CALC.FunctionalAmount(header.IsMultiply, distHeader.OriginatingTotal, header.ExchangeRate);
                            distHeader.CreatedBy = header.CreatedBy;
                            distHeader.CreatedDate = DateTime.Now;
                            distHeader.Status = header.Status;

                            _context.DistributionJournalHeaders.Add(distHeader);

                        }

                        //INSERT NEW DISTRIBUTION JOURNAL
                        if (distHeader.DistributionHeaderId != Guid.Empty)
                        {
                            bool isTaxDetail = true;
                            decimal originatingSubtotal = 0;

                            if (taxes != null)
                            {
                                if (taxes.Count > 0)
                                    isTaxDetail = false;
                            }

                            
                            var recDetails = (from det in details
                                              join ch in _context.Charges on det.ChargesId equals ch.ChargesId
                                              select new ChargeDetail
                                              {
                                                  AccountId = ch.RevenueAccountNo,
                                                  ChargesId = det.ChargesId,
                                                  ChargesName = ch.ChargesName,
                                                  JournalNote = det.ChargesDescription,
                                                  OriginatingAmount = det.OriginatingAmount,
                                                  OriginatingDiscount = det.OriginatingDiscount,
                                                  OriginatingTax = det.OriginatingTax,
                                                  OriginatingExtended = det.OriginatingExtendedAmount,
                                                  FunctionalAmount = CALC.FunctionalAmount(header.IsMultiply, det.OriginatingAmount, header.ExchangeRate),
                                                  FunctionalDiscount = det.FunctionalDiscount,
                                                  FunctionalTax = det.FunctionalTax,
                                                  FunctionalExtended = det.FunctionalExtendedAmount,
                                                  TaxSchedule = (from tax in _context.TaxSchedules
                                                                 where tax.TaxScheduleId == det.TaxScheduleId
                                                                 select tax).FirstOrDefault()
                                              }).ToList();

                            //OBTAIN CUSTOM CHARGE DETAIL ACCOUNT IF FROM NEGOTIATIONSHEET
                            if(!string.IsNullOrEmpty(header.NsDocumentNo))
                            {
                                Guid? shippingLineId = _context.NegotiationSheetHeaders.Where(x => x.DocumentNo.Equals(header.NsDocumentNo, StringComparison.OrdinalIgnoreCase)).Select(ns => ns.ShippingLineId).FirstOrDefault();

                                if(shippingLineId != null && shippingLineId != Guid.Empty)
                                {
                                    foreach(var ch in recDetails)
                                    {
                                        string alternateAccountId = _context.ChargesDetails.Where(x => x.ChargesId == ch.ChargesId && x.ShippingLineId == shippingLineId).Select(sl => sl.RevenueAccountNo).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(alternateAccountId))
                                        {
                                            ch.AccountId = alternateAccountId;
                                        }
                                    }
                                }
                            }

                            List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                            List<TaxScheduleAccount> chargeTaxAccount = new List<TaxScheduleAccount>();

                            foreach (var jed in recDetails)
                            {
                                //CHARGES DETAILS
                                if (!string.IsNullOrEmpty(jed.AccountId) && IsActiveAccountId(jed.AccountId))
                                {
                                    var distributionJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = jed.AccountId,
                                        ChargesId = jed.ChargesId,
                                        JournalNote = jed.JournalNote,
                                        OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? (jed.OriginatingAmount - jed.OriginatingDiscount) : 0),
                                        OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : (jed.OriginatingAmount - jed.OriginatingDiscount)),
                                        FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? (jed.FunctionalAmount - jed.FunctionalDiscount) : 0),
                                        FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : (jed.FunctionalAmount - jed.FunctionalDiscount)),
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = header.TransactionDate,
                                        BranchCode = header.BranchCode,
                                    };

                                    distDetails.Add(distributionJournalDetail);

                                    originatingSubtotal += (jed.OriginatingAmount - jed.OriginatingDiscount);

                                    if (isTaxDetail)
                                    {
                                        //TAX DETAILS
                                        if (jed.OriginatingTax > 0)
                                        {
                                            if(jed.TaxSchedule != null)
                                            {
                                                if (IsActiveAccountId(jed.TaxSchedule.TaxAccountNo))
                                                {
                                                    var taxAccount = chargeTaxAccount.Where(x => x.AccountId.Equals(jed.TaxSchedule.TaxAccountNo)).FirstOrDefault();

                                                    if (taxAccount == null)
                                                    {
                                                        chargeTaxAccount.Add(new TaxScheduleAccount { AccountId = jed.TaxSchedule.TaxAccountNo, TaxAmount = jed.OriginatingTax, TaxSchedule = jed.TaxSchedule });
                                                    }
                                                    else
                                                    {
                                                        taxAccount.TaxAmount += jed.OriginatingTax;
                                                    }
                                                }
                                                else
                                                {
                                                    resp.Valid = false;
                                                    resp.ErrorMessage = string.Format("{0} Account is not valid or inactive !", jed.TaxSchedule.TaxScheduleCode);

                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                resp.Valid = false;
                                                resp.ErrorMessage = string.Format("{0} Tax Schedule No is not valid !", jed.ChargesName);

                                                break;
                                            }                                            
                                        }
                                    }
                                }
                                else
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("{0} Revenue Account No is not valid or empty !", jed.ChargesName);

                                    break;
                                }
                            }

                            //DISCOUNT FOOTER
                            if (header.DiscountAmount > 0 && resp.Valid)
                            {
                                var discountAcccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AR_DISCOUNT).Select(s => s.AccountId).FirstOrDefault();

                                if (!string.IsNullOrEmpty(discountAcccountId) && IsActiveAccountId(discountAcccountId))
                                {
                                    decimal functionalDiscount = CALC.FunctionalAmount(header.IsMultiply, header.DiscountAmount, header.ExchangeRate);

                                    var discountJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = discountAcccountId,
                                        ChargesId = Guid.Empty,
                                        JournalNote = "Discount " + header.DocumentNo,
                                        OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : header.DiscountAmount),
                                        OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? header.DiscountAmount : 0),
                                        FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : functionalDiscount),
                                        FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? functionalDiscount : 0),
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = header.TransactionDate,
                                        BranchCode = header.BranchCode,
                                    };

                                    distDetails.Add(discountJournalDetail);
                                }
                                else
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("Posting Setup AR Discount Account No is not valid or empty !");
                                }
                            }

                            //TAX FOOTER
                            decimal originatingTax = 0;

                            if (resp.Valid)
                            {
                                if (!isTaxDetail)
                                {
                                    if (taxes.Count > 0)
                                    {
                                        var taxDetails = (from det in taxes
                                                          join tax in _context.TaxSchedules on det.TaxScheduleId equals tax.TaxScheduleId
                                                          select new
                                                          {
                                                              AccountId = tax.TaxAccountNo,
                                                              JournalNote = tax.TaxScheduleCode,
                                                              OriginatingTax = det.OriginatingTaxAmount,
                                                              FunctionalTax = CALC.FunctionalAmount(header.IsMultiply, det.OriginatingTaxAmount, header.ExchangeRate, tax),
                                                              TaxSchedule = tax
                                                          }).Where(x => x.OriginatingTax > 0).ToList();

                                        foreach (var tax in taxDetails)
                                        {
                                            if (IsActiveAccountId(tax.AccountId))
                                            {
                                                var taxJournalDetail = new DistributionJournalDetail();

                                                taxJournalDetail.DistributionDetailId = Guid.NewGuid();
                                                taxJournalDetail.DistributionHeaderId = distHeader.DistributionHeaderId;
                                                taxJournalDetail.AccountId = tax.AccountId;
                                                taxJournalDetail.ChargesId = Guid.Empty;
                                                taxJournalDetail.JournalNote = tax.JournalNote + " " + header.DocumentNo;
                                                taxJournalDetail.Status = DOCSTATUS.NEW;
                                                taxJournalDetail.JournalDate = header.TransactionDate;
                                                taxJournalDetail.BranchCode = header.BranchCode;

                                                if (tax.TaxSchedule.WithHoldingTax)
                                                {
                                                    taxJournalDetail.OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.OriginatingTax);
                                                    taxJournalDetail.OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.OriginatingTax : 0);
                                                    taxJournalDetail.FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.FunctionalTax);
                                                    taxJournalDetail.FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.FunctionalTax : 0);
                                                }
                                                else
                                                {
                                                    taxJournalDetail.OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.OriginatingTax : 0);
                                                    taxJournalDetail.OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.OriginatingTax);
                                                    taxJournalDetail.FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.FunctionalTax : 0);
                                                    taxJournalDetail.FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.FunctionalTax);
                                                }

                                                distDetails.Add(taxJournalDetail);

                                                originatingTax += tax.OriginatingTax;
                                            }
                                            else
                                            {
                                                resp.Valid = false;
                                                resp.ErrorMessage = string.Format("{0} for Tax Schedule is not valid or inactive !", tax.AccountId);

                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (chargeTaxAccount.Count > 0)
                                    {
                                        foreach (var tax in chargeTaxAccount)
                                        {
                                            if (IsActiveAccountId(tax.AccountId))
                                            {
                                                decimal functionalTax = CALC.FunctionalAmount(header.IsMultiply, tax.TaxAmount, header.ExchangeRate, tax.TaxSchedule);

                                                var taxJournalDetail = new DistributionJournalDetail()
                                                {
                                                    DistributionDetailId = Guid.NewGuid(),
                                                    DistributionHeaderId = distHeader.DistributionHeaderId,
                                                    AccountId = tax.AccountId,
                                                    ChargesId = Guid.Empty,
                                                    JournalNote = "Charge Tax " + header.DocumentNo,
                                                    OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? tax.TaxAmount : 0),
                                                    OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : tax.TaxAmount),
                                                    FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? functionalTax : 0),
                                                    FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : functionalTax),
                                                    Status = DOCSTATUS.NEW,
                                                    JournalDate = header.TransactionDate,
                                                    BranchCode = header.BranchCode,
                                                };

                                                distDetails.Add(taxJournalDetail);

                                                originatingTax += tax.TaxAmount;
                                            }
                                            else
                                            {
                                                resp.Valid = false;
                                                resp.ErrorMessage = string.Format("{0} for Tax Schedule is not valid or inactive !", tax.AccountId);

                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            //AR 
                            if (resp.Valid)
                            {
                                decimal originatingAR = Math.Abs(distDetails.Sum(x => x.OriginatingDebit) - distDetails.Sum(x => x.OriginatingCredit));

                                decimal functionalAR = Math.Abs(distDetails.Sum(x => x.FunctionalDebit) - distDetails.Sum(x => x.FunctionalCredit));

                                if (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE) //IF CREDIT NOTE ONLY
                                {
                                    string advanceAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AR_ADVANCE_RECEIPT).Select(s => s.AccountId).FirstOrDefault();
                                    if (string.IsNullOrEmpty(advanceAccountId))
                                    {
                                        resp.Valid = false;
                                        resp.ErrorMessage = string.Format("Advance Receipt Account must be set !");
                                    }

                                    if (IsActiveAccountId(advanceAccountId))
                                    {
                                        var arJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = advanceAccountId,
                                            ChargesId = Guid.Empty,
                                            JournalNote = header.DocumentNo,
                                            OriginatingDebit = 0,
                                            OriginatingCredit = originatingAR,
                                            FunctionalDebit = 0,
                                            FunctionalCredit = functionalAR,
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = header.TransactionDate,
                                            BranchCode = header.BranchCode,
                                        };

                                        distDetails.Add(arJournalDetail);
                                    }
                                    else
                                    {
                                        resp.Valid = false;
                                        resp.ErrorMessage = string.Format("Advance Receipt Account is not valid or inactive !");
                                    }
                                }
                                else //INVOICE , DEBIT NOTE
                                {
                                    var customer = _context.Customers.Where(x => x.CustomerId == header.CustomerId && !string.IsNullOrEmpty(x.ReceivableAccountNo)).OrderByDescending(o => o.Inactive).FirstOrDefault();

                                    if (customer != null)
                                    {
                                        if (IsActiveAccountId(customer.ReceivableAccountNo))
                                        {
                                            var arJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = customer.ReceivableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = header.DocumentNo,
                                                OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : originatingAR),
                                                OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? originatingAR : 0),
                                                FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? 0 : functionalAR),
                                                FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CREDIT_NOTE ? functionalAR : 0),
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = header.TransactionDate,
                                                BranchCode = header.BranchCode,
                                            };

                                            distDetails.Add(arJournalDetail);
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("Receivable Account No {0} is not valid or inactive !", customer.CustomerName);

                                        }
                                    }
                                    else
                                    {
                                        resp.Valid = false;
                                        resp.ErrorMessage = string.Format("Please check {0} Receivable Account No", customer.CustomerName);
                                    }
                                }                                
                            }

                            if (resp.Valid)
                            {
                                _context.DistributionJournalDetails.AddRange(distDetails);
                            }
                            else
                            {
                                resp.Valid = false;
                            }
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = "Header id not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = ex.Message;
                    }                                    
                }
            }

            return resp;
        }

        /// <summary>
        /// Distribution Journal for Checkbook Entry
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="includeTransaction"></param>
        /// <returns></returns>
        public async Task<JournalResponse> CreateDistributionJournalAsync(CheckbookTransactionHeader header, List<CheckbookTransactionDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (details != null && details.Count > 0)
            {
                if (!string.IsNullOrEmpty(header.DocumentNo))
                {
                    var checkbook = _context.Checkbooks.Where(x => x.CheckbookCode.Equals(header.CheckbookCode) && !string.IsNullOrEmpty(x.CheckbookAccountNo)).FirstOrDefault();

                    if (checkbook != null)
                    {
                        if (IsActiveAccountId(checkbook.CheckbookAccountNo))
                        {
                            DistributionJournalHeader distHeader = new DistributionJournalHeader();

                            var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_CHECKBOOK).FirstOrDefault();

                            if (edit != null)
                            {
                                var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                                //REMOVE EXISTING
                                await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                                //UPDATE HEADER
                                edit.CurrencyCode = header.CurrencyCode;
                                edit.Description = header.Description;
                                edit.DocumentDate = header.TransactionDate;
                                edit.ExchangeRate = header.ExchangeRate;
                                edit.IsMultiply = header.IsMultiply;
                                edit.TransactionId = header.CheckbookTransactionId;
                                edit.OriginatingTotal = header.OriginatingTotalAmount;
                                edit.FunctionalTotal = header.FunctionalTotalAmount;
                                edit.ModifiedBy = header.ModifiedBy;
                                edit.ModifiedDate = DateTime.Now;
                                edit.Status = header.Status;

                                _context.Update(edit);

                                distHeader = edit;
                            }
                            else
                            {
                                //UPDATE HEADER
                                distHeader.DistributionHeaderId = Guid.NewGuid();
                                distHeader.TrxModule = TRX_MODULE.TRX_CHECKBOOK;
                                distHeader.TransactionId = header.CheckbookTransactionId;
                                distHeader.DocumentDate = header.TransactionDate;
                                distHeader.DocumentNo = header.DocumentNo;
                                distHeader.CurrencyCode = header.CurrencyCode;
                                distHeader.ExchangeRate = header.ExchangeRate;
                                distHeader.IsMultiply = header.IsMultiply;
                                distHeader.Description = header.Description;
                                distHeader.OriginatingTotal = header.OriginatingTotalAmount;
                                distHeader.FunctionalTotal = header.FunctionalTotalAmount;
                                distHeader.CreatedBy = header.CreatedBy;
                                distHeader.CreatedDate = DateTime.Now;
                                distHeader.Status = header.Status;

                                _context.DistributionJournalHeaders.Add(distHeader);

                            }

                            //INSERT NEW DISTRIBUTION JOURNAL
                            if (distHeader.DistributionHeaderId != Guid.Empty)
                            {
                                var recDetails = (from det in details
                                                  join ch in _context.Charges on det.ChargesId equals ch.ChargesId
                                                  select new
                                                  {
                                                      RevenueAccountId = ch.RevenueAccountNo,
                                                      CostAccountId = ch.CostAccountNo,
                                                      ChargesId = det.ChargesId,
                                                      ChargesName = ch.ChargesName,
                                                      JournalNote = det.ChargesDescription,
                                                      OriginatingAmount = det.OriginatingAmount,
                                                      FunctionalAmount = CALC.FunctionalAmount(header.IsMultiply, det.OriginatingAmount, header.ExchangeRate)
                                                  }).ToList();

                                List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();
                                foreach (var jed in recDetails)
                                {
                                    if (header.DocumentType == DOCUMENTTYPE.CHECKBOOK_IN)
                                    {
                                        if (!string.IsNullOrEmpty(jed.RevenueAccountId) && IsActiveAccountId(jed.RevenueAccountId))
                                        {
                                            var distributionJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = jed.RevenueAccountId,
                                                ChargesId = Guid.Empty,
                                                JournalNote = jed.JournalNote,
                                                OriginatingDebit = 0,
                                                OriginatingCredit = jed.OriginatingAmount,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = jed.FunctionalAmount,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = header.TransactionDate,
                                                BranchCode = header.BranchCode,
                                            };

                                            distDetails.Add(distributionJournalDetail);
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("Charges {0} Revenue Account No is not valid or inactive !", jed.ChargesName);

                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(jed.CostAccountId) && IsActiveAccountId(jed.CostAccountId))
                                        {
                                            var distributionJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = jed.CostAccountId,
                                                ChargesId = Guid.Empty,
                                                JournalNote = jed.JournalNote,
                                                OriginatingDebit = jed.OriginatingAmount,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = jed.FunctionalAmount,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = header.TransactionDate,
                                                BranchCode = header.BranchCode,
                                            };

                                            distDetails.Add(distributionJournalDetail);
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("Charges {0} Cost Account No is not valid or ianctive !", jed.ChargesName);

                                            break;
                                        }
                                    }
                                }

                                if (resp.Valid)
                                {
                                    decimal originatingTotal = recDetails.Sum(x => x.OriginatingAmount);
                                    decimal functionalTotal = recDetails.Sum(x => x.FunctionalAmount);

                                    var arJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = checkbook.CheckbookAccountNo,
                                        ChargesId = Guid.Empty,
                                        JournalNote = header.DocumentNo,
                                        OriginatingDebit = (header.DocumentType == DOCUMENTTYPE.CHECKBOOK_IN ? originatingTotal : 0),
                                        OriginatingCredit = (header.DocumentType == DOCUMENTTYPE.CHECKBOOK_IN ? 0 : originatingTotal),
                                        FunctionalDebit = (header.DocumentType == DOCUMENTTYPE.CHECKBOOK_IN ? functionalTotal : 0),
                                        FunctionalCredit = (header.DocumentType == DOCUMENTTYPE.CHECKBOOK_IN ? 0 : functionalTotal),
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = header.TransactionDate,
                                        BranchCode = header.BranchCode,
                                    };

                                    distDetails.Add(arJournalDetail);

                                    _context.DistributionJournalDetails.AddRange(distDetails);
                                }
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = "Header GUID can not be generated !";
                            }
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Checkbook {0} Account No is not valid or inactive !", checkbook.CheckbookCode);
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = string.Format("Checkbook {0} Account No must be specified.", header.CheckbookCode);
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Document No is not valid";
                }
            }

            return resp;
        }

        public async Task<JournalResponse> CreateDistributionJournalAsync(APApplyHeader header, List<APApplyDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if(details != null && details.Count > 0)
            {
                if (!string.IsNullOrEmpty(header.DocumentNo))
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    string creditCurrencyCode = "";
                    decimal creditExchangeRate = 1;
                    bool creditIsMultiply = true;
                    Currency trxCurrency = null;

                    if (header.DocumentType.Equals(DOCUMENTTYPE.ADVANCE, StringComparison.OrdinalIgnoreCase))
                    {
                        var advance = _context.CheckbookTransactionHeaders.Where(x => x.CheckbookTransactionId == header.CheckbookTransactionId).FirstOrDefault();
                        if (advance != null)
                        {
                            creditCurrencyCode = advance.CurrencyCode;
                            creditExchangeRate = advance.ExchangeRate;
                            creditIsMultiply = advance.IsMultiply;

                            trxCurrency = _context.Currencies.Where(x => x.CurrencyCode.ToUpper() == advance.CurrencyCode.ToUpper()).FirstOrDefault();
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Advance Payment not exist !");
                        }
                    }
                    else if (header.DocumentType.Equals(DOCUMENTTYPE.PAYMENT, StringComparison.OrdinalIgnoreCase))
                    {
                        var unallocPV = _context.ApPaymentHeaders.Where(x => x.PaymentHeaderId == header.PaymentHeaderId).FirstOrDefault();
                        if (unallocPV != null)
                        {
                            creditCurrencyCode = unallocPV.CurrencyCode;
                            creditExchangeRate = unallocPV.ExchangeRate;
                            creditIsMultiply = unallocPV.IsMultiply;

                            trxCurrency = _context.Currencies.Where(x => x.CurrencyCode.Equals(unallocPV.CurrencyCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Official Receipt not exist !");
                        }
                    }
                    else
                    {
                        var cn = _context.PayableTransactionHeaders.Where(x => x.PayableTransactionId == header.PayableTransactionId).FirstOrDefault();
                        if (cn != null)
                        {
                            creditCurrencyCode = cn.CurrencyCode;
                            creditExchangeRate = cn.ExchangeRate;
                            creditIsMultiply = cn.IsMultiply;

                            trxCurrency = _context.Currencies.Where(x => x.CurrencyCode.ToUpper() == cn.CurrencyCode.ToUpper()).FirstOrDefault();
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Credit Note not exist !");
                        }
                    }

                    if (resp.Valid)
                    {
                        var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_APPLY_PAYABLE).FirstOrDefault();

                        if (edit != null)
                        {
                            var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                            //REMOVE EXISTING
                            await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                            //UPDATE HEADER
                            edit.CurrencyCode = creditCurrencyCode;
                            edit.Description = header.Description;
                            edit.DocumentDate = header.TransactionDate;
                            edit.ExchangeRate = creditExchangeRate;
                            edit.IsMultiply = creditIsMultiply;
                            edit.TransactionId = header.PayableApplyId;
                            edit.OriginatingTotal = header.OriginatingTotalPaid;
                            edit.FunctionalTotal = header.FunctionalTotalPaid;
                            edit.ModifiedBy = header.ModifiedBy;
                            edit.ModifiedDate = DateTime.Now;
                            edit.Status = header.Status;

                            _context.Update(edit);

                            distHeader = edit;
                        }
                        else
                        {
                            //UPDATE HEADER
                            distHeader.DistributionHeaderId = Guid.NewGuid();
                            distHeader.TrxModule = TRX_MODULE.TRX_APPLY_PAYABLE;
                            distHeader.TransactionId = header.PayableApplyId;
                            distHeader.DocumentDate = header.TransactionDate;
                            distHeader.DocumentNo = header.DocumentNo;
                            distHeader.CurrencyCode = creditCurrencyCode;
                            distHeader.ExchangeRate = creditExchangeRate;
                            distHeader.IsMultiply = creditIsMultiply;
                            distHeader.Description = header.Description;
                            distHeader.OriginatingTotal = header.OriginatingTotalPaid;
                            distHeader.FunctionalTotal = header.FunctionalTotalPaid;
                            distHeader.CreatedBy = header.CreatedBy;
                            distHeader.CreatedDate = DateTime.Now;
                            distHeader.Status = header.Status;

                            _context.DistributionJournalHeaders.Add(distHeader);

                        }

                        //AP Account
                        var vendor = _context.Vendors.Where(x => x.VendorId == header.VendorId && !string.IsNullOrEmpty(x.PayableAccountNo)).OrderByDescending(o => o.Inactive).FirstOrDefault();

                        if (vendor != null && distHeader.DistributionHeaderId != Guid.Empty)
                        {
                            //INSERT NEW DISTRIBUTION JOURNAL
                            if (IsActiveAccountId(vendor.PayableAccountNo))
                            {
                                List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                                if (resp.Valid)
                                {
                                    if (header.DocumentType.Equals(DOCUMENTTYPE.ADVANCE, StringComparison.OrdinalIgnoreCase) ||
                                        header.DocumentType.Equals(DOCUMENTTYPE.PAYMENT, StringComparison.OrdinalIgnoreCase) ||
                                        header.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        string advancePaymentAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AP_ADVANCE_PAYMENT).Select(s => s.AccountId).FirstOrDefault();

                                        if (!string.IsNullOrEmpty(advancePaymentAccountId) && IsActiveAccountId(advancePaymentAccountId))
                                        {
                                            var apJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = vendor.PayableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Apply Payable #" + header.DocumentNo,
                                                OriginatingDebit = distHeader.OriginatingTotal,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = distHeader.FunctionalTotal,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate                                             
                                            };

                                            distDetails.Add(apJournalDetail);

                                            var advanceDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = advancePaymentAccountId,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Apply Payable #" + header.DocumentNo,
                                                OriginatingDebit = 0,
                                                OriginatingCredit = distHeader.OriginatingTotal,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = distHeader.FunctionalTotal,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(advanceDetail);
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("Posting Setup - Advance Payment account is not valid or inactive !");
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(trxCurrency.RealizedGainAcc) || string.IsNullOrEmpty(trxCurrency.RealizedLossAcc))
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("Realized Gain and Realized Loss Account must be set !");
                                }

                                if (resp.Valid)
                                {
                                    //REALIZED GAIN / LOSS
                                    decimal totalFunctionalRateGainLoss = 0;

                                    var applyDetails = (from det in details
                                                        join pay in _context.PayableTransactionHeaders on det.PayableTransactionId equals pay.PayableTransactionId
                                                        select new
                                                        {
                                                            PayableTransactionId = det.PayableTransactionId,
                                                            DocumentNo = pay.DocumentNo,
                                                            ExchangeRate = pay.ExchangeRate,
                                                            IsMultiply = pay.IsMultiply,
                                                            OriginatingPaid = det.OriginatingPaid,
                                                            FunctionalPaid = CALC.FunctionalAmount(pay.IsMultiply, det.OriginatingPaid, pay.ExchangeRate)
                                                        }).AsQueryable().ToList();

                                    foreach (var jed in applyDetails)
                                    {
                                        decimal functionalApply = CALC.FunctionalAmount(jed.IsMultiply, jed.OriginatingPaid, creditExchangeRate);

                                        totalFunctionalRateGainLoss += (functionalApply - jed.FunctionalPaid);
                                    }

                                    if (totalFunctionalRateGainLoss != 0)
                                    {
                                        if (totalFunctionalRateGainLoss > 0)
                                        {
                                            //EXCHANGE RATE LOSS
                                            //Realized Loss to AP  
                                            var lossJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = trxCurrency.RealizedLossAcc,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Loss",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = totalFunctionalRateGainLoss,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(lossJournalDetail);

                                            //OBTAIN GAIN LOSS ACCOUNT

                                            var apJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = vendor.PayableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Loss",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = totalFunctionalRateGainLoss,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(apJournalDetail);
                                        }
                                        else
                                        {
                                            //EXCHANGE RATE GAIN
                                            //AP to Realized Gain
                                            totalFunctionalRateGainLoss = Math.Abs(totalFunctionalRateGainLoss);

                                            var apJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = vendor.PayableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Gain",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = totalFunctionalRateGainLoss,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(apJournalDetail);

                                            //OBTAIN GAIN LOSS ACCOUNT
                                            var gainJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = trxCurrency.RealizedGainAcc,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Gain",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = totalFunctionalRateGainLoss,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(gainJournalDetail);
                                        }
                                    }
                                }

                                if (resp.Valid)
                                {
                                    _context.DistributionJournalDetails.AddRange(distDetails);
                                }
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("{0} Payable Account No is not valid or inactive !", vendor.VendorCode);
                            }
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Please check {0} Payable Account No", vendor.VendorName);
                        }
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Document No is not valid";
                }
            }

            return resp;
        }

        public async Task<JournalResponse> CreateDistributionJournalAsync(ARApplyHeader header, List<ARApplyDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (details != null && details.Count > 0)
            {
                if (!string.IsNullOrEmpty(header.DocumentNo))
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    string creditCurrencyCode = "";
                    decimal creditExchangeRate = 1;
                    bool creditIsMultiply = true;
                    Currency trxCurrency = null;

                    if (header.DocumentType.Equals(DOCUMENTTYPE.ADVANCE, StringComparison.OrdinalIgnoreCase))
                    {
                        var advance = _context.CheckbookTransactionHeaders.Where(x => x.CheckbookTransactionId == header.CheckbookTransactionId).FirstOrDefault();
                        if (advance != null)
                        {
                            creditCurrencyCode = advance.CurrencyCode;
                            creditExchangeRate = advance.ExchangeRate;
                            creditIsMultiply = advance.IsMultiply;

                            trxCurrency = _context.Currencies.Where(x => x.CurrencyCode.Equals(advance.CurrencyCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Advance Receipt not exist !");
                        }
                    }
                    else if (header.DocumentType.Equals(DOCUMENTTYPE.RECEIPT, StringComparison.OrdinalIgnoreCase))
                    {
                        var unallocRV = _context.ArReceiptHeaders.Where(x => x.ReceiptHeaderId == header.ReceiptHeaderId).FirstOrDefault();
                        if (unallocRV != null)
                        {
                            creditCurrencyCode = unallocRV.CurrencyCode;
                            creditExchangeRate = unallocRV.ExchangeRate;
                            creditIsMultiply = unallocRV.IsMultiply;

                            trxCurrency = _context.Currencies.Where(x => x.CurrencyCode.Equals(unallocRV.CurrencyCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Official Receipt not exist !");
                        }
                    }
                    else
                    {
                        var cn = _context.ReceivableTransactionHeaders.Where(x => x.ReceiveTransactionId == header.ReceiveTransactionId).FirstOrDefault();
                        if (cn != null)
                        {
                            creditCurrencyCode = cn.CurrencyCode;
                            creditExchangeRate = cn.ExchangeRate;
                            creditIsMultiply = cn.IsMultiply;

                            trxCurrency = _context.Currencies.Where(x => x.CurrencyCode.Equals(cn.CurrencyCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Credit Note not exist !");
                        }
                    }

                    if (resp.Valid)
                    {
                        var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_APPLY_RECEIPT).FirstOrDefault();

                        if (edit != null)
                        {
                            var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                            //REMOVE EXISTING
                            await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                            //UPDATE HEADER
                            edit.CurrencyCode = creditCurrencyCode;
                            edit.Description = header.Description;
                            edit.DocumentDate = header.TransactionDate;
                            edit.ExchangeRate = creditExchangeRate;
                            edit.IsMultiply = creditIsMultiply;
                            edit.TransactionId = header.ReceivableApplyId;
                            edit.OriginatingTotal = header.OriginatingTotalPaid;
                            edit.FunctionalTotal = header.FunctionalTotalPaid;
                            edit.ModifiedBy = header.ModifiedBy;
                            edit.ModifiedDate = DateTime.Now;
                            edit.Status = header.Status;

                            _context.Update(edit);

                            distHeader = edit;
                        }
                        else
                        {
                            //UPDATE HEADER
                            distHeader.DistributionHeaderId = Guid.NewGuid();
                            distHeader.TrxModule = TRX_MODULE.TRX_APPLY_RECEIPT;
                            distHeader.TransactionId = header.ReceivableApplyId;
                            distHeader.DocumentDate = header.TransactionDate;
                            distHeader.DocumentNo = header.DocumentNo;
                            distHeader.CurrencyCode = creditCurrencyCode;
                            distHeader.ExchangeRate = creditExchangeRate;
                            distHeader.IsMultiply = creditIsMultiply;
                            distHeader.Description = header.Description;
                            distHeader.OriginatingTotal = header.OriginatingTotalPaid;
                            distHeader.FunctionalTotal = header.FunctionalTotalPaid;
                            distHeader.CreatedBy = header.CreatedBy;
                            distHeader.CreatedDate = DateTime.Now;
                            distHeader.Status = header.Status;

                            _context.DistributionJournalHeaders.Add(distHeader);
                        }

                        //AR Account
                        var customer = _context.Customers.Where(x => x.CustomerId == header.CustomerId && !string.IsNullOrEmpty(x.ReceivableAccountNo)).OrderByDescending(o => o.Inactive).FirstOrDefault();

                        if (customer != null && distHeader.DistributionHeaderId != Guid.Empty)
                        {
                            //INSERT NEW DISTRIBUTION JOURNAL
                            if (IsActiveAccountId(customer.ReceivableAccountNo))
                            {
                                List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                                if (resp.Valid)
                                {
                                    string advanceReceiptAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AR_ADVANCE_RECEIPT).Select(s => s.AccountId).FirstOrDefault();

                                    if (header.DocumentType.Equals(DOCUMENTTYPE.ADVANCE, StringComparison.OrdinalIgnoreCase) ||
                                        header.DocumentType.Equals(DOCUMENTTYPE.RECEIPT, StringComparison.OrdinalIgnoreCase) ||
                                        header.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (!string.IsNullOrEmpty(advanceReceiptAccountId))
                                        {
                                            var apJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = customer.ReceivableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Apply Receivable #" + header.DocumentNo,
                                                OriginatingDebit = 0,
                                                OriginatingCredit = distHeader.OriginatingTotal,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = distHeader.FunctionalTotal,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(apJournalDetail);

                                            var advanceDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = advanceReceiptAccountId,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Apply Receivable #" + header.DocumentNo,
                                                OriginatingDebit = distHeader.OriginatingTotal,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = distHeader.FunctionalTotal,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(advanceDetail);
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("Posting Setup - Advance Receipt account is not set !");
                                        }
                                    }else if(header.DocumentType.Equals(DOCUMENTTYPE.DEPOSIT_SETTLEMENT, StringComparison.OrdinalIgnoreCase))
                                    {
                                        string advanceDepositAccountId = (from dd in _context.ReceivableTransactionDetails
                                                                          join ch in _context.Charges on dd.ChargesId equals ch.ChargesId
                                                                          join hh in _context.DepositSettlementHeaders on dd.ReceiveTransactionId equals hh.ReceiveTransactionId
                                                                          where hh.ReceiveTransactionId == header.ReceiveTransactionId
                                                                          select ch.RevenueAccountNo
                                                                          ).FirstOrDefault() ?? "";

                                        if (!string.IsNullOrEmpty(advanceDepositAccountId) && IsActiveAccountId(advanceDepositAccountId))
                                        {
                                            var advanceDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = advanceDepositAccountId,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Apply Deposit #" + header.DocumentNo,
                                                OriginatingDebit = distHeader.OriginatingTotal,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = distHeader.FunctionalTotal,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(advanceDetail);

                                            var arJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = customer.ReceivableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Apply Deposit #" + header.DocumentNo,
                                                OriginatingDebit = 0,
                                                OriginatingCredit = distHeader.OriginatingTotal,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = distHeader.FunctionalTotal,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(arJournalDetail);
                                        }
                                        else
                                        {
                                            resp.Valid = false;
                                            resp.ErrorMessage = string.Format("Posting Setup - Advance Deposit account is not set !");
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(trxCurrency.RealizedGainAcc) || string.IsNullOrEmpty(trxCurrency.RealizedLossAcc))
                                {
                                    resp.Valid = false;
                                    resp.ErrorMessage = string.Format("Realized Gain and Realized Loss Account must be set !");
                                }

                                if (resp.Valid)
                                {
                                    //REALIZED GAIN / LOSS
                                    decimal totalFunctionalRateGainLoss = 0;

                                    var applyDetails = (from det in details
                                                        join pay in _context.ReceivableTransactionHeaders on det.ReceiveTransactionId equals pay.ReceiveTransactionId
                                                        select new
                                                        {
                                                            ReceiveTransactionId = det.ReceiveTransactionId,
                                                            DocumentNo = pay.DocumentNo,
                                                            ExchangeRate = pay.ExchangeRate,
                                                            IsMultiply = pay.IsMultiply,
                                                            OriginatingPaid = det.OriginatingPaid,
                                                            FunctionalPaid = CALC.FunctionalAmount(pay.IsMultiply, det.OriginatingPaid, pay.ExchangeRate)
                                                        }).AsQueryable().ToList();

                                    foreach (var jed in applyDetails)
                                    {
                                        decimal functionalApply = CALC.FunctionalAmount(jed.IsMultiply, jed.OriginatingPaid, creditExchangeRate);

                                        totalFunctionalRateGainLoss += (functionalApply - jed.FunctionalPaid);
                                    }

                                    if (totalFunctionalRateGainLoss != 0)
                                    {
                                        if (totalFunctionalRateGainLoss > 0)
                                        {
                                            //EXCHANGE RATE LOSS
                                            //Realized Gain to AR  
                                            var gainJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = trxCurrency.RealizedGainAcc,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Gain",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = totalFunctionalRateGainLoss,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(gainJournalDetail);

                                            //OBTAIN GAIN LOSS ACCOUNT

                                            var arJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = customer.ReceivableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Gain",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = totalFunctionalRateGainLoss,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(arJournalDetail);
                                        }
                                        else
                                        {
                                            //EXCHANGE RATE GAIN
                                            //AP to Realized Loss
                                            totalFunctionalRateGainLoss = Math.Abs(totalFunctionalRateGainLoss);

                                            var arJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = customer.ReceivableAccountNo,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Loss",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = 0,
                                                FunctionalCredit = totalFunctionalRateGainLoss,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(arJournalDetail);

                                            //OBTAIN GAIN LOSS ACCOUNT
                                            var lossJournalDetail = new DistributionJournalDetail()
                                            {
                                                DistributionDetailId = Guid.NewGuid(),
                                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                                AccountId = trxCurrency.RealizedLossAcc,
                                                ChargesId = Guid.Empty,
                                                JournalNote = "Exchange Rate Loss",
                                                OriginatingDebit = 0,
                                                OriginatingCredit = 0,
                                                FunctionalDebit = totalFunctionalRateGainLoss,
                                                FunctionalCredit = 0,
                                                Status = DOCSTATUS.NEW,
                                                JournalDate = distHeader.DocumentDate
                                            };

                                            distDetails.Add(lossJournalDetail);
                                        }
                                    }
                                }

                                if (resp.Valid)
                                {
                                    _context.DistributionJournalDetails.AddRange(distDetails);
                                }
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("{0} Receivable Account No is not valid or inactive !", customer.CustomerName);
                            }
                        }
                        else
                        {
                            resp.Valid = false;
                            resp.ErrorMessage = string.Format("Please check {0} Receivable Account No", customer.CustomerName);
                        }
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Document No is not valid";
                }
            }

            return resp;
        }

        public async Task<JournalResponse> CreateDistributionJournalAsync(ApPaymentHeader header, List<ApPaymentDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (!string.IsNullOrEmpty(header.DocumentNo))
            {
                StringBuilder errorMsg = new StringBuilder();

                var cb = _context.Checkbooks.Where(x => x.CheckbookCode == header.CheckbookCode).FirstOrDefault();
                if (cb == null)
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Checkbook {0} must be set !", header.CheckbookCode));
                }

                var vendor = _context.Vendors.Where(x => x.VendorId == header.VendorId).FirstOrDefault();
                if (string.IsNullOrEmpty(vendor.PayableAccountNo))
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Vendor {0} Payable Account No must be set !", header.VendorCode));
                }

                string advanceAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AP_ADVANCE_PAYMENT).Select(s => s.AccountId).FirstOrDefault();
                if (string.IsNullOrEmpty(advanceAccountId))
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Advance Payment Account must be set !"));
                }

                if (resp.Valid)
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_PAYMENT).FirstOrDefault();

                    if (edit != null)
                    {
                        var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                        //REMOVE EXISTING
                        await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                        //UPDATE HEADER
                        edit.CurrencyCode = header.CurrencyCode;
                        edit.Description = header.Description;
                        edit.DocumentDate = header.TransactionDate;
                        edit.ExchangeRate = header.ExchangeRate;
                        edit.IsMultiply = header.IsMultiply;
                        edit.TransactionId = header.PaymentHeaderId;
                        edit.OriginatingTotal = header.OriginatingTotalPaid;
                        edit.FunctionalTotal = header.FunctionalTotalPaid;
                        edit.ModifiedBy = header.ModifiedBy;
                        edit.ModifiedDate = DateTime.Now;
                        edit.Status = header.Status;

                        _context.Update(edit);

                        distHeader = edit;
                    }
                    else
                    {
                        //UPDATE HEADER
                        distHeader.DistributionHeaderId = Guid.NewGuid();
                        distHeader.TrxModule = TRX_MODULE.TRX_PAYMENT;
                        distHeader.TransactionId = header.VendorId;
                        distHeader.DocumentDate = header.TransactionDate;
                        distHeader.DocumentNo = header.DocumentNo;
                        distHeader.CurrencyCode = header.CurrencyCode;
                        distHeader.ExchangeRate = header.ExchangeRate;
                        distHeader.IsMultiply = header.IsMultiply;
                        distHeader.Description = header.Description;
                        distHeader.OriginatingTotal = header.OriginatingTotalPaid;
                        distHeader.FunctionalTotal = header.FunctionalTotalPaid;
                        distHeader.CreatedBy = header.CreatedBy;
                        distHeader.CreatedDate = DateTime.Now;
                        distHeader.Status = header.Status;

                        _context.DistributionJournalHeaders.Add(distHeader);
                    }

                    //INSERT NEW DISTRIBUTION JOURNAL
                    if (distHeader.DistributionHeaderId != Guid.Empty)
                    {
                        List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                        if (resp.Valid)
                        {
                            if (!string.IsNullOrEmpty(cb.CheckbookAccountNo))
                            {
                                var creditJournalDetail = new DistributionJournalDetail()
                                {
                                    DistributionDetailId = Guid.NewGuid(),
                                    DistributionHeaderId = distHeader.DistributionHeaderId,
                                    AccountId = cb.CheckbookAccountNo,
                                    ChargesId = Guid.Empty,
                                    JournalNote = "Payment #" + header.DocumentNo,
                                    OriginatingDebit = 0,
                                    OriginatingCredit = distHeader.OriginatingTotal,
                                    FunctionalDebit = 0,
                                    FunctionalCredit = distHeader.FunctionalTotal,
                                    Status = DOCSTATUS.NEW,
                                    JournalDate = distHeader.DocumentDate
                                };

                                distDetails.Add(creditJournalDetail);
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("Checkbook {0} Account No is empty !", cb.CheckbookCode);
                            }

                            if (resp.Valid)
                            {
                                if (details.Count > 0)
                                {
                                    //AP
                                    decimal apAmount = details.Sum(s => s.OriginatingPaid);
                                    if (apAmount > 0)
                                    {
                                        var debitJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = vendor.PayableAccountNo,
                                            ChargesId = Guid.Empty,
                                            JournalNote = "Payable #" + header.DocumentNo,
                                            OriginatingDebit = apAmount,
                                            OriginatingCredit = 0,
                                            FunctionalDebit = CALC.FunctionalAmount(header.IsMultiply, apAmount, header.ExchangeRate),
                                            FunctionalCredit = 0,
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = distHeader.DocumentDate
                                        };

                                        distDetails.Add(debitJournalDetail);
                                    }

                                    //ADVANCE (if any)
                                    decimal advanceAmount = header.OriginatingTotalPaid - apAmount;
                                    if (advanceAmount > 0)
                                    {
                                        var debitJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = advanceAccountId,
                                            ChargesId = Guid.Empty,
                                            JournalNote = "Payment #" + header.DocumentNo,
                                            OriginatingDebit = advanceAmount,
                                            OriginatingCredit = 0,
                                            FunctionalDebit = CALC.FunctionalAmount(header.IsMultiply, advanceAmount, header.ExchangeRate),
                                            FunctionalCredit = 0,
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = distHeader.DocumentDate
                                        };

                                        distDetails.Add(debitJournalDetail);
                                    }
                                }
                                else
                                {
                                    //ADVANCE FULL
                                    var creditJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = advanceAccountId,
                                        ChargesId = Guid.Empty,
                                        JournalNote = "Payment #" + header.DocumentNo,
                                        OriginatingDebit = distHeader.OriginatingTotal,
                                        OriginatingCredit = 0,
                                        FunctionalDebit = distHeader.FunctionalTotal,
                                        FunctionalCredit = 0,
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = distHeader.DocumentDate
                                    };

                                    distDetails.Add(creditJournalDetail);
                                }
                            }
                        }

                        if (resp.Valid)
                        {
                            _context.DistributionJournalDetails.AddRange(distDetails);
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Distribution Header GUID can not be generated";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = errorMsg.ToString();
                }
            }
            else
            {
                resp.Valid = false;
                resp.ErrorMessage = "Document No is empty !";
            }

            return resp;
        }

        public async Task<JournalResponse> CreateDistributionJournalAsync(ArReceiptHeader header, List<ArReceiptDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (!string.IsNullOrEmpty(header.DocumentNo))
            {
                StringBuilder errorMsg = new StringBuilder();
                
                var cb = _context.Checkbooks.Where(x => x.CheckbookCode == header.CheckbookCode).FirstOrDefault();
                if (cb == null)
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Checkbook {0} must be set !", header.CheckbookCode));
                }

                var customer = _context.Customers.Where(x => x.CustomerId == header.CustomerId).FirstOrDefault();
                if (string.IsNullOrEmpty(customer.ReceivableAccountNo))
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Customer {0} Receivable Account No must be set !", header.CustomerCode));
                }

                string advanceAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AR_ADVANCE_RECEIPT).Select(s=>s.AccountId).FirstOrDefault();
                if (string.IsNullOrEmpty(advanceAccountId))
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Advance Receipt Account must be set !"));
                }

                if (resp.Valid)
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_RECEIPT).FirstOrDefault();

                    if (edit != null)
                    {
                        var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                        //REMOVE EXISTING
                        await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                        //UPDATE HEADER
                        edit.CurrencyCode = header.CurrencyCode;
                        edit.Description = header.Description;
                        edit.DocumentDate = header.TransactionDate;
                        edit.ExchangeRate = header.ExchangeRate;
                        edit.IsMultiply = header.IsMultiply;
                        edit.TransactionId = header.ReceiptHeaderId;
                        edit.OriginatingTotal = header.OriginatingTotalPaid;
                        edit.FunctionalTotal = header.FunctionalTotalPaid;
                        edit.ModifiedBy = header.ModifiedBy;
                        edit.ModifiedDate = DateTime.Now;
                        edit.Status = header.Status;

                        _context.Update(edit);

                        distHeader = edit;
                    }
                    else
                    {
                        //UPDATE HEADER
                        distHeader.DistributionHeaderId = Guid.NewGuid();
                        distHeader.TrxModule = TRX_MODULE.TRX_RECEIPT;
                        distHeader.TransactionId = header.ReceiptHeaderId;
                        distHeader.DocumentDate = header.TransactionDate;
                        distHeader.DocumentNo = header.DocumentNo;
                        distHeader.CurrencyCode = header.CurrencyCode;
                        distHeader.ExchangeRate = header.ExchangeRate;
                        distHeader.IsMultiply = header.IsMultiply;
                        distHeader.Description = header.Description;
                        distHeader.OriginatingTotal = header.OriginatingTotalPaid;
                        distHeader.FunctionalTotal = header.FunctionalTotalPaid;
                        distHeader.CreatedBy = header.CreatedBy;
                        distHeader.CreatedDate = DateTime.Now;
                        distHeader.Status = header.Status;

                        _context.DistributionJournalHeaders.Add(distHeader);

                    }

                    //INSERT NEW DISTRIBUTION JOURNAL
                    if (distHeader.DistributionHeaderId != Guid.Empty)
                    {
                        List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                        if (resp.Valid)
                        {
                            if (!string.IsNullOrEmpty(cb.CheckbookAccountNo))
                            {
                                var debitJournalDetail = new DistributionJournalDetail()
                                {
                                    DistributionDetailId = Guid.NewGuid(),
                                    DistributionHeaderId = distHeader.DistributionHeaderId,
                                    AccountId = cb.CheckbookAccountNo,
                                    ChargesId = Guid.Empty,
                                    JournalNote = "Receipt #" + header.DocumentNo,
                                    OriginatingDebit = distHeader.OriginatingTotal,
                                    OriginatingCredit = 0,
                                    FunctionalDebit = distHeader.FunctionalTotal,
                                    FunctionalCredit = 0,
                                    Status = DOCSTATUS.NEW,
                                    JournalDate = distHeader.DocumentDate
                                };

                                distDetails.Add(debitJournalDetail);
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("Checkbook {0} Account No is empty !", cb.CheckbookCode);
                            }

                            if (resp.Valid)
                            {
                                if(details.Count > 0)
                                {
                                    //AR
                                    decimal arAmount = details.Sum(s => s.OriginatingPaid);
                                    if(arAmount > 0)
                                    {
                                        var creditJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = customer.ReceivableAccountNo,
                                            ChargesId = Guid.Empty,
                                            JournalNote = "Receipt #" + header.DocumentNo,
                                            OriginatingDebit = 0,
                                            OriginatingCredit = arAmount,
                                            FunctionalDebit = 0,
                                            FunctionalCredit = CALC.FunctionalAmount(header.IsMultiply, arAmount, header.ExchangeRate),
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = distHeader.DocumentDate
                                        };

                                        distDetails.Add(creditJournalDetail);
                                    }

                                    //ADVANCE (if any)
                                    decimal advanceAmount = header.OriginatingTotalPaid - arAmount;
                                    if(advanceAmount > 0)
                                    {
                                        var creditJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = advanceAccountId,
                                            ChargesId = Guid.Empty,
                                            JournalNote = "Receipt #" + header.DocumentNo,
                                            OriginatingDebit = 0,
                                            OriginatingCredit = advanceAmount,
                                            FunctionalDebit = 0,
                                            FunctionalCredit = CALC.FunctionalAmount(header.IsMultiply, advanceAmount, header.ExchangeRate),
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = distHeader.DocumentDate
                                        };

                                        distDetails.Add(creditJournalDetail);
                                    }
                                }
                                else
                                {
                                    //ADVANCE FULL
                                    var creditJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = advanceAccountId,
                                        ChargesId = Guid.Empty,
                                        JournalNote = "Receipt #" + header.DocumentNo,
                                        OriginatingDebit = 0,
                                        OriginatingCredit = distHeader.OriginatingTotal,
                                        FunctionalDebit = 0,
                                        FunctionalCredit = distHeader.FunctionalTotal,
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = distHeader.DocumentDate
                                    };

                                    distDetails.Add(creditJournalDetail);
                                }                                
                            }
                        }

                        if (resp.Valid)
                        {
                            _context.DistributionJournalDetails.AddRange(distDetails);
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Distribution Header GUID can not be generated";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = errorMsg.ToString();
                }
            }
            else
            {
                resp.Valid = false;
                resp.ErrorMessage = "Document No is empty !";
            }
            
            return resp;
        }

        public async Task<JournalResponse> CreateDistributionJournalAsync(DepositSettlementHeader header, List<DepositSettlementDetail> details, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (!string.IsNullOrEmpty(header.DocumentNo))
            {
                StringBuilder errorMsg = new StringBuilder();

                var cb = _context.Checkbooks.Where(x => x.CheckbookCode == header.CheckbookCode).FirstOrDefault();
                if (cb == null)
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Checkbook {0} must be set !", header.CheckbookCode));
                }

                var customer = _context.Customers.Where(x => x.CustomerId == header.CustomerId).FirstOrDefault();
                if (string.IsNullOrEmpty(customer.ReceivableAccountNo))
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Customer {0} Receivable Account No must be set !", header.CustomerCode));
                }

                string advanceAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.AR_ADVANCE_RECEIPT).Select(s => s.AccountId).FirstOrDefault();
                if (string.IsNullOrEmpty(advanceAccountId))
                {
                    resp.Valid = false;
                    errorMsg.AppendLine(string.Format("Deposit Settlement Account must be set !"));
                }

                if (resp.Valid)
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(header.DocumentNo) && x.TrxModule == TRX_MODULE.TRX_RECEIPT).FirstOrDefault();

                    if (edit != null)
                    {
                        var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                        //REMOVE EXISTING
                        await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                        //UPDATE HEADER
                        edit.CurrencyCode = header.CurrencyCode;
                        edit.Description = header.Description;
                        edit.DocumentDate = header.TransactionDate;
                        edit.ExchangeRate = header.ExchangeRate;
                        edit.IsMultiply = header.IsMultiply;
                        edit.TransactionId = header.SettlementHeaderId;
                        edit.OriginatingTotal = header.OriginatingTotalPaid;
                        edit.FunctionalTotal = header.FunctionalTotalPaid;
                        edit.ModifiedBy = header.ModifiedBy;
                        edit.ModifiedDate = DateTime.Now;
                        edit.Status = header.Status;

                        _context.Update(edit);

                        distHeader = edit;
                    }
                    else
                    {
                        //UPDATE HEADER
                        distHeader.DistributionHeaderId = Guid.NewGuid();
                        distHeader.TrxModule = TRX_MODULE.TRX_RECEIPT;
                        distHeader.TransactionId = header.SettlementHeaderId;
                        distHeader.DocumentDate = header.TransactionDate;
                        distHeader.DocumentNo = header.DocumentNo;
                        distHeader.CurrencyCode = header.CurrencyCode;
                        distHeader.ExchangeRate = header.ExchangeRate;
                        distHeader.IsMultiply = header.IsMultiply;
                        distHeader.Description = header.Description;
                        distHeader.OriginatingTotal = header.OriginatingTotalPaid;
                        distHeader.FunctionalTotal = header.FunctionalTotalPaid;
                        distHeader.CreatedBy = header.CreatedBy;
                        distHeader.CreatedDate = DateTime.Now;
                        distHeader.Status = header.Status;

                        _context.DistributionJournalHeaders.Add(distHeader);

                    }

                    //INSERT NEW DISTRIBUTION JOURNAL
                    if (distHeader.DistributionHeaderId != Guid.Empty)
                    {
                        List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                        if (resp.Valid)
                        {
                            if (!string.IsNullOrEmpty(cb.CheckbookAccountNo))
                            {
                                var debitJournalDetail = new DistributionJournalDetail()
                                {
                                    DistributionDetailId = Guid.NewGuid(),
                                    DistributionHeaderId = distHeader.DistributionHeaderId,
                                    AccountId = cb.CheckbookAccountNo,
                                    ChargesId = Guid.Empty,
                                    JournalNote = "Receipt #" + header.DocumentNo,
                                    OriginatingDebit = 0,
                                    OriginatingCredit = distHeader.OriginatingTotal,
                                    FunctionalDebit = 0,
                                    FunctionalCredit = distHeader.FunctionalTotal,
                                    Status = DOCSTATUS.NEW,
                                    JournalDate = distHeader.DocumentDate,
                                };

                                distDetails.Add(debitJournalDetail);
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = string.Format("Checkbook {0} Account No is empty !", cb.CheckbookCode);
                            }

                            if (resp.Valid)
                            {
                                if (details.Count > 0)
                                {
                                    //AR
                                    decimal arAmount = details.Sum(s => s.OriginatingPaid);
                                    if (arAmount > 0)
                                    {
                                        var creditJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = customer.ReceivableAccountNo,
                                            ChargesId = Guid.Empty,
                                            JournalNote = "Receipt #" + header.DocumentNo,
                                            OriginatingDebit = 0,
                                            OriginatingCredit = arAmount,
                                            FunctionalDebit = 0,
                                            FunctionalCredit = CALC.FunctionalAmount(header.IsMultiply, arAmount, header.ExchangeRate),
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = distHeader.DocumentDate
                                        };

                                        distDetails.Add(creditJournalDetail);
                                    }

                                    //ADVANCE (if any)
                                    decimal advanceAmount = header.OriginatingTotalPaid - arAmount;
                                    if (advanceAmount > 0)
                                    {
                                        var creditJournalDetail = new DistributionJournalDetail()
                                        {
                                            DistributionDetailId = Guid.NewGuid(),
                                            DistributionHeaderId = distHeader.DistributionHeaderId,
                                            AccountId = advanceAccountId,
                                            ChargesId = Guid.Empty,
                                            JournalNote = "Receipt #" + header.DocumentNo,
                                            OriginatingDebit = 0,
                                            OriginatingCredit = advanceAmount,
                                            FunctionalDebit = 0,
                                            FunctionalCredit = CALC.FunctionalAmount(header.IsMultiply, advanceAmount, header.ExchangeRate),
                                            Status = DOCSTATUS.NEW,
                                            JournalDate = distHeader.DocumentDate
                                        };

                                        distDetails.Add(creditJournalDetail);
                                    }
                                }
                                else
                                {
                                    //ADVANCE FULL
                                    var creditJournalDetail = new DistributionJournalDetail()
                                    {
                                        DistributionDetailId = Guid.NewGuid(),
                                        DistributionHeaderId = distHeader.DistributionHeaderId,
                                        AccountId = advanceAccountId,
                                        ChargesId = Guid.Empty,
                                        JournalNote = "Receipt #" + header.DocumentNo,
                                        OriginatingDebit = distHeader.OriginatingTotal,
                                        OriginatingCredit = 0,
                                        FunctionalDebit = distHeader.FunctionalTotal,
                                        FunctionalCredit = 0,
                                        Status = DOCSTATUS.NEW,
                                        JournalDate = distHeader.DocumentDate
                                    };

                                    distDetails.Add(creditJournalDetail);
                                }
                            }
                        }

                        if (resp.Valid)
                        {
                            _context.DistributionJournalDetails.AddRange(distDetails);
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Distribution Header GUID can not be generated";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = errorMsg.ToString();
                }
            }
            else
            {
                resp.Valid = false;
                resp.ErrorMessage = "Document No is empty !";
            }

            return resp;
        }

        private async Task<JournalResponse> CreateDistributionJournalAsync(GLClosingHeader header, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (header != null)
            {
                string journalNote = string.Format("{0} {1}/{2}", "Close Month", header.PeriodYear, header.PeriodIndex);
                string documentNo = string.Format("{0}/{1}/{2}", "CM", header.PeriodYear, header.PeriodIndex);
                if (header.IsYearly)
                {
                    journalNote = string.Format("{0} {1}", "Close Year", header.PeriodYear);
                    documentNo = string.Format("{0}/{1}", "CY", header.PeriodYear);
                }

                
                if (!string.IsNullOrEmpty(documentNo))
                {
                    DistributionJournalHeader distHeader = new DistributionJournalHeader();

                    var edit = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(documentNo) && x.TrxModule == TRX_MODULE.TRX_GENERAL_JOURNAL).FirstOrDefault();

                    if (edit != null)
                    {
                        var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();

                        //REMOVE EXISTING
                        await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", edit.DistributionHeaderId);

                        //UPDATE HEADER
                        edit.CurrencyCode = header.CurrencyCode;
                        edit.Description = journalNote;
                        edit.DocumentDate = header.ClosingDate;
                        edit.ExchangeRate = 1;
                        edit.IsMultiply = true;
                        edit.TransactionId = Guid.Empty;
                        edit.OriginatingTotal = 0;
                        edit.FunctionalTotal = header.RetainBalance;
                        edit.ModifiedBy = header.ClosedBy;
                        edit.ModifiedDate = DateTime.Now;
                        edit.Status = DOCSTATUS.POST;

                        _context.Update(edit);

                        distHeader = edit;
                    }
                    else
                    {
                        //UPDATE HEADER
                        distHeader.DistributionHeaderId = Guid.NewGuid();
                        distHeader.TrxModule = TRX_MODULE.TRX_GENERAL_JOURNAL;
                        distHeader.TransactionId = Guid.Empty;
                        distHeader.DocumentDate = header.ClosingDate;
                        distHeader.DocumentNo = documentNo;
                        distHeader.CurrencyCode = header.CurrencyCode;
                        distHeader.ExchangeRate = 1;
                        distHeader.IsMultiply = true;
                        distHeader.Description = journalNote;
                        distHeader.OriginatingTotal = 0;
                        distHeader.FunctionalTotal = header.RetainBalance;
                        distHeader.CreatedBy = header.ClosedBy;
                        distHeader.CreatedDate = DateTime.Now;
                        distHeader.Status = DOCSTATUS.POST;

                        _context.DistributionJournalHeaders.Add(distHeader);
                    }

                    //INSERT NEW DISTRIBUTION JOURNAL
                    if (distHeader.DistributionHeaderId != Guid.Empty)
                    {
                        List<DistributionJournalDetail> distDetails = new List<DistributionJournalDetail>();

                        if (header.IsYearly)
                        {
                            //CLOSE YEAR
                            //PREV RETAIN
                            var prevRetain = new DistributionJournalDetail()
                            {
                                DistributionDetailId = Guid.NewGuid(),
                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                AccountId = header.AccountId,
                                ChargesId = Guid.Empty,
                                JournalNote = journalNote,
                                OriginatingDebit = 0,
                                OriginatingCredit = 0,
                                FunctionalDebit = (header.RetainBalance < 0) ? Math.Abs(header.RetainBalance) : 0, //LOSS 
                                FunctionalCredit = (header.RetainBalance >= 0 ? Math.Abs(header.RetainBalance) : 0), //PROFIT
                                Status = DOCSTATUS.POST,
                                JournalDate = header.ClosingDate
                            };

                            distDetails.Add(prevRetain);

                            //CURRENT RETAIN
                            string retainCurrentAccountId = _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_CURRENT).Select(x => x.AccountId).FirstOrDefault();

                            var currentRetain = new DistributionJournalDetail()
                            {
                                DistributionDetailId = Guid.NewGuid(),
                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                AccountId = retainCurrentAccountId,
                                ChargesId = Guid.Empty,
                                JournalNote = journalNote,
                                OriginatingDebit = 0,
                                OriginatingCredit = 0,
                                FunctionalDebit = (header.RetainBalance < 0) ? 0 : Math.Abs(header.RetainBalance), //LOSS 
                                FunctionalCredit = (header.RetainBalance >= 0 ? 0 : Math.Abs(header.RetainBalance)), //PROFIT
                                Status = DOCSTATUS.POST,
                                JournalDate = header.ClosingDate
                            };

                            distDetails.Add(currentRetain);

                            if(Math.Abs(currentRetain.FunctionalCredit + currentRetain.FunctionalDebit) == Math.Abs(prevRetain.FunctionalDebit + prevRetain.FunctionalCredit))
                            {
                                await _context.DistributionJournalDetails.AddRangeAsync(distDetails);
                            }
                            else
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = "Retained Earnings balance not match !";
                            }
                        }
                        else
                        {
                            //CLOSE MONTH
                            var distributionJournalDetail = new DistributionJournalDetail()
                            {
                                DistributionDetailId = Guid.NewGuid(),
                                DistributionHeaderId = distHeader.DistributionHeaderId,
                                AccountId = header.AccountId,
                                ChargesId = Guid.Empty,
                                JournalNote = journalNote,
                                OriginatingDebit = 0,
                                OriginatingCredit = 0,
                                FunctionalDebit = (header.RetainBalance < 0) ? Math.Abs(header.RetainBalance) : 0, //LOSS 
                                FunctionalCredit = (header.RetainBalance >= 0 ? Math.Abs(header.RetainBalance) : 0), //PROFIT
                                Status = DOCSTATUS.POST,
                                JournalDate = header.ClosingDate
                            };

                            distDetails.Add(distributionJournalDetail);
                        }                        

                        _context.DistributionJournalDetails.AddRange(distDetails);
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Header GUID can not be generated";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Document No is not valid";
                }
            }

            return resp;
        }

        /// <summary>
        /// Delete Distribution Journal Permanently
        /// </summary>
        /// <param name="documentNo"></param>
        /// <param name="trxModule"></param>
        /// <param name="includeTransaction"></param>
        /// <returns></returns>
        public async Task<JournalResponse> DeleteDistributionJournalAsync(string documentNo, int trxModule, bool includeTransaction = true)
        {
            JournalResponse resp = new JournalResponse();
            resp.Valid = true;
            resp.ErrorMessage = "";

            if (!string.IsNullOrEmpty(documentNo) && trxModule > 0)
            {
                var header = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(documentNo) && x.TrxModule == trxModule).FirstOrDefault();

                if(header != null)
                {
                    var mapping = _context.Model.FindEntityType(typeof(DistributionJournalDetail)).Relational();
                    int deleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE distribution_header_id = {0} ", header.DistributionHeaderId);

                    _context.Attach(header);
                    _context.Remove(header);
                }                
            }

            return resp;
            
        }

        /// <summary>
        /// Get Posting Account by Parameter
        /// </summary>
        /// <param name="postingParamKey"></param>
        /// <returns></returns>
        public async Task<string> GetPostingAccount(int postingParamKey = 0)
        {
            string result = string.Empty;

            if(postingParamKey > 0)
            {
                result = _context.FNPostingParams.Where(x => x.PostingKey == postingParamKey)
                    .Select(s=>s.AccountId).FirstOrDefault();
            }

            return result;
        }

        #endregion Distribution Journal

        #region Post Journal

        /// <summary>
        /// Distribution Journal for Post Journal
        /// </summary>
        /// <param name="documentNo"></param>
        /// <param name="includeTransaction"></param>
        /// <returns></returns>
        public async Task<JournalResponse> PostJournalAsync(string documentNo, int trxModule, string userId, DateTime postingDate, bool includeTransaction = true)
        {
            JournalResponse response = new JournalResponse();
            response.Valid = true;
            response.ErrorMessage = "";

            if (!string.IsNullOrEmpty(documentNo))
            {
                DistributionJournalHeader header = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(documentNo) && x.TrxModule == trxModule).FirstOrDefault();

                if(header != null)
                {
                    //Create Post Journal 
                    var details = _context.DistributionJournalDetails.Where(x => x.DistributionHeaderId == header.DistributionHeaderId).ToList();

                    foreach(var det in details)
                    {
                        det.JournalDate = header.DocumentDate; //postingDate;
                        det.Status = DOCSTATUS.POST;
                        det.ModifiedBy = userId;
                        det.ModifiedDate = DateTime.Now;

                        _context.DistributionJournalDetails.Update(det);
                    }

                    header.Status = DOCSTATUS.POST;
                    header.ModifiedBy = userId;
                    header.ModifiedDate = DateTime.Now;

                    _context.DistributionJournalHeaders.Update(header);
                    
                }
                else
                {
                    response.Valid = false;
                    response.ErrorMessage = "Distribution Journal Document not found !";
                }
            }
            else
            {
                response.Valid = false;
                response.ErrorMessage = "Document not found";
            }

            return response;
        }

        public async Task<JournalResponse> VoidJournalAsync(string documentNo, int trxModule, string userId, DateTime postingDate, bool includeTransaction = true)
        {
            JournalResponse response = new JournalResponse();
            response.Valid = true;
            response.ErrorMessage = "";

            if (!string.IsNullOrEmpty(documentNo))
            {
                DistributionJournalHeader header = _context.DistributionJournalHeaders.Where(x => x.DocumentNo.Equals(documentNo) && x.TrxModule == trxModule).FirstOrDefault();

                if (header != null)
                {
                    //Reverse Post Journal 
                    List<DistributionJournalDetail> voidDetails = new List<DistributionJournalDetail>();

                    var sourceJournals = _context.DistributionJournalDetails.Where(x => x.DistributionHeaderId == header.DistributionHeaderId);
                    if (sourceJournals.Any())
                    {
                        foreach(var det in sourceJournals)
                        {
                            DistributionJournalDetail voidDetail = new DistributionJournalDetail();

                            voidDetail.DistributionDetailId = Guid.NewGuid();
                            voidDetail.DistributionHeaderId = det.DistributionHeaderId;
                            voidDetail.AccountId = det.AccountId;
                            voidDetail.ChargesId = det.ChargesId;
                            voidDetail.JournalNote = det.JournalNote;

                            voidDetail.Status = DOCSTATUS.VOID;
                            voidDetail.JournalDate = postingDate;
                            voidDetail.ModifiedBy = userId;
                            voidDetail.ModifiedDate = DateTime.Now;

                            //REVERSE POSTED DEBIT -> CREDIT
                            voidDetail.OriginatingDebit = det.OriginatingCredit;
                            voidDetail.OriginatingCredit = det.OriginatingDebit;
                            voidDetail.FunctionalDebit = det.FunctionalCredit;
                            voidDetail.FunctionalCredit = det.FunctionalDebit;

                            voidDetails.Add(voidDetail);
                        }
                    }
                    
                    if (voidDetails.Count > 0)
                    {
                        _context.DistributionJournalDetails.AddRange(voidDetails);

                        header.Status = DOCSTATUS.VOID;
                        header.ModifiedBy = userId;
                        header.ModifiedDate = DateTime.Now;

                        _context.Update(header);
                    }
                    else
                    {
                        //KEEP AS VALID , because some journal may appear occasionally (exchange rate gain/loss)
                    }                    
                }
                else
                {
                    response.Valid = false;
                    response.ErrorMessage = "Document not exist for this module";
                }
            }
            else
            {
                response.Valid = false;
                response.ErrorMessage = "Document not found";
            }

            return response;
        }

        #endregion Post Journal

        #region Closing
        public async Task<JournalResponse> CloseMonth(string currencyCode, int periodYear, int periodIndex, string userId = "", bool includeTransaction = true)
        {
            JournalResponse response = new JournalResponse { ErrorMessage = "", Valid = true, ValidMessage = "", ValidStatus = 0 };

            if(!string.IsNullOrEmpty(currencyCode) && periodYear > 2010 && periodIndex > 0 )
            {
                var currentPnLAccountId = (from postingSetup in _context.FNPostingParams.Where(x => x.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_CURRENT)
                                           join coa in _context.Accounts on postingSetup.AccountId equals coa.AccountId
                                           select postingSetup.AccountId).FirstOrDefault();

                if (!string.IsNullOrEmpty(currentPnLAccountId))
                {
                    var fiscalPeriod = (from d in _context.FiscalPeriodDetails
                                        join h in _context.FiscalPeriodHeaders on d.FiscalHeaderId equals h.FiscalHeaderId
                                        where h.PeriodYear == periodYear && d.PeriodIndex == periodIndex
                                        select new { PeriodYear = h.PeriodYear, MaxPeriodIndex = h.TotalPeriod, PeriodEnd = d.PeriodEnd }).FirstOrDefault();

                    if(fiscalPeriod != null)
                    {
                        DateTime closingDate = fiscalPeriod.PeriodEnd;

                        try
                        {
                            decimal plAmount = 0;

                            GLClosingHeader header = new GLClosingHeader
                            {
                                CurrencyCode = currencyCode,
                                ClosingDate = closingDate,
                                PeriodYear = periodYear,
                                PeriodIndex = periodIndex,                                
                                IsYearly = false,
                                AccountId = currentPnLAccountId,
                                RetainBalance = plAmount,
                                Status = DOCSTATUS.NEW,
                                ClosedBy = userId,
                            };

                            _context.GLClosingHeaders.Add(header);

                            await _context.SaveChangesAsync();

                            if (header.ClosingHeaderId > 0)
                            {
                                //DO DISTRIBUTION JOURNAL
                                var journal = await CreateDistributionJournalAsync(header);

                                if (!journal.Valid)
                                {
                                    response.Valid = false;
                                    response.ErrorMessage = journal.ErrorMessage;
                                }

                                //DO CLOSING MONTHS
                                var resp = await SummarizeCOAEntries(header);

                                if (!resp.Valid)
                                {
                                    return resp;
                                }

                                //DO CLOSING YEAR
                                if (periodIndex == fiscalPeriod.MaxPeriodIndex)
                                {
                                    await _context.SaveChangesAsync();

                                    var closeYear = await CloseYear(header);

                                    if (!closeYear.Valid)
                                    {
                                        return closeYear;
                                    }
                                }

                                //SET FISCAL PERIOD
                                /*
                                fiscalPeriod.IsCloseFinancial = true;
                                fiscalPeriod.IsCloseAsset = true;
                                fiscalPeriod.IsCloseInventory = true;
                                fiscalPeriod.IsClosePurchasing = true;
                                fiscalPeriod.IsCloseSales = true;

                                _context.FiscalPeriodDetails.Update(fiscalPeriod);*/

                                response.ValidMessage = header.ClosingHeaderId.ToString();
                            }
                            else
                            {
                                response.Valid = false;
                                response.ErrorMessage = string.Format("Closing Month header can not be created !");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[CloseMonth] ****** ERROR ***** " + ex.Message);
                            Console.WriteLine("[CloseMonth] ****** STACK ***** " + ex.StackTrace);

                            response.Valid = false;
                            response.ErrorMessage = string.Format("Closing Month failed ! {0}", ex.Message);
                        }
                    }
                    else
                    {
                        response.Valid = false;
                        response.ErrorMessage = string.Format("Fiscal Period not found !");
                    }
                }
                else
                {
                    response.Valid = false;
                    response.ErrorMessage = string.Format("Current Retain Earnings in Posting Setup is empty or not valid !");
                }
            }
            else
            {
                response.Valid = false;
                response.ErrorMessage = string.Format("Close Month parameters not valid !");
            }

            return response;
        }

        private async Task<JournalResponse> SummarizeCOAEntries(GLClosingHeader header)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            string qry = string.Format("SELECT DISTINCT Convert(varchar(max),STUFF((SELECT ',' + a.account_id FROM account a FOR XML PATH ('')), 1, 1, '')) as coa_code_comma FROM account ");

            DataTable dt = await RawQuery.SelectRawSqlAsync(_context.Database, qry, _context.Database.CurrentTransaction.GetDbTransaction());

            if(dt.Rows.Count > 0)
            {
                string coaCommas = dt.Rows[0]["coa_code_comma"].ToString();

                qry = string.Format("SELECT * FROM fxnGLCOABalanceByMonth({0},{1},'{2}') ", header.PeriodIndex, header.PeriodYear, coaCommas);

                Console.WriteLine("[SummarizeCOAEntries] *****************  " + qry);

                var currentTable = await RawQuery.SelectRawSqlAsync(_context.Database, qry, _context.Database.CurrentTransaction.GetDbTransaction());

                List<GLClosingDetail> details = new List<GLClosingDetail>();

                decimal prevBalance = 0;
                foreach (DataRow currentRow in currentTable.Rows)
                {
                    prevBalance = 0;

                    decimal currentBalance = (decimal) currentRow["Balance"];

                    var closeDetail = new GLClosingDetail
                    {
                        ClosingHeaderId = header.ClosingHeaderId,
                        BranchCode = currentRow["BranchCode"].ToString(),
                        AccountId = currentRow["AccountId"].ToString(),
                        Debit = (decimal)currentRow["Debit"],
                        Credit = (decimal)currentRow["Credit"],
                        Balance = prevBalance + currentBalance,                        
                        Status = DOCSTATUS.NEW
                    };

                    details.Add(closeDetail);
                }

                await _context.GLClosingDetails.AddRangeAsync(details);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    resp.Valid = false;
                    resp.ErrorMessage = string.Format("Close Month Account entries failed !");
                }
            }
            else
            {
                resp.Valid = false;
                resp.ErrorMessage = string.Format("Account Id can not be obtain !");
            }

            return resp;
        }

        private async Task<JournalResponse> CloseYear(GLClosingHeader closeMonth)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            if(closeMonth != null)
            {
                var retainSetups = (from param in _context.FNPostingParams
                                    join coa in _context.Accounts on param.AccountId equals coa.AccountId
                                    where param.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_CURRENT || param.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_PREVIOUS
                                    select param).AsQueryable();

                string retainCurrentAccountId = retainSetups.Where(x => x.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_CURRENT).Select(x => x.AccountId).FirstOrDefault();
                string retainPreviousAccountId = retainSetups.Where(x => x.PostingKey == POSTING_PARAM.GL_RETAIN_EARNING_PREVIOUS).Select(x => x.AccountId).FirstOrDefault();

                if(!string.IsNullOrEmpty(retainCurrentAccountId) && !string.IsNullOrEmpty(retainPreviousAccountId))
                {
                    decimal closingBalance = 0;

                    if(closingBalance != 0)
                    {
                        DateTime closingDate = closeMonth.ClosingDate.AddDays(1);

                        GLClosingHeader closeYearHeader = new GLClosingHeader
                        {
                            CurrencyCode = closeMonth.CurrencyCode,
                            ClosingDate = closingDate,
                            PeriodYear = closeMonth.PeriodYear,
                            PeriodIndex = closeMonth.PeriodIndex,
                            IsYearly = true,
                            AccountId = retainPreviousAccountId,
                            RetainBalance = closingBalance,
                            Status = DOCSTATUS.NEW,
                            ClosedBy = closeMonth.ClosedBy,
                        };

                        _context.GLClosingHeaders.Add(closeYearHeader);

                        await _context.SaveChangesAsync();

                        if(closeYearHeader.ClosingHeaderId > 0)
                        {
                            var journal = await CreateDistributionJournalAsync(closeYearHeader);

                            if (!journal.Valid)
                            {
                                resp.Valid = false;
                                resp.ErrorMessage = journal.ErrorMessage;
                            }
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Last Close Month Balance is 0 !";
                    }

                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = string.Format("Retain Earnings Current & Previous must be set !");
                }                
            }
            else
            {
                resp.Valid = false;
                resp.ErrorMessage = string.Format("Close Year is not valid !");
            }
            

            return resp;
        }

        public async Task<JournalResponse> UncloseMonth(string currencyCode, int periodYear, int periodIndex, string userId = "", bool includeTransaction = true)
        {
            JournalResponse response = new JournalResponse { ErrorMessage = "", Valid = true, ValidMessage = "", ValidStatus = 0 };

            if (!string.IsNullOrEmpty(currencyCode) && periodYear > 2010 && periodIndex > 0)
            {
                try
                {
                    //UNCLOSE MONTH
                    var cancelPeriods = await _context.GLClosingHeaders.Where(x => x.PeriodIndex >= periodIndex && x.PeriodYear == periodYear && !x.IsYearly && x.Status == DOCSTATUS.NEW).OrderByDescending(x => x.PeriodYear).OrderByDescending(x => x.PeriodIndex).ToListAsync();

                    foreach(var canceled in cancelPeriods)
                    {
                        //UPDATE EXISTING
                        var details =_context.GLClosingDetails
                       .Where(x => x.ClosingHeaderId == canceled.ClosingHeaderId).ToList();
                        foreach(var det in details)
                        {
                            det.Status = DOCSTATUS.CANCEL;

                            _context.Update(det);
                        }

                        DateTime journalDate = canceled.ClosingDate;

                        string documentNo = string.Format("{0}/{1}/{2}", "CM", periodYear, periodIndex);

                        var closedJournals = await (from h in _context.DistributionJournalHeaders
                                              where h.DocumentNo == documentNo && h.TrxModule == TRX_MODULE.TRX_GENERAL_JOURNAL && h.DocumentDate.Date == journalDate.Date
                                              select h).ToListAsync();

                        foreach(var cj in closedJournals)
                        {
                            //REMOVE EXISTING
                            _context.DistributionJournalDetails
                           .Where(x => x.DistributionHeaderId == cj.DistributionHeaderId).ToList().ForEach(p => _context.Remove(p));
                        }

                        _context.DistributionJournalHeaders.RemoveRange(closedJournals);

                        //UPDATE GL PERIOD
                        canceled.Status = DOCSTATUS.CANCEL;
                        canceled.UnclosedBy = userId;
                        canceled.UnclosedDate = DateTime.Now;
                        
                        _context.GLClosingHeaders.Update(canceled);

                    }

                    //UNCLOSE SAME YEAR (IF ANY)
                    if(periodIndex >= 12)
                    {
                        var cancelYears = await _context.GLClosingHeaders.Where(x => x.PeriodYear == periodYear && x.IsYearly && x.Status == DOCSTATUS.NEW).OrderByDescending(x => x.PeriodYear).OrderByDescending(x => x.PeriodIndex).ToListAsync();

                        foreach (var canceled in cancelPeriods)
                        {
                            //UPDATE EXISTING
                            var details = _context.GLClosingDetails
                           .Where(x => x.ClosingHeaderId == canceled.ClosingHeaderId).ToList();
                            foreach (var det in details)
                            {
                                det.Status = DOCSTATUS.CANCEL;

                                _context.Update(det);
                            }

                            DateTime journalDate = canceled.ClosingDate;

                            string documentNo = string.Format("{0}/{1}", "CY", periodYear);

                            var closedJournals = await (from h in _context.DistributionJournalHeaders
                                                        where h.DocumentNo == documentNo && h.TrxModule == TRX_MODULE.TRX_GENERAL_JOURNAL && h.DocumentDate.Date == journalDate.Date
                                                        select h).ToListAsync();

                            foreach (var cj in closedJournals)
                            {
                                //REMOVE EXISTING
                                _context.DistributionJournalDetails
                               .Where(x => x.DistributionHeaderId == cj.DistributionHeaderId).ToList().ForEach(p => _context.Remove(p));
                            }

                            _context.DistributionJournalHeaders.RemoveRange(closedJournals);

                            //UPDATE GL PERIOD
                            canceled.Status = DOCSTATUS.CANCEL;
                            canceled.UnclosedBy = userId;
                            canceled.UnclosedDate = DateTime.Now;

                            _context.GLClosingHeaders.Update(canceled);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[UnCloseMonth] ****** ERROR ***** " + ex.Message);
                    Console.WriteLine("[UnCloseMonth] ****** STACK ***** " + ex.StackTrace);

                    response.Valid = false;
                    response.ErrorMessage = string.Format("UnClose Month failed ! {0}", ex.Message);
                }
            }
            else
            {
                response.Valid = false;
                response.ErrorMessage = string.Format("UnClose Month parameters not valid !");
            }

            return response;
        }

        #endregion Closing
    }

    public class JournalResponse
    {
        public bool Valid { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidMessage { get; set; }

        /// <summary>
        /// If required, set Valid Status with expected status
        /// </summary>
        public int ValidStatus { get; set; }
    }

    class TaxScheduleAccount
    {
        public string AccountId { get; set; }
        public decimal TaxAmount { get; set; }
        public TaxSchedule TaxSchedule { get; set; }
    }

    class ChargeDetail
    {
        public string AccountId { get; set; }
        public Guid ChargesId { get; set; }
        public string ChargesName { get; set; }
        public string JournalNote { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal OriginatingDiscount { get; set; }
        public decimal OriginatingTax { get; set; }
        public decimal OriginatingExtended { get; set; }
        public decimal FunctionalAmount { get; set; }
        public decimal FunctionalDiscount { get; set; }
        public decimal FunctionalTax { get; set; }
        public decimal FunctionalExtended { get; set; }
        public TaxSchedule TaxSchedule { get; set; } 
    }
}