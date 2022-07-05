using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;


namespace FLOG.Core.Smartview
{
    public interface ISmartviewQuery
    {
        DataTable DefaultSmartview(String tableName, DbTransaction trans = null);
        DataTable ResultSmartview(String tableName, String Filter, DbTransaction trans = null);
    }
}
