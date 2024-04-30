using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.ClientModels
{
    public class QueryParamModel
    {
        public string? filter { get; set; }
        public string? sortOrder { get; set; }
        public string? sortField { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

        public QueryParamModel()
        {
            filter = null;
            sortOrder = "asc";
            sortField = null;
            pageNumber = 0;
            pageSize = int.MaxValue;
        }
    }
}
