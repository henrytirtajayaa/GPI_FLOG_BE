﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutDeleteContainerRentalRequest
{
    public class Validator : AsyncRequestValidator<Request>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Validator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<ValidationResult> Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Body.ContainerRentalRequestHeaderId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.ContainerRentalRequestHeaderId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
