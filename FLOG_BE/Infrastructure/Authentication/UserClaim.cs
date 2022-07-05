using System;
using System.ComponentModel;

namespace Infrastructure.Authentication
{
    public enum UserClaim
    {
        [Description("User Request Login")]
        PreLogin,
        [Description("Super Admin")]
        SuperAdmin,
        [Description("User Company Login")]
        Company,
    }
}
