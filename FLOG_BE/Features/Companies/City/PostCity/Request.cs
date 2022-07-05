using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.City.PostCity
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCityBody Body { get; set; }
    }

    public class RequestCityBody
    {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public string CountryId { get; set; }
   
    }
}
