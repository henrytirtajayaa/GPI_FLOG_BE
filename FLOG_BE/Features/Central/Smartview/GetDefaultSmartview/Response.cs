using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace FLOG_BE.Features.Central.Smartview.GetDefaultSmartview
{
    public class Response
    {
        public DataTable Smartviews { get; set; }
        public List<ResponseColumn> Columns { get; set; }

    }

    public class ResponseColumn
    {
        public string Column { get; set; }
    }
}
