using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ApprovalSetup.PostApprovalSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestApprovalSetupHeader Body { get; set; }
       
    }

    public class RequestApprovalSetupHeader
    {
      
        public Guid ApprovalSetupHeaderId { get; set; }
        public string ApprovalCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ApprovalSetupDetails> ApprovalSetupDetails{ get; set; }

    }
    public class ApprovalSetupDetails
    {

     
        public Guid ApprovalSetupDetailId { get; set; }
        public Guid ApprovalSetupHeaderId { get; set; }
        public string Description { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public int Level { get; set; }
        public bool HasLimit { get; set; }
        public decimal ApprovalLimit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }


    }
}
