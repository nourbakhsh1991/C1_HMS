using BMS.Domain.Models.Modbus;
using BMS.Services.Modbus.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Modbus.Jobs
{
    public class ModbusStarterJob : IJob
    {

        private readonly IModbusService _modbusService;
        private readonly IModbusRegisterService _modbusRegisterService;

        public ModbusStarterJob(IModbusService modbusService, IModbusRegisterService modbusRegisterService)
        {
            _modbusService = modbusService;
            _modbusRegisterService = modbusRegisterService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //var all = _modbusService.GetAll();
            //foreach (var item in all)
            //{
            //    if (!ModbusHandleJob.connections.Any(a =>
            //                a.ModbusEntityId == item.Id))
            //    {

            //        ModbusHandleJob.connections.Add(new ModbusTCP(item.ServerAddress, item.ServerPort, 1, 100)
            //        {
            //            Name = item.Name,
            //            ModbusEntityId = item.Id
            //        });
            //    }
            //    var registers = _modbusRegisterService.GetAsQueryable().Where(a => a.ModbusId == item.Id).ToList();
            //    if (ModbusHandleJob.connectionData.ContainsKey(item.Id) &&
            //        ModbusHandleJob.connectionData[item.Id].Count != registers.Count)
            //    {
            //        ModbusHandleJob.connectionData[item.Id] = registers.
            //            Select(a =>
            //            {
            //                var itm = new Domain.Models.Modbus.ModbusData()
            //                {
            //                    StartAddress = a.StartAddress,
            //                    Count = a.Count,
            //                    Type = a.Type == 1 ? ModbusDataType.Coil :
            //                       a.Type == 2 ? ModbusDataType.DiscreteInput :
            //                       a.Type == 3 ? ModbusDataType.InputRegister :
            //                       a.Type == 4 ? ModbusDataType.HoldingRegister : ModbusDataType.Coil,
            //                    BitData = new List<bool?>(),
            //                    WordData = new List<ushort?>(),
            //                    EntityId= a.Id,
            //                    ModbusName = item.Name
            //                };
            //                for (int i = 0; i < itm.Count; i++)
            //                {
            //                    itm.BitData.Add(null);
            //                    itm.WordData.Add(null);
            //                }
            //                return itm;
            //            }).ToList();
            //    }else if (!ModbusHandleJob.connectionData.ContainsKey(item.Id))
            //    {
            //        ModbusHandleJob.connectionData.Add(item.Id, registers.
            //            Select(a =>
            //            {
            //                var itm = new Domain.Models.Modbus.ModbusData()
            //                {
            //                    StartAddress = a.StartAddress,
            //                    Count = a.Count,
            //                    Type = a.Type == 1 ? ModbusDataType.Coil :
            //                       a.Type == 2 ? ModbusDataType.DiscreteInput :
            //                       a.Type == 3 ? ModbusDataType.InputRegister :
            //                       a.Type == 4 ? ModbusDataType.HoldingRegister : ModbusDataType.Coil,
            //                    BitData = new List<bool?>(),
            //                    WordData = new List<ushort?>(),
            //                    EntityId = a.Id,
            //                    ModbusName = item.Name
            //                };
            //                for (int i = 0; i < itm.Count; i++)
            //                {
            //                    itm.BitData.Add(null);
            //                    itm.WordData.Add(null);
            //                }
            //                return itm;
            //            }).ToList());
            //    }

            //}

            return Task.CompletedTask;
        }
    }
}
