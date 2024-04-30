using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Modbus
{
    public enum ModbusDataType
    {
        Coil,
        DiscreteInput,
        InputRegister,
        HoldingRegister
    }
}
