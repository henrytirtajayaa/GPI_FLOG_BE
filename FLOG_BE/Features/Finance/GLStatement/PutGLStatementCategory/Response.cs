using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.GLStatement.PutGLStatementCategory
{
    public class Response
    {
        public int CategoryId { get; set; }
        public string Message { get; set; }
        
    }
}
