using BMS.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models
{
    public class StartupLog : BaseDBModel
    {
        public DateTime dateTime { get; set; }

    }
}
