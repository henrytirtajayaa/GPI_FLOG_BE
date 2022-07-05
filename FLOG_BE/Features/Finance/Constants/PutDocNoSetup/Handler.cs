using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG.Core.DocumentNo;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.Constants.PutDocNoSetup
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
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            List<ResponseItem> docNumberSetups = new List<ResponseItem>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                bool valid = true;

                #region FEATURE
                foreach (var req in request.DocNumberSetups)
                {
                    if(req.DocNumberId > 0)
                    {
                        //UPDATE
                        var spec = _context.FNDocNumberSetups.Find(req.DocNumberId);
                        spec.DocNo = req.DocNo;
                        spec.Description = req.Description;

                        _context.FNDocNumberSetups.Update(spec);

                        var setups = await InsertApprovalSetups(spec, req.ApprovalSetups, request);

                        List<ResponseItemApproval> approvals = new List<ResponseItemApproval>();
                        foreach(var set in setups)
                        {
                            var apr = new ResponseItemApproval
                            {
                              ApprovalCode = set.ApprovalCode,
                              DocFeatureId = set.DocFeatureId,
                              TransactionType = set.TransactionType,
                              ModeStatus = set.ModeStatus,
                              TrxModule = set.TrxModule,
                              ModifiedBy = set.ModifiedBy,
                              ModifiedDate = (DateTime) set.ModifiedDate
                            };
                            approvals.Add(apr);
                        }
                        
                        docNumberSetups.Add(new ResponseItem { DocNumberId = spec.DocNumberId, TrxModule = spec.TrxModule, Description = spec.Description, DocFeatureId = spec.DocFeatureId, DocNo = spec.DocNo, TransactionType = spec.TransactionType, ApprovalSetups = approvals });
                    }
                    else
                    {
                        //REMOVE EXISTING 
                        _context.FNDocNumberSetups
               .Where(x => x.TrxModule == req.TrxModule && x.TransactionType == req.TransactionType && x.DocFeatureId == req.DocFeatureId).ToList().ForEach(p => _context.Remove(p));

                        //CREATE NEW
                        Entities.FNDocNumberSetup spec = new Entities.FNDocNumberSetup();
                        spec.TrxModule = req.TrxModule;
                        spec.Description = req.Description;
                        spec.TransactionType = req.TransactionType;
                        spec.DocFeatureId = req.DocFeatureId;
                        spec.DocNo = req.DocNo;

                        _context.FNDocNumberSetups.Add(spec);

                        var setups = await InsertApprovalSetups(spec, req.ApprovalSetups, request);

                        await _context.SaveChangesAsync();

                        List<ResponseItemApproval> approvals = new List<ResponseItemApproval>();
                        foreach (var set in setups)
                        {
                            var apr = new ResponseItemApproval
                            {
                                ApprovalCode = set.ApprovalCode,
                                DocFeatureId = set.DocFeatureId,
                                TransactionType = set.TransactionType,
                                ModeStatus = set.ModeStatus,
                                TrxModule = set.TrxModule,
                                ModifiedBy = set.ModifiedBy,
                                ModifiedDate = (DateTime)set.ModifiedDate
                            };
                            approvals.Add(apr);
                        }

                        if (spec.DocNumberId > 0)
                        {
                            docNumberSetups.Add(new ResponseItem { DocNumberId = spec.DocNumberId, TrxModule = spec.TrxModule, Description = spec.Description, DocFeatureId = spec.DocFeatureId, DocNo = spec.DocNo, TransactionType = "", ApprovalSetups = approvals });
                        }
                    }
                }
                #endregion FEATURE

                #region NON FEATURE - BY TRANSACTION TYPE
                List<ResponseItem> trxTypeSetups = new List<ResponseItem>();

                if(request.TrxTypeSetups != null)
                {
                    foreach (var req in request.TrxTypeSetups)
                    {
                        if (req.DocNumberId > 0)
                        {
                            var spec = _context.FNDocNumberSetups.Find(req.DocNumberId);

                            if (spec != null)
                            {
                                if (req.IsDeleted)
                                {
                                    _context.FNDocNumberSetups.Attach(spec);
                                    _context.FNDocNumberSetups.Remove(spec);
                                }
                                else
                                {
                                    //UPDATE
                                    spec.DocNo = req.DocNo;
                                    spec.Description = req.Description;
                                    spec.TrxModule = req.TrxModule;
                                    spec.TransactionType = req.TransactionType;

                                    _context.FNDocNumberSetups.Update(spec);

                                    var setups = await InsertApprovalSetups(spec, req.ApprovalSetups, request);

                                    List<ResponseItemApproval> approvals = new List<ResponseItemApproval>();
                                    foreach (var set in setups)
                                    {
                                        var apr = new ResponseItemApproval
                                        {
                                            ApprovalCode = set.ApprovalCode,
                                            DocFeatureId = set.DocFeatureId,
                                            TransactionType = set.TransactionType,
                                            ModeStatus = set.ModeStatus,
                                            TrxModule = set.TrxModule,
                                            ModifiedBy = set.ModifiedBy,
                                            ModifiedDate = (DateTime)set.ModifiedDate
                                        };
                                        approvals.Add(apr);
                                    }

                                    trxTypeSetups.Add(new ResponseItem { DocNumberId = spec.DocNumberId, TrxModule = spec.TrxModule, Description = spec.Description, DocFeatureId = spec.DocFeatureId, DocNo = spec.DocNo, TransactionType = spec.TransactionType, ApprovalSetups = approvals });
                                }
                            }
                        }
                        else
                        {
                            //REMOVE EXISTING 
                            _context.FNDocNumberSetups
                   .Where(x => x.TrxModule == req.TrxModule && x.TransactionType == req.TransactionType && x.DocFeatureId == req.DocFeatureId).ToList().ForEach(p => _context.Remove(p));

                            //CREATE NEW
                            Entities.FNDocNumberSetup spec = new Entities.FNDocNumberSetup();
                            spec.TrxModule = req.TrxModule;
                            spec.TransactionType = req.TransactionType;
                            spec.DocFeatureId = req.DocFeatureId;
                            spec.Description = DOCNO_FEATURE.CaptionTrxType(spec.DocFeatureId);
                            spec.DocNo = req.DocNo;

                            _context.FNDocNumberSetups.Add(spec);

                            var setups = await InsertApprovalSetups(spec, req.ApprovalSetups, request);

                            await _context.SaveChangesAsync();

                            List<ResponseItemApproval> approvals = new List<ResponseItemApproval>();
                            foreach (var set in setups)
                            {
                                var apr = new ResponseItemApproval
                                {
                                    ApprovalCode = set.ApprovalCode,
                                    DocFeatureId = set.DocFeatureId,
                                    TransactionType = set.TransactionType,
                                    ModeStatus = set.ModeStatus,
                                    TrxModule = set.TrxModule,
                                    ModifiedBy = set.ModifiedBy,
                                    ModifiedDate = (DateTime)set.ModifiedDate
                                };
                                approvals.Add(apr);
                            }

                            if (spec.DocNumberId > 0)
                            {
                                trxTypeSetups.Add(new ResponseItem { DocNumberId = spec.DocNumberId, TrxModule = spec.TrxModule, Description = spec.Description, DocFeatureId = spec.DocFeatureId, DocNo = spec.DocNo, TransactionType = spec.TransactionType, ApprovalSetups = approvals });
                            }
                        }
                    }
                }                

                #endregion NON FEATURE - BY TRANSACTION TYPE

                if (valid)
                {
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ApiResult<Response>.Ok(new Response { Success = true, Message = "Success", DocNumberSetups = docNumberSetups, TrxTypeSetups = trxTypeSetups });
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Document Number Setup can not be updated !");
                }                
            }            
        }

        private async Task<List<Entities.FNDocNumberSetupApproval>> InsertApprovalSetups(Entities.FNDocNumberSetup spec, List<RequestItemApproval> approvalSetups, Request req)
        {
            List<Entities.FNDocNumberSetupApproval> result = new List<Entities.FNDocNumberSetupApproval>();

            if (spec != null && approvalSetups != null)
            {
                //REMOVE EXISTING 
                _context.FNDocNumberSetupApprovals
       .Where(x => x.TrxModule == spec.TrxModule && x.TransactionType == spec.TransactionType && x.DocFeatureId == spec.DocFeatureId).ToList().ForEach(p => _context.Remove(p));

                foreach(var appr in approvalSetups)
                {
                    var aps = new Entities.FNDocNumberSetupApproval
                    {
                        DocFeatureId = spec.DocFeatureId,
                        TransactionType = spec.TransactionType,
                        TrxModule = spec.TrxModule,
                        ModeStatus = appr.ModeStatus,
                        ApprovalCode = appr.ApprovalCode,
                        ModifiedBy = req.Initiator.UserId,
                        ModifiedDate = DateTime.Now
                    };

                    result.Add(aps);
                }

                if(result.Count > 0)
                    await _context.FNDocNumberSetupApprovals.AddRangeAsync(result);
            }

            return result;
        }
    }
}
