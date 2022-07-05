using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.City.PutCity
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCityUpdate Body { get; set; }
    }

    public class RequestCityUpdate
    {
        public string CityId { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public string CountryId { get; set; }
        public bool inActive { get; set; }
    }
}
