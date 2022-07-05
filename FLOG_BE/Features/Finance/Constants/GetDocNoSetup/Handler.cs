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
using LinqKit;
using FLOG.Core.Finance;
using FLOG.Core;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Finance.Constants.GetDocNoSetup
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
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var features  = GetFeatureSetups();
            var trxTypes = GetTrxTypeSetups();

            List<ResponseTrxModule> trxModules = new List<ResponseTrxModule>();
            foreach (var trx in GetDocTrxModules())
            {
                trxModules.Add(new ResponseTrxModule { TrxModule = trx.Key, Caption = trx.Value });
            }

            return ApiResult<Response>.Ok(new Response()
            {
                DocNumberSetups = features,
                TrxTypeSetups = trxTypes,
                TrxModules = trxModules,
                ListInfo = null
            }); ;
        }

        private List<ResponseHeader> GetFeatureSetups()
        {
            List<ResponseHeader> headers = new List<ResponseHeader>();

            List<Entities.FNDocNumberSetup> datas = _context.FNDocNumberSetups.AsNoTracking().ToList();

            #region NO TRX TYPE
            List<ResponseItem> listFeature = new List<ResponseItem>();

            //GJE
            var param = datas.Where(x => x.DocFeatureId == DOCNO_FEATURE.NOTRX_GJE).FirstOrDefault();

            listFeature.Add(new ResponseItem
            {
                TrxModule = TRX_MODULE.TRX_GENERAL_JOURNAL,
                DocFeatureId = DOCNO_FEATURE.NOTRX_GJE,
                TransactionType = "",
                DocNumberId = (param != null ? param.DocNumberId : 0),
                DocNo = (param != null ? param.DocNo : ""),
                Description = (param != null ? param.Description : "GENERAL JOURNAL ENTRY"),
                ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                  where ap.TrxModule == TRX_MODULE.TRX_GENERAL_JOURNAL && ap.DocFeatureId == DOCNO_FEATURE.NOTRX_GJE  && ap.TransactionType == "" 
                                  orderby ap.ModeStatus ascending
                                  select new ResponseItemApproval { 
                                   DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                   TransactionType = ap.TransactionType, DocFeatureId = ap.DocFeatureId, TrxModule = ap.TrxModule,
                                   ModeStatus = ap.ModeStatus, ApprovalCode = ap.ApprovalCode, 
                                   ModifiedBy = ap.ModifiedBy, ModifiedDate = (DateTime)ap.ModifiedDate
                                  }).Distinct().ToList()
            });

            //RECEIVE APPLY
            param = datas.Where(x => x.DocFeatureId == DOCNO_FEATURE.NOTRX_RECEIVABLE_APPLY).FirstOrDefault();
            
            listFeature.Add(new ResponseItem
            {
                TrxModule = TRX_MODULE.TRX_RECEIVABLE,
                DocFeatureId = DOCNO_FEATURE.NOTRX_RECEIVABLE_APPLY,
                TransactionType = "",
                DocNumberId = (param != null ? param.DocNumberId : 0),
                DocNo = (param != null ? param.DocNo : ""),
                Description = (param != null ? param.Description : "RECEIVABLE APPLY/ALLOCATION"),
                ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                  where ap.TrxModule == TRX_MODULE.TRX_RECEIVABLE && ap.DocFeatureId == DOCNO_FEATURE.NOTRX_RECEIVABLE_APPLY && ap.TransactionType == ""
                                  orderby ap.ModeStatus ascending
                                  select new ResponseItemApproval
                                  {
                                      DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                      TransactionType = ap.TransactionType,
                                      DocFeatureId = ap.DocFeatureId,
                                      TrxModule = ap.TrxModule,
                                      ModeStatus = ap.ModeStatus,
                                      ApprovalCode = ap.ApprovalCode,
                                      ModifiedBy = ap.ModifiedBy,
                                      ModifiedDate = (DateTime)ap.ModifiedDate
                                  }).Distinct().ToList()
            });

            //PAY APPLY
            param = datas.Where(x => x.DocFeatureId == DOCNO_FEATURE.NOTRX_PAYABLE_APPLY).FirstOrDefault();

            listFeature.Add(new ResponseItem
            {
                TrxModule = TRX_MODULE.TRX_PAYABLE,
                DocFeatureId = DOCNO_FEATURE.NOTRX_PAYABLE_APPLY,
                TransactionType = "",
                DocNumberId = (param != null ? param.DocNumberId : 0),
                DocNo = (param != null ? param.DocNo : ""),
                Description = (param != null ? param.Description : "PAYABLE APPLY/ALLOCATION"),
                ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                  where ap.TrxModule == TRX_MODULE.TRX_PAYABLE && ap.DocFeatureId == DOCNO_FEATURE.NOTRX_PAYABLE_APPLY && ap.TransactionType == ""
                                  orderby ap.ModeStatus ascending
                                  select new ResponseItemApproval
                                  {
                                      DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                      TransactionType = ap.TransactionType,
                                      DocFeatureId = ap.DocFeatureId,
                                      TrxModule = ap.TrxModule,
                                      ModeStatus = ap.ModeStatus,
                                      ApprovalCode = ap.ApprovalCode,
                                      ModifiedBy = ap.ModifiedBy,
                                      ModifiedDate = (DateTime)ap.ModifiedDate
                                  }).Distinct().ToList()
            });

            //SOA NO
            param = datas.Where(x => x.DocFeatureId == DOCNO_FEATURE.NOTRX_SOA_NO).FirstOrDefault();

            listFeature.Add(new ResponseItem
            {
                TrxModule = TRX_MODULE.TRX_GENERAL_JOURNAL,
                DocFeatureId = DOCNO_FEATURE.NOTRX_SOA_NO,
                TransactionType = "",
                DocNumberId = (param != null ? param.DocNumberId : 0),
                DocNo = (param != null ? param.DocNo : ""),
                Description = (param != null ? param.Description : "SOA NO"),
                ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                  where ap.TrxModule == TRX_MODULE.TRX_GENERAL_JOURNAL && ap.DocFeatureId == DOCNO_FEATURE.NOTRX_SOA_NO && ap.TransactionType == ""
                                  orderby ap.ModeStatus ascending
                                  select new ResponseItemApproval
                                  {
                                      DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                      TransactionType = ap.TransactionType,
                                      DocFeatureId = ap.DocFeatureId,
                                      TrxModule = ap.TrxModule,
                                      ModeStatus = ap.ModeStatus,
                                      ApprovalCode = ap.ApprovalCode,
                                      ModifiedBy = ap.ModifiedBy,
                                      ModifiedDate = (DateTime)ap.ModifiedDate
                                  }).Distinct().ToList()
            });

            //DEPOSIT SETTLEMENT
            param = datas.Where(x => x.DocFeatureId == DOCNO_FEATURE.NOTRX_DEPOSIT_SETTLEMENT).FirstOrDefault();

            listFeature.Add(new ResponseItem
            {
                TrxModule = TRX_MODULE.TRX_DEPOSIT,
                DocFeatureId = DOCNO_FEATURE.NOTRX_DEPOSIT_SETTLEMENT,
                TransactionType = "",
                DocNumberId = (param != null ? param.DocNumberId : 0),
                DocNo = (param != null ? param.DocNo : ""),
                Description = (param != null ? param.Description : "DEPOSIT SETTLEMENT"),
                ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                  where ap.TrxModule == TRX_MODULE.TRX_DEPOSIT && ap.DocFeatureId == DOCNO_FEATURE.NOTRX_DEPOSIT_SETTLEMENT && ap.TransactionType == ""
                                  orderby ap.ModeStatus ascending
                                  select new ResponseItemApproval
                                  {
                                      DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                      TransactionType = ap.TransactionType,
                                      DocFeatureId = ap.DocFeatureId,
                                      TrxModule = ap.TrxModule,
                                      ModeStatus = ap.ModeStatus,
                                      ApprovalCode = ap.ApprovalCode,
                                      ModifiedBy = ap.ModifiedBy,
                                      ModifiedDate = (DateTime)ap.ModifiedDate
                                  }).Distinct().ToList()
            });

            headers.Add(new ResponseHeader { Group = "FEATURE", Features = listFeature });

            #endregion NO TRX TYPE

            return headers;
        }

        private Dictionary<int, string> GetDocTrxModules()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(TRX_MODULE.TRX_RECEIVABLE, "RECEIVABLE");
            dict.Add(TRX_MODULE.TRX_PAYABLE, "PAYABLE");
            dict.Add(TRX_MODULE.TRX_SALES, "SALES");
            dict.Add(TRX_MODULE.TRX_CONTAINER_RENT, "CONTAINER RENT");
            dict.Add(TRX_MODULE.TRX_DEPOSIT, "RECEIVABLE DEPOSIT");

            return dict;
        }

        private List<ResponseHeader> GetTrxTypeSetups()
        {
            List<ResponseHeader> headers = new List<ResponseHeader>();

            var datas = _context.FNDocNumberSetups.Where(x=>x.TransactionType != string.Empty).OrderBy(o=>o.TransactionType).OrderBy(o=>o.DocFeatureId).AsNoTracking().AsQueryable();

            var trxTypes = _context.MSTransactionTypes.Where(x=>x.InActive != true).OrderBy(o=>o.TransactionType).AsQueryable();

            var param = datas.Where(x => x.TransactionType=="" && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_INVOICE).FirstOrDefault();

            foreach(var trxModule in this.GetDocTrxModules())
            {
                if (trxModule.Key == TRX_MODULE.TRX_CONTAINER_RENT)
                {
                    foreach (var trx in trxTypes)
                    {
                        List<ResponseItem> listFeature = new List<ResponseItem>();

                        //RENTAL REQUEST
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_RENTAL_REQUEST).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_RENTAL_REQUEST,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_RENTAL_REQUEST)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key && 
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_RENTAL_REQUEST && 
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //RENTAL DELIVERY
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_RENTAL_DELIVERY).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_RENTAL_DELIVERY,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_RENTAL_DELIVERY)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_RENTAL_DELIVERY &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //RENTAL CLOSING
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_RENTAL_CLOSING).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_RENTAL_CLOSING,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_RENTAL_CLOSING)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_RENTAL_CLOSING &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        headers.Add(new ResponseHeader { TrxModule = trxModule.Key, Group = trx.TransactionType, Features = listFeature });
                    }
                } else if (trxModule.Key == TRX_MODULE.TRX_SALES)
                {
                    foreach (var trx in trxTypes)
                    {
                        List<ResponseItem> listFeature = new List<ResponseItem>();

                        //QUOT
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_QUOT).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_QUOT,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_SALES_QUOT)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_QUOT &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //ORDER
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_ORDER).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_ORDER,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_SALES_ORDER)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_ORDER &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //NEGO SHEET
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        headers.Add(new ResponseHeader { TrxModule = trxModule.Key, Group = trx.TransactionType, Features = listFeature });
                    }
                }
                else if (trxModule.Key == TRX_MODULE.TRX_DEPOSIT)
                {
                    foreach (var trx in trxTypes)
                    {
                        List<ResponseItem> listFeature = new List<ResponseItem>();

                        // DEMURRAGE
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_DEPOSIT_DEMURRAGE).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_DEPOSIT_DEMURRAGE,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_DEPOSIT_DEMURRAGE)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_DEPOSIT_DEMURRAGE &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        // CONTAINER GUARANTEE
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_CONTAINER_GUARANTEE).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_CONTAINER_GUARANTEE,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_CONTAINER_GUARANTEE)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_CONTAINER_GUARANTEE &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        // DETENTION
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_DETENTION).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_DETENTION,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_DETENTION)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_DETENTION &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        headers.Add(new ResponseHeader { TrxModule = trxModule.Key, Group = trx.TransactionType, Features = listFeature });
                    }
                }
                else
                {
                    foreach (var trx in trxTypes)
                    {
                        List<ResponseItem> listFeature = new List<ResponseItem>();

                        //INVOICE
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_INVOICE).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_INVOICE,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_INVOICE)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_INVOICE &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //DN
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_DEBITNOTE).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_DEBITNOTE,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_DEBITNOTE)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_DEBITNOTE &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //CN
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_CREDITNOTE).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_CREDITNOTE,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_CREDITNOTE)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_CREDITNOTE &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //WO
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_WRITEOFF).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_WRITEOFF,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_WRITEOFF)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_WRITEOFF &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //WHT
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_WHT).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_WHT,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_WHT)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_WHT &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //SOA DEBIT
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SOA_DEBIT).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_SOA_DEBIT,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_SOA_DEBIT)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SOA_DEBIT &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        //SOA CREDIT
                        param = datas.Where(x => x.TrxModule == trxModule.Key && x.TransactionType == trx.TransactionType && x.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SOA_CREDIT).FirstOrDefault();

                        listFeature.Add(new ResponseItem
                        {
                            TrxModule = trxModule.Key,
                            TransactionType = trx.TransactionType,
                            DocFeatureId = DOCNO_FEATURE.TRXTYPE_SOA_CREDIT,
                            DocNumberId = (param != null ? param.DocNumberId : 0),
                            DocNo = (param != null ? param.DocNo : ""),
                            Description = (param != null ? param.Description : DOCNO_FEATURE.CaptionTrxType(DOCNO_FEATURE.TRXTYPE_SOA_CREDIT)),
                            ApprovalSetups = (from ap in _context.FNDocNumberSetupApprovals
                                              where ap.TrxModule == trxModule.Key &&
                                              ap.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SOA_CREDIT &&
                                              ap.TransactionType == trx.TransactionType
                                              orderby ap.ModeStatus ascending
                                              select new ResponseItemApproval
                                              {
                                                  DocNumberSetupApprovalId = ap.DocNumberSetupApprovalId,
                                                  TransactionType = ap.TransactionType,
                                                  DocFeatureId = ap.DocFeatureId,
                                                  TrxModule = ap.TrxModule,
                                                  ModeStatus = ap.ModeStatus,
                                                  ApprovalCode = ap.ApprovalCode,
                                                  ModifiedBy = ap.ModifiedBy,
                                                  ModifiedDate = (DateTime)ap.ModifiedDate
                                              }).Distinct().ToList()
                        });

                        headers.Add(new ResponseHeader { TrxModule = trxModule.Key, Group = trx.TransactionType, Features = listFeature });
                    }
                }
            }


            return headers;
        }

    }
}
