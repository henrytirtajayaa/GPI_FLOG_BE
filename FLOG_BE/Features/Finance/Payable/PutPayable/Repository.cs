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

namespace FLOG_BE.Features.Finance.Payable.PutPayable
{
    public class Repository
    {
        private readonly CompanyContext _context;

        public Repository(CompanyContext context)
        {
            _context = context;
        }
        
        public async Task<Entities.PayableTransactionHeader> UpdateHeader(RequestPayable body, UserLogin user)
        {
            var payableTrx = await _context.PayableTransactionHeaders.FirstOrDefaultAsync(x => x.PayableTransactionId == body.PayableTransactionId);

            if (payableTrx != null)
            {
                payableTrx.BranchCode = body.BranchCode;
                payableTrx.TransactionDate = body.TransactionDate;
                payableTrx.TransactionType = body.TransactionType;
                payableTrx.CurrencyCode = body.CurrencyCode;
                payableTrx.ExchangeRate = body.ExchangeRate;
                payableTrx.IsMultiply = body.IsMultiply;
                payableTrx.VendorId = body.VendorId;
                payableTrx.PaymentTermCode = body.PaymentTermCode;
                payableTrx.VendorAddressCode = body.VendorAddressCode;
                payableTrx.VendorDocumentNo = body.VendorDocumentNo;
                payableTrx.NsDocumentNo = body.NsDocumentNo;
                payableTrx.Description = body.Description;
                payableTrx.SubtotalAmount = body.SubtotalAmount;
                payableTrx.DiscountAmount = body.DiscountAmount;
                payableTrx.TaxAmount = body.TaxAmount;
                payableTrx.TaxAmount = body.TaxAmount;
                payableTrx.StatusComment = body.StatusComment;
                payableTrx.ModifiedBy = user.UserId;
                payableTrx.ModifiedDate = DateTime.Now;
                payableTrx.BillToAddressCode = body.BillToAddressCode;
                payableTrx.ShipToAddressCode = body.ShipToAddressCode;

                _context.PayableTransactionHeaders.Update(payableTrx);
            }

            return payableTrx;
        }

        public async Task<List<Entities.PayableTransactionDetail>> InsertPayableDetails(RequestPayable body)
        {
            List<Entities.PayableTransactionDetail> details = new List<Entities.PayableTransactionDetail>();

            if (body.RequestPayableDetails != null)
            {
                //REMOVE EXISTING
                _context.PayableTransactionDetails
               .Where(x => x.PayableTransactionId == body.PayableTransactionId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.RequestPayableDetails)
                {
                    var payableDetail = new Entities.PayableTransactionDetail()
                    {
                        TransactionDetailId = Guid.NewGuid(),
                        PayableTransactionId = body.PayableTransactionId,
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

                    details.Add(payableDetail);
                }

                if (details.Count > 0)
                    await _context.PayableTransactionDetails.AddRangeAsync(details);
            }

            return details;
        }

        public async Task<List<Entities.PayableTransactionTax>> InsertPayableTax(RequestPayable body)
        {
            List<Entities.PayableTransactionTax> taxes = new List<Entities.PayableTransactionTax>();

            if (body.RequestPayableTaxes != null)
            {
                //REMOVE EXISTING
                _context.PayableTransactionTaxes
               .Where(x => x.PayableTransactionId == body.PayableTransactionId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS Tax
                foreach (var item in body.RequestPayableTaxes)
                {
                    var tax = new Entities.PayableTransactionTax()
                    {
                        TransactionTaxId = Guid.NewGuid(),
                        PayableTransactionId = body.PayableTransactionId,
                        TaxScheduleId = item.TaxScheduleId,
                        IsTaxAfterDiscount = item.IsTaxAfterDiscount,
                        TaxScheduleCode = item.TaxScheduleCode,
                        TaxBasePercent = item.TaxBasePercent,
                        TaxBaseOriginatingAmount = item.TaxBaseOriginatingAmount,
                        TaxablePercent = item.TaxablePercent,
                        OriginatingTaxAmount = item.OriginatingTaxAmount,
                        Status = item.Status
                    };

                    taxes.Add(tax);
                }

                if (taxes.Count > 0)
                    await _context.PayableTransactionTaxes.AddRangeAsync(taxes);
            }

            return taxes;
        }

    }
}
