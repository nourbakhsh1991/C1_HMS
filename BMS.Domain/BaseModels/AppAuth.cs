using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.BaseModels
{
    public class AppAuth
    {
        public string Secret { get; set; }
        public string Site { get; set; }
        public string Audiance { get; set; }
        public string ExpireTime { get; set; }
    }
}
