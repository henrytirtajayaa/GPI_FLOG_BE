﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies.Entities;
using FLOG_BE.Model.Companies.Mapping;
using FLOG_BE.Model.Fetcher;
using FLOG_BE.Model.Companies.View;

namespace FLOG_BE.Model.Companies
{
    public class CompanyContext : DbContext
    {
        private readonly ICompanyFetcher _companyFetcher;

        public CompanyContext(DbContextOptions<CompanyContext> options, ICompanyFetcher companyFetcher) : base(options)
        {
            _companyFetcher = companyFetcher;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (_companyFetcher != null)
                {
                    var connectionString = _companyFetcher.GetCompanyConnection();
                    
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        optionsBuilder.UseSqlServer(connectionString);
                    }
                    else
                    {
                        throw new Exception($"Could not find database connection string");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[CompanyContext] onconfiguring ...." + e.Message);
                Console.WriteLine("[CompanyContext] onconfiguring ...." + e.StackTrace);
                throw new Exception(e.Message);
            }

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<AccountSegment> AccountSegments { get; set; }
        public virtual DbSet<CompanyBranch> CompanyBranchs { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Port> Ports { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<TaxSchedule> TaxSchedules { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public virtual DbSet<CompanySetup> CompanySetups { get; set; }
        public virtual DbSet<Charges> Charges { get; set; }
        public virtual DbSet<ChargesDetail> ChargesDetails { get; set; }
        public virtual DbSet<FiscalPeriodHeader> FiscalPeriodHeaders { get; set; }
        public virtual DbSet<FiscalPeriodDetail> FiscalPeriodDetails { get; set; }
        public virtual DbSet<NumberFormatHeader> NumberFormatHeaders { get; set; }
        public virtual DbSet<NumberFormatDetail> NumberFormatDetails { get; set; }
        public virtual DbSet<NumberFormatLastNo> NumberFormatLastNos { get; set; }

        public virtual DbSet<Checkbook> Checkbooks { get; set; }
        public virtual DbSet<ApprovalSetupHeader> ApprovalSetupHeaders { get; set; }
        public virtual DbSet<ApprovalSetupDetail> ApprovalSetupDetails { get; set; }
        public virtual DbSet<CustomerGroup> CustomerGroups { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorGroup> VendorGroups { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<VendorAddress> VendorAddresses { get; set; }
        public virtual DbSet<CustomerVendorRelation> CustomerVendorRelations { get; set; }
        public virtual DbSet<FinancialSetup> FinancialSetups { get; set; }
        public virtual DbSet<ExchangeRateHeader> ExchangeRateHeaders { get; set; }
        public virtual DbSet<ExchangeRateDetail> ExchangeRateDetails { get; set; }
        public virtual DbSet<TaxRefferenceNumber> TaxRefferenceNumbers { get; set; }
        public virtual DbSet<ReceivableSetup> ReceivableSetups { get; set; }
        public virtual DbSet<PayableSetup> PayableSetups { get; set; }
        public virtual DbSet<ShippingLine> ShippingLines { get; set; }
        public virtual DbSet<PayableTransactionHeader> PayableTransactionHeaders { get; set; }
        public virtual DbSet<PayableTransactionDetail> PayableTransactionDetails { get; set; }
        public virtual DbSet<Entities.ReceivableTransactionHeader> ReceivableTransactionHeaders { get; set; }
        public virtual DbSet<Entities.ReceivableTransactionDetail> ReceivableTransactionDetails { get; set; }
        public virtual DbSet<CheckbookTransactionHeader> CheckbookTransactionHeaders { get; set; }
        public virtual DbSet<CheckbookTransactionDetail> CheckbookTransactionDetails { get; set; }
        public virtual DbSet<CheckbookTransactionApproval> CheckbookTransactionApprovals { get; set; }
        public virtual DbSet<CheckbookApprovalComment> CheckbookApprovalComments { get; set; }

        public virtual DbSet<ReceivableTransactionTax> ReceivableTransactionTaxes { get; set; }
        public virtual DbSet<PayableTransactionTax> PayableTransactionTaxes { get; set; }

        public virtual DbSet<MSTransactionType> MSTransactionTypes { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<ContainerDepot> ContainerDepots { get; set; }
        public virtual DbSet<MsDepartment> MsDepartments { get; set; }

        #region UOM

        public virtual DbSet<UOMBase> UOMBases { get; set; }
        public virtual DbSet<UOMHeader> UOMHeaders { get; set; }
        public virtual DbSet<UOMDetail> UOMDetails { get; set; }

        #endregion UOM

        #region DISTRIBUTION JOURNAL

        public virtual DbSet<DistributionJournalHeader> DistributionJournalHeaders { get; set; }
        public virtual DbSet<DistributionJournalDetail> DistributionJournalDetails { get; set; }

        #endregion DISTRIBUTION JOURNAL

        public virtual DbSet<ApPaymentHeader> ApPaymentHeaders { get; set; }
        public virtual DbSet<ApPaymentDetail> ApPaymentDetails { get; set; }
        public virtual DbSet<ApPaymentApproval> ApPaymentApprovals { get; set; }
        public virtual DbSet<ApPaymentApprovalComment> ApPaymentApprovalComments { get; set; }

        #region JOURNAL ENTRY

        public virtual DbSet<JournalEntryHeader> JournalEntryHeaders { get; set; }
        public virtual DbSet<JournalEntryDetail> JournalEntryDetails { get; set; }

        #endregion JOURNAL ENTRY
                
        public virtual DbSet<ArReceiptHeader> ArReceiptHeaders { get; set; }
        public virtual DbSet<ArReceiptDetail> ArReceiptDetails { get; set; }
        public virtual DbSet<ChargeGroup> ChargeGroups { get; set; }
        public virtual DbSet<SalesPerson> SalesPersons { get; set; }

        #region SALES 
        public virtual DbSet<SalesQuotationHeader> SalesQuotationHeaders { get; set; }
        public virtual DbSet<SalesQuotationDetail> QuotationDetails { get; set; }
        public virtual DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public virtual DbSet<SalesOrderContainer> SalesOrderContainers { get; set; }
        public virtual DbSet<SalesOrderTrucking> SalesOrderTruckings { get; set; }
        public virtual DbSet<SalesOrderBuying> SalesOrderBuyings { get; set; }
        public virtual DbSet<SalesOrderSelling> SalesOrderSellings { get; set; }
        public virtual DbSet<NegotiationSheetHeader> NegotiationSheetHeaders { get; set; }
        public virtual DbSet<NegotiationSheetSelling> NegotiationSheetSellings { get; set; }
        public virtual DbSet<NegotiationSheetBuying> NegotiationSheetBuyings { get; set; }
        public virtual DbSet<NegotiationSheetContainer> NegotiationSheetContainers { get; set; }
        public virtual DbSet<NegotiationSheetTrucking> NegotiationSheetTruckings { get; set; }
        
        #endregion SALES

        public virtual DbSet<DepositSettlementHeader> DepositSettlementHeaders { get; set; }
        public virtual DbSet<DepositSettlementDetail> DepositSettlementDetails { get; set; }

        #region APPLY 

        public virtual DbSet<APApplyHeader> APApplyHeaders { get; set; }
        public virtual DbSet<APApplyDetail> APApplyDetails { get; set; }

        public virtual DbSet<ARApplyHeader> ARApplyHeaders { get; set; }
        public virtual DbSet<ARApplyDetail> ARApplyDetails { get; set; }

        #endregion APPLY

        #region BANK RECONCILIATION

        public virtual DbSet<BankReconcileHeader> BankReconcileHeaders { get; set; }
        public virtual DbSet<BankReconcileDetail> BankReconcileDetails { get; set; }
        public virtual DbSet<BankReconcileAdjustment> BankReconcileAdjustments { get; set; }

        #endregion BANK RECONCILIATION

        #region FN SETUP
        public virtual DbSet<FNPostingParam> FNPostingParams { get; set; }
        public virtual DbSet<FNDocNumberSetup> FNDocNumberSetups { get; set; }
        public virtual DbSet<FNDocNumberSetupApproval> FNDocNumberSetupApprovals { get; set; }

        #endregion

        #region GL STATEMENT

        public virtual DbSet<GLStatementCategory> GLStatementCategories { get; set; }
        public virtual DbSet<GLStatementSubCategory> GLStatementSubCategories { get; set; }
        public virtual DbSet<GLStatementDetail> GLStatementDetails { get; set; }
        public virtual DbSet<GLStatementDetailSub> GLStatementDetailSubs { get; set; }
        public virtual DbSet<GLClosingHeader> GLClosingHeaders { get; set; }
        public virtual DbSet<GLClosingDetail> GLClosingDetails { get; set; }

        #endregion GL STATEMENT

        #region CONTAINER
        public virtual DbSet<Container> Containers { get; set; }
        public virtual DbSet<SetupContainerRental> SetupContainerRentals { get; set; }
        public virtual DbSet<ContainerRentalRequestHeader> ContainerRentalRequestHeaders { get; set; }
        public virtual DbSet<ContainerRentalRequestDetail> ContainerRentalRequestDetails { get; set; }
        public virtual DbSet<ContainerRequestConfirmHeader> ContainerRequestConfirmHeaders { get; set; }
        public virtual DbSet<ContainerRequestConfirmDetail> ContainerRequestConfirmDetails { get; set; }
        #endregion CONTAINER

        #region TRX DOCUMENT APPROVAL

        public virtual DbSet<TrxDocumentApproval> TrxDocumentApprovals { get; set; }
        public virtual DbSet<TrxDocumentApprovalComment> TrxDocumentApprovalComments { get; set; }

        #endregion TRX DOCUMENT APPROVAL

        #region VIEW
        public DbQuery<APPayablePending> APPayableInvoicePending { get; set; }
        public DbQuery<ARReceivablePending> ARReceivablePendings { get; set; }
        public DbQuery<APCreditNote> APPendingCreditNotes { get; set; }
        public DbQuery<APAdvancePayment> APPendingAdvancePayments { get; set; }
        public DbQuery<ARCreditNote> ARPendingCreditNotes { get; set; }
        public DbQuery<ARAdvanceReceipt> ARPendingAdvanceReceipts { get; set; }
        public DbQuery<ContainerConfirmQuantity> ContainerConfirmQuantities { get; set; }
        public DbQuery<DepositReceivablePending> DepositReceivablePendings { get; set; }
        public DbQuery<UnapplyDeposit> UnapplyDeposits { get; set; }
        public DbQuery<ARUnapplyReceipt> ARUnapplyReceipts { get; set; }

        public DbQuery<APUnapplyPayment> APUnapplyPayments { get; set; }

        #endregion VIEW

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Generated Configuration
            modelBuilder.ApplyConfiguration(new AccountSegmentMap());
            modelBuilder.ApplyConfiguration(new BankMap());
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new CountryMap());
            modelBuilder.ApplyConfiguration(new PortMap());
            modelBuilder.ApplyConfiguration(new ReferenceMap());
            modelBuilder.ApplyConfiguration(new ContainerMap());
            modelBuilder.ApplyConfiguration(new AccountMap());
            modelBuilder.ApplyConfiguration(new PaymentTermMap());
            modelBuilder.ApplyConfiguration(new CompanyAddressMap());
            modelBuilder.ApplyConfiguration(new TaxScheduleMap());
            modelBuilder.ApplyConfiguration(new CompanySetupMap());
            modelBuilder.ApplyConfiguration(new CurrencyMap());
            modelBuilder.ApplyConfiguration(new ChargesMap());
            modelBuilder.ApplyConfiguration(new ChargesDetailMap());
            modelBuilder.ApplyConfiguration(new FiscalPeriodHeaderMap());
            modelBuilder.ApplyConfiguration(new FiscalPeriodDetailMap());
            modelBuilder.ApplyConfiguration(new NumberFormatHeaderMap());
            modelBuilder.ApplyConfiguration(new NumberFormatDetailMap());
            modelBuilder.ApplyConfiguration(new NumberFormatLastNoMap());
            modelBuilder.ApplyConfiguration(new CheckbookMap());
            modelBuilder.ApplyConfiguration(new ApprovalSetupHeaderMap());
            modelBuilder.ApplyConfiguration(new ApprovalSetupDetailMap());
            modelBuilder.ApplyConfiguration(new CustomerGroupMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new VendorMap());
            modelBuilder.ApplyConfiguration(new VendorGroupMap());
            modelBuilder.ApplyConfiguration(new CustomerAddressMap());
            modelBuilder.ApplyConfiguration(new CustomerVendorRelationMap());
            modelBuilder.ApplyConfiguration(new VendorAddressMap());
            modelBuilder.ApplyConfiguration(new FinancialSetupMap());
            modelBuilder.ApplyConfiguration(new ExchangeRateDetailMap());
            modelBuilder.ApplyConfiguration(new ExchangeRateHeaderMap());
            modelBuilder.ApplyConfiguration(new TaxRefferenceNumberMap());
            modelBuilder.ApplyConfiguration(new ReceivableSetupMap());
            modelBuilder.ApplyConfiguration(new PayableSetupMap());
            modelBuilder.ApplyConfiguration(new ShippingLineMap());
            modelBuilder.ApplyConfiguration(new PayableTransactionHeaderMap());
            modelBuilder.ApplyConfiguration(new PayableTransactionDetailMap());
            modelBuilder.ApplyConfiguration(new ReceivableTransactionHeaderMap());
            modelBuilder.ApplyConfiguration(new ReceivableTransactionDetailMap());
            modelBuilder.ApplyConfiguration(new CheckbookTransactionHeaderMap());
            modelBuilder.ApplyConfiguration(new CheckbookTransactionDetailMap());
            modelBuilder.ApplyConfiguration(new ReceivableTransactionTaxMap());
            modelBuilder.ApplyConfiguration(new PayableTransactionTaxMap());
            modelBuilder.ApplyConfiguration(new CheckbookTransactionApprovalMap());
            modelBuilder.ApplyConfiguration(new CheckbookApprovalCommentMap());
            modelBuilder.ApplyConfiguration(new JournalEntryHeaderMap());
            modelBuilder.ApplyConfiguration(new JournalEntryDetailMap());
            modelBuilder.ApplyConfiguration(new ApPaymentHeaderMap());
            modelBuilder.ApplyConfiguration(new ApPaymentDetailMap());
            modelBuilder.ApplyConfiguration(new ArReceiptHeaderMap());
            modelBuilder.ApplyConfiguration(new ArReceiptDetailMap());
            modelBuilder.ApplyConfiguration(new ApPaymentApprovalMap());
            modelBuilder.ApplyConfiguration(new ApPaymentApprovalCommentMap());
            modelBuilder.ApplyConfiguration(new APApplyHeaderMap());
            modelBuilder.ApplyConfiguration(new APApplyDetailMap());
            modelBuilder.ApplyConfiguration(new ARApplyHeaderMap());
            modelBuilder.ApplyConfiguration(new ARApplyDetailMap());
            modelBuilder.ApplyConfiguration(new BankReconcileHeaderMap());
            modelBuilder.ApplyConfiguration(new BankReconcileDetailMap());
            modelBuilder.ApplyConfiguration(new BankReconcileAdjustmentMap());
            modelBuilder.ApplyConfiguration(new ChargeGroupMap());
            modelBuilder.ApplyConfiguration(new SalesPersonMap());
            modelBuilder.ApplyConfiguration(new SalesPersonMap());
            modelBuilder.ApplyConfiguration(new MSTransactionTypeMap());
            modelBuilder.ApplyConfiguration(new SalesQuotationHeaderMap());
            modelBuilder.ApplyConfiguration(new SalesQuotationDetailMap());
            modelBuilder.ApplyConfiguration(new SalesOrderHeaderMap());
            modelBuilder.ApplyConfiguration(new SalesOrderContainerMap());
            modelBuilder.ApplyConfiguration(new VehicleTypeMap());
            modelBuilder.ApplyConfiguration(new SetupContainerRentalMap());
            modelBuilder.ApplyConfiguration(new ContainerRentalRequestHeaderMap());
            modelBuilder.ApplyConfiguration(new ContainerRentalRequestDetailMap());
            modelBuilder.ApplyConfiguration(new SalesOrderTruckingMap());
            modelBuilder.ApplyConfiguration(new SalesOrderBuyingMap());
            modelBuilder.ApplyConfiguration(new SalesOrderSellingMap());
            modelBuilder.ApplyConfiguration(new ContainerDepotMap());
            modelBuilder.ApplyConfiguration(new ContainerRequestConfirmHeaderMap());
            modelBuilder.ApplyConfiguration(new ContainerRequestConfirmDetailMap());
            modelBuilder.ApplyConfiguration(new NegotiationSheetHeaderMap());
            modelBuilder.ApplyConfiguration(new MSDepartmentMap());
            modelBuilder.ApplyConfiguration(new NegotiationSheetSellingMap());
            modelBuilder.ApplyConfiguration(new NegotiationSheetContainerMap());
            modelBuilder.ApplyConfiguration(new NegotiationSheetTruckingMap());
            modelBuilder.ApplyConfiguration(new NegotiationSheetBuyingMap());
            modelBuilder.ApplyConfiguration(new TrxDocumentApprovalMap());
            modelBuilder.ApplyConfiguration(new TrxDocumentApprovalCommentMap());
            modelBuilder.ApplyConfiguration(new DepositSettlementHeaderMap());
            modelBuilder.ApplyConfiguration(new DepositSettlementDetailMap());

            #endregion

            modelBuilder.Entity<ShippingLine>()
                .HasOne(p => p.Vendor)
                .WithMany(p => p.ShippingLines)
                .HasForeignKey(k => k.VendorId)
                .HasPrincipalKey(k => k.VendorId);

            modelBuilder.Entity<Bank>()
               .HasOne(p => p.Cities)
               .WithMany(p => p.Banks)
               .HasForeignKey(k => k.CityCode)
               .HasPrincipalKey(k => k.CityCode);

            modelBuilder.Entity<ContainerDepot>()
               .HasOne(p => p.Cities)
               .WithMany(p => p.ContainerDepots)
               .HasForeignKey(k => k.CityCode)
               .HasPrincipalKey(k => k.CityCode);

            modelBuilder.Entity<Country>()
              .HasMany(p => p.Cities)
              .WithOne(p => p.Country)
              .HasForeignKey(k => k.CountryID)
              .HasPrincipalKey(k => k.CountryId);

            modelBuilder.Entity<Port>()
               .HasOne(p => p.Cities)
               .WithMany(p => p.Ports)
               .HasForeignKey(k => k.CityCode)
               .HasPrincipalKey(k => k.CityCode);
            
            modelBuilder.Entity<NumberFormatHeader>()
              .HasMany(p => p.NumberFormatDetails)
              .WithOne(p => p.NumberFormatHeader)
              .HasForeignKey(k => k.FormatHeaderId)
              .HasPrincipalKey(k => k.FormatHeaderId);

            modelBuilder.Entity<CustomerVendorRelation>()
              .HasOne(p => p.Customers)
              .WithMany(p => p.CustomerVendorRelations)
              .HasForeignKey(k => k.CustomerId)
              .HasPrincipalKey(k => k.CustomerId);
            
            modelBuilder.Entity<CustomerVendorRelation>()
              .HasOne(p => p.Vendors)
              .WithMany(p => p.CustomerVendorRelations)
              .HasForeignKey(k => k.VendorId)
              .HasPrincipalKey(k => k.VendorId);

            modelBuilder.Entity<CustomerVendorRelation>()
             .HasOne(p => p.Customers)
             .WithMany(p => p.CustomerVendorRelations)
             .HasForeignKey(k => k.CustomerId)
             .HasPrincipalKey(k => k.CustomerId);

            modelBuilder.Entity<CustomerVendorRelation>()
              .HasOne(p => p.Vendors)
              .WithMany(p => p.CustomerVendorRelations)
              .HasForeignKey(k => k.VendorId)
              .HasPrincipalKey(k => k.VendorId);

            //ADDING DB VIEWS
            modelBuilder.Query<APPayablePending>().ToView("vw_pending_payable");
            modelBuilder.Query<ARReceivablePending>().ToView("vw_pending_receivable");
            modelBuilder.Query<APAdvancePayment>().ToView("vw_pending_ap_advance");
            modelBuilder.Query<APUnapplyPayment>().ToView("vw_unapply_ap_payment");
            modelBuilder.Query<APCreditNote>().ToView("vw_pending_ap_credit_note");
            modelBuilder.Query<ARUnapplyReceipt>().ToView("vw_unapply_ar_receipt");
            modelBuilder.Query<ARAdvanceReceipt>().ToView("vw_pending_ar_advance");
            modelBuilder.Query<ARCreditNote>().ToView("vw_pending_ar_credit_note");
            modelBuilder.Query<ContainerConfirmQuantity>().ToView("vw_container_confirm_quantity");
            modelBuilder.Query<DepositReceivablePending>().ToView("vw_pending_deposit_receivable");
            modelBuilder.Query<UnapplyDeposit>().ToView("vw_unapply_deposit");
        }
    }
}
