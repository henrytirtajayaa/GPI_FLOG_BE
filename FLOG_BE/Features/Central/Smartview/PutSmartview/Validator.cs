﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.Smartview.PutrSmartview
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
            if (request.Body.SmartViewId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.SmartViewId)} cannot be null");

           

            return ValidationResult.Ok();
        }
    }
}
