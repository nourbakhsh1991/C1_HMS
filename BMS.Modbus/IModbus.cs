using BMS.Domain.Models.Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Modbus
{
    public interface IModbus
    {
        IPAddress ServerAddress { get; set; }
        int ServerPort { get; set; }
        byte SlaveId { get; set; }
        bool IsStarted { get; }
        long UpdateTime { get; set; }

        void ReadData(ModbusData data);
        void WriteData(ModbusData data);
    }
}
