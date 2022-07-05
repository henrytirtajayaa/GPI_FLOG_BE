using System;
namespace Infrastructure.Mediator
{
    public interface IInitiator
    {
        UserLogin Initiator { get; set; }
    }

    public class UserLogin
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string SecurityId { get; set; }
    }
}
