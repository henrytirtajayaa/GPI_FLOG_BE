using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Log.Interface
{
    public interface ILogWriter
    {
        void Write(LoggerType type, string logMessage);
    }
}
