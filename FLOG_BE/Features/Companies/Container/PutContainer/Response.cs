using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.Container.PutContainer
{
    public class Response
    {
        public Guid Containerid { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public bool IsReefer { get; set; }
        public int ContainerTeus { get; set; }
        public bool Inactive { get; set; }
    }
}
