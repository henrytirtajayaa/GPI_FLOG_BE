using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.User.PutUser
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutBodyUser Body { get; set; }
    }

    public class RequestPutBodyUser
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }

        public bool _UserPasswordIsChanged = false;
        private string _UserPassword { get; set; }
        public string UserPassword
        {
            get
            {
                return _UserPassword;
            }
            set
            {
                this._UserPassword = value;
                this._UserPasswordIsChanged = true;
            }
        }

        public string EmailAddress { get; set; }
        public string UserGroupId { get; set; }
        public bool InActive { get; set; }
    }
}
