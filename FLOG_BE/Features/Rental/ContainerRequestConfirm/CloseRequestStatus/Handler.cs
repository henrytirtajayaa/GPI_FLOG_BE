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


namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.CloseRequestStatus
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
            var query = (from s in _context.ContainerConfirmQuantities
                         where s.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId
                         && s.QuantityRemaining > 0
                         select new Entities.ContainerRentalRequestDetail
                         {
                             ContainerRentalRequestDetailId = s.ContainerRentalRequestDetailId,
                             ContainerRentalRequestHeaderId = s.ContainerRentalRequestHeaderId,
                             Quantity = s.QuantityRemaining
                         }).AsEnumerable().ToList().AsQueryable();

            /*var query = (from x in _context.ContainerRentalRequestHeaders select new Entities.ContainerRentalRequestHeader
            {
                ContainerRentalRequestHeaderId = x.ContainerRentalRequestHeaderId,
                ContainerRentalRequestDetails = (from s in _context.ContainerConfirmQuantities
                                                 where s.ContainerRentalRequestHeaderId == x.ContainerRentalRequestHeaderId
                                                 && s.QuantityRemaining > 0
                                                 select new Entities.ContainerRentalRequestDetail
                                                 {
                                                     ContainerRentalRequestDetailId = s.ContainerRentalRequestDetailId,
                                                     ContainerRentalRequestHeaderId = s.ContainerRentalRequestHeaderId,
                                                     Quantity = s.QuantityRemaining
                                                 }).ToList(),
            }).AsEnumerable().ToList().AsQueryable();*/

            Console.WriteLine("============================QUERY=======================" + query.Count() + "======================END QUERY===================");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var RentalRequest = await _context.ContainerRentalRequestHeaders.FirstOrDefaultAsync(x => x.ContainerRentalRequestHeaderId == request.Body.ContainerRentalRequestHeaderId);

                    string docNo = "";
                    if (RentalRequest != null)
                    {
                        docNo = RentalRequest.DocumentNo;

                        if (query.Count() == 0)
                        {
                            if (request.Body.Status == DOCSTATUS.COMPLETE)
                            {
                                if (RentalRequest.Status == DOCSTATUS.SUBMIT)
                                {
                                    RentalRequest.Status = DOCSTATUS.COMPLETE;
                                    RentalRequest.ModifiedBy = request.Initiator.UserId;
                                    RentalRequest.ModifiedDate = DateTime.Now;

                                    _context.ContainerRentalRequestHeaders.Update(RentalRequest);

                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Record can not be deleted!");
                                }
                            }
                        }
                        else
                        {
                            RentalRequest.Status = DOCSTATUS.SUBMIT;
                            RentalRequest.ModifiedBy = request.Initiator.UserId;
                            RentalRequest.ModifiedDate = DateTime.Now;

                            _context.ContainerRentalRequestHeaders.Update(RentalRequest);

                            await _context.SaveChangesAsync();

                            transaction.Commit();
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Document not available");
                    }

                    var response = new Response()
                    {
                        ContainerRentalRequestHeaderId = request.Body.ContainerRentalRequestHeaderId,
                        Status = request.Body.Status
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.InternalServerError("Cannot put status!");
                }
            }
        }
    }
}
