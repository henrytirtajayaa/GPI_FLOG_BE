using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.GLClosing.PostGLClosingMonth
{
    public class Response
    {
        public long GLClosingId { get; set; }
        public string Message { get; set; }    
    }
}
