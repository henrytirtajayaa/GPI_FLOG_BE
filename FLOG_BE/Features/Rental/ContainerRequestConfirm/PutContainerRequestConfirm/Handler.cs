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
using FLOG_BE.Model.Companies.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PutContainerRequestConfirm
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                ContainerRequestConfirmHeaderId = request.Body.ContainerRequestConfirmHeaderId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var DoConfirmHeader = await _context.ContainerRequestConfirmHeaders.FirstOrDefaultAsync(x => x.ContainerRequestConfirmHeaderId == request.Body.ContainerRequestConfirmHeaderId);

                    if (DoConfirmHeader != null)
                    {
                        DoConfirmHeader.DocumentDate = request.Body.DocumentDate;
                        DoConfirmHeader.IssueDate = request.Body.IssueDate;
                        DoConfirmHeader.ExpiredDate = request.Body.ExpiredDate;
                        DoConfirmHeader.ModifiedBy = request.Initiator.UserId;
                        DoConfirmHeader.ModifiedDate = DateTime.Now;

                        var DoConfirmDetail = await insertContainerRequestConfirmDetail(_context, request.Body);

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            ContainerRequestConfirmHeaderId = DoConfirmHeader.ContainerRequestConfirmHeaderId
                        });
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Delivery Order Confirmation Update NOT Successful!");
                    }
                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task<List<Entities.ContainerRequestConfirmDetail>> insertContainerRequestConfirmDetail(CompanyContext ctx, RequestContainerRequestConfirm body)
        {
            List<Entities.ContainerRequestConfirmDetail> result = new List<ContainerRequestConfirmDetail>();

            if (body.ContainerRequestConfirmDetails != null)
            {
                //REMOVE EXISTING
                ctx.ContainerRequestConfirmDetails
               .Where(x => x.ContainerRequestConfirmHeaderId == body.ContainerRequestConfirmHeaderId).ToList().ForEach(p => ctx.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.ContainerRequestConfirmDetails)
                {
                    var DoConfirmDetail = new Entities.ContainerRequestConfirmDetail()
                    {
                        ContainerRequestConfirmDetailId = Guid.NewGuid(),
                        ContainerRequestConfirmHeaderId = body.ContainerRequestConfirmHeaderId,
                        ContainerRentalRequestDetailId = item.ContainerRentalRequestDetailId,
                        Remarks = item.Remarks,
                        Grade = item.Grade,
                        QuantityToConfirm = item.QuantityToConfirm,
                        QuantityBalance = item.QuantityBalance
                    };

                    result.Add(DoConfirmDetail);
                }

                if (result.Any())
                {
                    await _context.ContainerRequestConfirmDetails.AddRangeAsync(result);
                }
            }

            return result;
        }
    }
}
