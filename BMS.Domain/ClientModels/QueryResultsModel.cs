using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.ClientModels
{
    public class QueryResultsModel
    {
        public object Result { get; set; }
        public int Code { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
    }
}
