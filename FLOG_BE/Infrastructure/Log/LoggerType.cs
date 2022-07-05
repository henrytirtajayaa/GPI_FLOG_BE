using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Log
{
    public enum LoggerType
    {
        [Description("Error")]
        Error = 0,

        [Description("Info")]
        Info = 1,
    }
}
