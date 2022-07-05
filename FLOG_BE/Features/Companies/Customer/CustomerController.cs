using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Customer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Customer")]
        public async Task<IActionResult> GetCustomer(
            [CommaSeparated] Guid CustomerId
           , [CommaSeparated] List<string> CustomerCode
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> AddressCode
           , [CommaSeparated] List<string> TaxRegistrationNo
           , [CommaSeparated] List<string> CustomerGroupCode
           , [CommaSeparated] List<string> CustomerTaxName
           , [CommaSeparated] List<string> PaymentTermCode
           , [CommaSeparated] bool? HasCreditLimit
           , [CommaSeparated] List<decimal?> CreditLimit
           , [CommaSeparated] List<string> ReceivableAccountNo
           , [CommaSeparated] List<string> BillToAddressCode
           , [CommaSeparated] List<string> ShipToAddressCode
           , [CommaSeparated] List<string> TaxAddressCode
           , [CommaSeparated] List<string> AccruedReceivableAccountNo
           , [CommaSeparated] bool? Inactive
           , [CommaSeparated] List<int?> Status
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetCustomer.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCustomer.RequestFilter
                {
                    CustomerId = CustomerId,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    AddressCode = AddressCode,
                    TaxRegistrationNo = TaxRegistrationNo,
                    CustomerGroupCode = CustomerGroupCode,
                    CustomerTaxName = CustomerTaxName,
                    PaymentTermCode = PaymentTermCode,
                    HasCreditLimit = HasCreditLimit,
                    CreditLimit = CreditLimit,
                    BillToAddressCode = BillToAddressCode,
                    ShipToAddressCode = ShipToAddressCode,
                    TaxAddressCode = TaxAddressCode,
                    ReceivableAccountNo = ReceivableAccountNo,
                    AccruedReceivableAccountNo = AccruedReceivableAccountNo,
                    Inactive = Inactive,
                    Status = Status,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("Customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] PostCustomer.RequestPostBody body)
        {
            var req = new PostCustomer.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Customer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] PutCustomer.RequestPutBody body)
        {
            var req = new PutCustomer.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Customer")]
        public async Task<IActionResult> DeleteCustomer(DeleteCustomer.RequestDeleteBody body)
        {
            var req = new DeleteCustomer.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
