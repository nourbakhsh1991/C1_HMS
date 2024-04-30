using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Modbus
{
    public class ModbusData
    {
        public ModbusDataType Type { get; set; }
        public ushort StartAddress { get; set; }
        public ushort Count { get; set; }
        public string ModbusName { get; set; }
        public string EntityId { get; set; }
        public List<bool?> BitData { get; set; } = new List<bool?>();
        public List<ushort?> WordData { get; set; } = new List<ushort?>();

    }
}
