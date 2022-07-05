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

namespace FLOG_BE.Features.Finance.Receivable.PutReceivableTransaction
{
    public class Repository
    {
        private readonly CompanyContext _context;

        public Repository(CompanyContext context)
        {
            _context = context;
        }
        
        public async Task<Entities.ReceivableTransactionHeader> UpdateHeader(RequestReceivable body, UserLogin user)
        {
            var receivableTrx = await _context.ReceivableTransactionHeaders.FirstOrDefaultAsync(x => x.ReceiveTransactionId == body.ReceiveTransactionId);

            if (receivableTrx != null)
            {
                receivableTrx.BranchCode = body.BranchCode;
                receivableTrx.TransactionDate = body.TransactionDate;
                receivableTrx.TransactionType = body.TransactionType;
                receivableTrx.CurrencyCode = body.CurrencyCode;
                receivableTrx.ExchangeRate = body.ExchangeRate;
                receivableTrx.IsMultiply = body.IsMultiply;
                receivableTrx.CustomerId = body.CustomerId;
                receivableTrx.PaymentTermCode = body.PaymentTermCode;
                receivableTrx.CustomerAddressCode = body.CustomerAddressCode;
                receivableTrx.SoDocumentNo = body.SoDocumentNo;
                receivableTrx.NsDocumentNo = body.NsDocumentNo;
                receivableTrx.Description = body.Description;
                receivableTrx.SubtotalAmount = body.SubtotalAmount;
                receivableTrx.DiscountAmount = body.DiscountAmount;
                receivableTrx.TaxAmount = body.TaxAmount;
                receivableTrx.TaxAmount = body.TaxAmount;
                receivableTrx.ModifiedBy = user.UserId;
                receivableTrx.ModifiedDate = DateTime.Now;
                receivableTrx.BillToAddressCode = body.BillToAddressCode;
                receivableTrx.ShipToAddressCode = body.ShipToAddressCode;

                _context.ReceivableTransactionHeaders.Update(receivableTrx);
            }

            return receivableTrx;
        }

        public async Task<List<Entities.ReceivableTransactionDetail>> InsertReceivableDetails(RequestReceivable body)
        {
            List<Entities.ReceivableTransactionDetail> details = new List<Entities.ReceivableTransactionDetail>();

            if (body.RequestReceivableDetails != null)
            {
                //REMOVE EXISTING
                _context.ReceivableTransactionDetails
               .Where(x => x.ReceiveTransactionId == body.ReceiveTransactionId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.RequestReceivableDetails)
                {
                    var ReceivableDetails = new Entities.ReceivableTransactionDetail()
                    {
                        TransactionDetailId = Guid.NewGuid(),
                        ReceiveTransactionId = body.ReceiveTransactionId,
                        ChargesId = item.ChargesId,
                        ChargesDescription = item.ChargesDescription,
                        OriginatingAmount = item.OriginatingAmount,
                        OriginatingTax = item.OriginatingTax,
                        OriginatingDiscount = item.OriginatingDiscount,
                        OriginatingExtendedAmount = item.OriginatingExtendedAmount,
                        FunctionalTax = item.FunctionalTax,
                        FunctionalDiscount = item.FunctionalDiscount,
                        FunctionalExtendedAmount = item.FunctionalExtendedAmount,
                        Status = item.Status,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        PercentDiscount = item.PercentDiscount
                    };

                    details.Add(ReceivableDetails);
                }

                if (details.Count > 0)
                    await _context.ReceivableTransactionDetails.AddRangeAsync(details);
            }

            return details;
        }

        public async Task<List<Entities.ReceivableTransactionTax>> InsertReceivableTax(RequestReceivable body)
        {
            List<Entities.ReceivableTransactionTax> taxes = new List<Entities.ReceivableTransactionTax>();

            if (body.RequestReceivableTaxes != null)
            {
                //REMOVE EXISTING
                _context.ReceivableTransactionTaxes
               .Where(x => x.ReceiveTransactionId == body.ReceiveTransactionId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS Tax
                foreach (var item in body.RequestReceivableTaxes)
                {
                    var ReceivableTax = new Entities.ReceivableTransactionTax()
                    {
                        TransactionTaxId = Guid.NewGuid(),
                        ReceiveTransactionId = body.ReceiveTransactionId,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        TaxScheduleCode = item.TaxScheduleCode,
                        TaxBasePercent = item.TaxBasePercent,
                        TaxBaseOriginatingAmount = item.TaxBaseOriginatingAmount,
                        TaxablePercent = item.TaxablePercent,
                        OriginatingTaxAmount = item.OriginatingTaxAmount,
                        Status = DOCSTATUS.NEW
                    };

                    taxes.Add(ReceivableTax);
                }

                if (taxes.Count > 0)
                    await _context.ReceivableTransactionTaxes.AddRangeAsync(taxes);
            }

            return taxes;
        }

    }
}
