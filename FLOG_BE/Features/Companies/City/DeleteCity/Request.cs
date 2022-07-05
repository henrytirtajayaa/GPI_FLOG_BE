using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.City.DeleteCity
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCityDelete Body { get; set; }
    }

    public class RequestCityDelete
    {
        public String CityId { get; set; }
    }
}
