using System;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Authentication.GetSession
{
    //public class Request : IRequest<Response>
    //{

    //        public UserLogin Initiator { get; set; }
    //        public RequestFilter Filter { get; set; }
    //        public Int32 Offset { get; set; }
    //        public Int32 Limit { get; set; }
    //        public List<string> Sort { get; set; }

    //}
    //public class RequestFilter
    //{
    //     public Guid Id { get; set; }
    //    public Guid PersonId { get; set; }
    //    public Guid CompanySecurityId { get; set; }
    //    public Guid CompanyId { get; set; }
    //    public bool Status { get; set; }
    //}

    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid Id { get; set; }
        public string PersonId { get; set; }
        public string CompanySecurityId { get; set; }
        public string CompanyId { get; set; }
        public bool Status { get; set; }
        // public List<string> Sort { get; set; }
    }

   
}
