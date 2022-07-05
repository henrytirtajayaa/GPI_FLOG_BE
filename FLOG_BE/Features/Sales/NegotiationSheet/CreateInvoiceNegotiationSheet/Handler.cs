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
using FLOG_BE.Model.Central;
using Infrastructure;

namespace FLOG_BE.Features.Sales.NegotiationSheet.CreateInvoiceNegotiationSheet
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _flogContext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext flogContext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _flogContext = flogContext;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _docGenerator = new DocumentGenerator(_context);

        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.NegotiationSheetHeaders.FirstOrDefaultAsync(x => x.NegotiationSheetId == request.Body.NegotiationSheetId);

                    string docNo = "";
                    if(entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        var validResp = await CreateInvoices(entryHeader, request);

                        if (validResp.Valid)
                        {
                            transaction.Commit();

                            var response = new Response()
                            {
                                NegotiationSheetId = request.Body.NegotiationSheetId,
                                Message = string.Format("Invoices for {0} successfully created.", docNo)
                            };

                            return ApiResult<Response>.Ok(response);
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(validResp.ErrorMessage);
                        }                        
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Document not available.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine("[CreateInvoiceNegotiationSheet] ***** ERROR ****** " + ex.Message);
                    Console.WriteLine("[CreateInvoiceNegotiationSheet] ***** ERROR ****** " + ex.StackTrace);

                    return ApiResult<Response>.InternalServerError("[CreateInvoiceNegotiationSheet] " + ex.Message);
                }
            }
        }

        private async Task<JournalResponse> CreateInvoices(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            JournalResponse validInvoiceReceivables = new JournalResponse { Valid = true, ErrorMessage = "" };
            if(req.Body.NsSellings != null)
            {
                validInvoiceReceivables = await CreateInvoiceReceivables(ns, req);
            }

            if (!validInvoiceReceivables.Valid)
            {
                return validInvoiceReceivables;
            }

            JournalResponse validInvoicePayables = new JournalResponse { Valid = true, ErrorMessage = "" };
            if (req.Body.NsBuyings != null)
            {
                validInvoicePayables = await CreateInvoicePayables(ns, req);
            }

            if (!validInvoicePayables.Valid)
            {
                return validInvoicePayables;
            }

            return resp;
        }

        private async Task<JournalResponse> CreateInvoiceReceivables(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            List<Guid> nsSellingIds = req.Body.NsSellings.Select(s => s.NsSellingId).ToList();

            var sellings = _context.NegotiationSheetSellings.Where(x => x.NegotiationSheetId == ns.NegotiationSheetId &&
                    nsSellingIds.Any(s => s.Equals(x.NsSellingId))).AsQueryable();
            
            //GROUP BY Exchange Rate, Multiply, Transaction Date
            var groupSellings = (from sell in sellings
                                    group sell by new
                                    {
                                        sell.CustomerId,
                                        sell.CurrencyCode,
                                        sell.ExchangeRate,
                                        sell.IsMultiply
                                    } into grp
                                    select new
                                    {
                                        CustomerId = grp.Key.CustomerId,
                                        CurrencyCode = grp.Key.CurrencyCode,
                                        ExchangeRate = grp.Key.ExchangeRate,
                                        IsMultiply = grp.Key.IsMultiply
                                    }).ToList();

            foreach (var group in groupSellings)
            {
                var items = sellings.Where(g => g.CustomerId == group.CustomerId && g.CurrencyCode == group.CurrencyCode && g.ExchangeRate == group.ExchangeRate && g.IsMultiply == group.IsMultiply).ToList();

                //CREATE INVOICE RECEIVABLES
                var invoice = await this.CreateARInvoice(ns, items, req);

                if (!invoice.Valid)
                {
                    resp = invoice;
                    break;
                }
            }

            return resp;
        }

        private async Task<JournalResponse> CreateARInvoice(Entities.NegotiationSheetHeader header, List<Entities.NegotiationSheetSelling> listSellings, Request request)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            if (listSellings != null && listSellings.Count > 0)
            {
                Entities.NegotiationSheetSelling selling = listSellings.FirstOrDefault();

                Entities.Customer theCustomer = _context.Customers.Where(x => x.CustomerId == selling.CustomerId).FirstOrDefault();

                if(theCustomer != null)
                {
                    string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.InvoiceDate, TRX_MODULE.TRX_RECEIVABLE, DOCNO_FEATURE.TRXTYPE_INVOICE, header.TransactionType, _context.Database.CurrentTransaction.GetDbTransaction());

                    if (!string.IsNullOrEmpty(documentUniqueNo))
                    {
                        decimal totalExtOriginating = listSellings.Sum(x => x.OriginatingExtendedAmount);
                        decimal totalExtFunctional = listSellings.Sum(x => x.FunctionalExtendedAmount);

                        decimal totalAmount = listSellings.Sum(x => x.OriginatingExtendedAmount);
                        
                        var receiveHeader = new Entities.ReceivableTransactionHeader()
                        {
                            ReceiveTransactionId = Guid.NewGuid(),
                            DocumentType = DOCUMENTTYPE.INVOICE,
                            DocumentNo = documentUniqueNo,
                            TransactionDate = request.Body.InvoiceDate,
                            TransactionType = header.TransactionType,
                            CurrencyCode = selling.CurrencyCode,
                            ExchangeRate = selling.ExchangeRate,
                            IsMultiply = selling.IsMultiply,
                            CustomerId = selling.CustomerId,
                            SoDocumentNo = header.SoDocumentNo,
                            NsDocumentNo = header.DocumentNo,
                            Description = "Receivable Invoice for Master No #" + header.MasterNo,
                            SubtotalAmount = totalAmount,
                            DiscountAmount = 0,
                            TaxAmount = 0,
                            Status = DOCSTATUS.POST,
                            CreatedBy = request.Initiator.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CustomerAddressCode = theCustomer.AddressCode,
                            BillToAddressCode = theCustomer.BillToAddressCode,
                            ShipToAddressCode = theCustomer.ShipToAddressCode,
                            PaymentTermCode = theCustomer.PaymentTermCode,
                        };

                        _context.ReceivableTransactionHeaders.Add(receiveHeader);

                        //INSERT DETAILS
                        List<Entities.ReceivableTransactionDetail> details = new List<Entities.ReceivableTransactionDetail>();

                        foreach (var item in listSellings)
                        {
                            var receiveDetail = new Entities.ReceivableTransactionDetail()
                            {
                                TransactionDetailId = Guid.NewGuid(),
                                ReceiveTransactionId = receiveHeader.ReceiveTransactionId,
                                ChargesId = item.ChargeId,
                                ChargesDescription = item.Remark,
                                OriginatingAmount = item.OriginatingAmount,
                                OriginatingTax = item.OriginatingTax,
                                OriginatingDiscount = item.OriginatingDiscount,
                                OriginatingExtendedAmount = item.OriginatingExtendedAmount,
                                FunctionalTax = item.FunctionalTax,
                                FunctionalDiscount = item.FunctionalDiscount,
                                FunctionalExtendedAmount = CALC.FunctionalAmount(item.IsMultiply, item.OriginatingAmount, item.ExchangeRate),
                                TaxScheduleId = item.TaxScheduleId,
                                IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                                PercentDiscount = item.PercentDiscount,
                                Status = DOCSTATUS.POST,
                            };

                            details.Add(receiveDetail);

                            //UPDATE BANK RECONCILIATION ADJUSTMENT
                            item.ReceiveTransactionId = receiveHeader.ReceiveTransactionId;

                            _context.NegotiationSheetSellings.Update(item);
                        }

                        await _context.ReceivableTransactionDetails.AddRangeAsync(details);

                        //UDATE NS SELLING
                        List<Entities.ReceivableTransactionTax> taxes = new List<Entities.ReceivableTransactionTax>();
                        var journal = await _financeManager.CreateDistributionJournalAsync(receiveHeader, details, taxes);

                        if (journal.Valid)
                        {
                            resp.ValidMessage = receiveHeader.DocumentNo;
                            resp.ValidStatus = receiveHeader.Status;

                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            resp = journal;
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Receivable Invoice Document No can not be generated !";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Customer data not found ! Invoice(s) can not be created.";
                }                            
            }

            return resp;
        }

        private async Task<JournalResponse> CreateInvoicePayables(Entities.NegotiationSheetHeader ns, Request req)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            List<Guid> nsBuyingIds = req.Body.NsBuyings.Select(s => s.NsBuyingId).ToList();

            var buyings = _context.NegotiationSheetBuyings.Where(x => x.NegotiationSheetId == ns.NegotiationSheetId && nsBuyingIds.Any(s => s.Equals(x.NsBuyingId))).AsQueryable();

            //GROUP BY Exchange Rate, Multiply, Transaction Date
            var groupBuyings = (from buy in buyings
                                 group buy by new
                                 {
                                     buy.VendorId,
                                     buy.CurrencyCode,
                                     buy.ExchangeRate,
                                     buy.IsMultiply
                                 } into grp
                                 select new
                                 {
                                     VendorId = grp.Key.VendorId,
                                     CurrencyCode = grp.Key.CurrencyCode,
                                     ExchangeRate = grp.Key.ExchangeRate,
                                     IsMultiply = grp.Key.IsMultiply
                                 }).ToList();

            foreach (var g in buyings)
            {
                Console.WriteLine("[CreateInvoicePayables] **************** rowId " + g.RowId);
            }

            foreach (var group in groupBuyings)
            {
                var items = buyings.Where(g => g.VendorId == group.VendorId && g.CurrencyCode == group.CurrencyCode && g.ExchangeRate == group.ExchangeRate && g.IsMultiply == group.IsMultiply).ToList();

                //CREATE INVOICE RECEIVABLES
                
                var invoice = await this.CreateAPInvoice(ns, items, req);

                if (!invoice.Valid)
                {
                    resp = invoice;
                    break;
                }
            }

            return resp;
        }

        private async Task<JournalResponse> CreateAPInvoice(Entities.NegotiationSheetHeader header, List<Entities.NegotiationSheetBuying> listBuyings, Request request)
        {
            JournalResponse resp = new JournalResponse { Valid = true, ErrorMessage = "", ValidMessage = "", ValidStatus = 0 };

            if (listBuyings != null && listBuyings.Count > 0)
            {
                Entities.NegotiationSheetBuying buying = listBuyings.FirstOrDefault();
                Entities.Vendor theVendor = _context.Vendors.Where(x => x.VendorId == buying.VendorId).FirstOrDefault();

                if(theVendor != null)
                {
                    string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.InvoiceDate, TRX_MODULE.TRX_PAYABLE, DOCNO_FEATURE.TRXTYPE_INVOICE, header.TransactionType, _context.Database.CurrentTransaction.GetDbTransaction());

                    if (!string.IsNullOrEmpty(documentUniqueNo))
                    {
                        decimal totalExtOriginating = listBuyings.Sum(x => x.OriginatingExtendedAmount);
                        decimal totalExtFunctional = listBuyings.Sum(x => x.FunctionalExtendedAmount);

                        decimal totalAmount = listBuyings.Sum(x => x.OriginatingExtendedAmount);
                        
                        var payableHeader = new Entities.PayableTransactionHeader()
                        {
                            PayableTransactionId = Guid.NewGuid(),
                            DocumentType = DOCUMENTTYPE.INVOICE,
                            DocumentNo = documentUniqueNo,
                            TransactionDate = request.Body.InvoiceDate,
                            TransactionType = header.TransactionType,
                            CurrencyCode = buying.CurrencyCode,
                            ExchangeRate = buying.ExchangeRate,
                            IsMultiply = buying.IsMultiply,
                            VendorId = buying.VendorId,
                            NsDocumentNo = header.DocumentNo,
                            VendorDocumentNo = header.MasterNo,
                            Description = "Payable Invoice for Master No #" + header.MasterNo,
                            SubtotalAmount = totalAmount,
                            DiscountAmount = 0,
                            TaxAmount = 0,
                            Status = DOCSTATUS.POST,
                            CreatedBy = request.Initiator.UserId,
                            CreatedDate = DateTime.Now,
                            PaymentTermCode = theVendor.PaymentTermCode,
                            VendorAddressCode = theVendor.AddressCode,
                            BillToAddressCode = theVendor.BillToAddressCode,
                            ShipToAddressCode = theVendor.ShipToAddressCode,
                        };

                        _context.PayableTransactionHeaders.Add(payableHeader);

                        //INSERT DETAILS
                        List<Entities.PayableTransactionDetail> details = new List<Entities.PayableTransactionDetail>();

                        foreach (var item in listBuyings)
                        {
                            var payableDetail = new Entities.PayableTransactionDetail()
                            {
                                TransactionDetailId = Guid.NewGuid(),
                                PayableTransactionId = payableHeader.PayableTransactionId,
                                ChargesId = item.ChargeId,
                                ChargesDescription = item.Remark,
                                OriginatingAmount = item.OriginatingAmount,
                                OriginatingTax = item.OriginatingTax,
                                OriginatingDiscount = item.OriginatingDiscount,
                                OriginatingExtendedAmount = item.OriginatingExtendedAmount,
                                FunctionalTax = item.FunctionalTax,
                                FunctionalDiscount = item.FunctionalDiscount,
                                FunctionalExtendedAmount = CALC.FunctionalAmount(item.IsMultiply, item.OriginatingAmount, item.ExchangeRate),
                                TaxScheduleId = item.TaxScheduleId,
                                IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                                PercentDiscount = item.PercentDiscount,
                                Status = DOCSTATUS.POST,
                            };

                            details.Add(payableDetail);

                            //UPDATE NS BUYING
                            item.PayableTransactionId = payableHeader.PayableTransactionId;

                            _context.NegotiationSheetBuyings.Update(item);
                        }

                        await _context.PayableTransactionDetails.AddRangeAsync(details);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        List<Entities.PayableTransactionTax> taxes = new List<Entities.PayableTransactionTax>();
                        var journal = await _financeManager.CreateDistributionJournalAsync(payableHeader, details, taxes);

                        if (journal.Valid)
                        {
                            resp.ValidMessage = payableHeader.DocumentNo;
                            resp.ValidStatus = payableHeader.Status;

                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            resp = journal;
                        }
                    }
                    else
                    {
                        resp.Valid = false;
                        resp.ErrorMessage = "Payable Invoice Document No can not be generated !";
                    }
                }
                else
                {
                    resp.Valid = false;
                    resp.ErrorMessage = "Vendor data not found ! Invoice(s) can not be created.";
                }
               
            }

            return resp;
        }


    }
}
