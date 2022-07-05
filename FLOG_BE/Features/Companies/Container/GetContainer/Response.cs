using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.Container.GetContainer
{
    public class Response
    {
        public List<ResponseItem> Containers { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public string RefContainerTypeDesc { get; set; }
        public int ContainerTeus { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
