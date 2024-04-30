using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.BaseModels
{
    public class MBaseDBModel
    {
        public string Id { get; set; }

        public Dictionary<string, object> Metadata { get; set; }
    }
}
