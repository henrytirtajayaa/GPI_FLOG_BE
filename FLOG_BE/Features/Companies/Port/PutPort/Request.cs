using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Port.PutPort
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPortUpdate Body { get; set; }
    }

    public class RequestPortUpdate
    { 
        public string PortId { get; set; }
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortType { get; set; }
        public string CityCode { get; set; }
        public bool InActive { get; set; }
    }
}
