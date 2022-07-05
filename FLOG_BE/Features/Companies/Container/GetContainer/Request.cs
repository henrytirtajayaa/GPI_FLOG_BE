using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Container.GetContainer
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> ContainerCode { get; set; }
        public List<string> ContainerName { get; set; }
        public List<int?> ContainerSizeMin { get; set; }
        public List<int?> ContainerSizeMax { get; set; }
        public List<string> ContainerType { get; set; }
        public List<int?> ContainerTeusMin { get; set; }
        public List<int?> ContainerTeusMax { get; set; }
        public bool? Inactive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }

    }
}
