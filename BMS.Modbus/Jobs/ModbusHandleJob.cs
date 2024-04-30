using BMS.Domain.Models.Modbus;
//using BMS.Modbus.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Win32;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Modbus.Jobs
{
    public class ModbusHandleJob : IJob
    {


        //public static List<ModbusTCP> connections = new List<ModbusTCP>();
        //public static Dictionary<string, List<ModbusData>> connectionData = new Dictionary<string, List<ModbusData>>();

        //private readonly IHubContext<ModbusDataHub> _dataHubContext;

        //public ModbusHandleJob(IHubContext<ModbusDataHub> dataHubContext)
        //{
        //    _dataHubContext = dataHubContext;
        //}

        public async Task Execute(IJobExecutionContext context)
        {
            return;
            //try
            //{
            //    foreach (var conn in connections)
            //    {
            //        if (!conn.IsStarted) conn.Start();
            //        if(!conn.IsStarted) { continue; }
            //        //System.Diagnostics.Debug.WriteLine($"______________________________________");
            //        //System.Diagnostics.Debug.WriteLine($"MODBUS: {conn.Name}");
            //        var datas = conn.ModbusEntityId != null && connectionData.ContainsKey(conn.ModbusEntityId) ?
            //            connectionData[conn.ModbusEntityId] : new List<ModbusData>();
            //        foreach (var register in datas)
            //        {
            //            conn.ReadData(register);
            //            for (int i = 0; i < register.Count; i++)
            //            {
            //                //if (register.Type == ModbusDataType.Coil || register.Type == ModbusDataType.DiscreteInput)
            //                //    System.Diagnostics.Debug.WriteLine($"\tAddress {i + 1 + register.StartAddress}: {register.BitData[i]}");
            //                //else
            //                //    System.Diagnostics.Debug.WriteLine($"\tAddress {i + 1 + register.StartAddress}: {register.WordData[i]}");
            //            }
            //            await _dataHubContext.Clients.All.SendAsync("NewData", register);
            //        }

            //    }
                //await _dataHubContext.Clients.All.SendAsync("NewData", "New Data From Modbus");
                //if (tcp == null)
                //{
                //    tcp = new ModbusTCP("127.0.0.1", 50200, 1, 100);
                //    tcp.Start();
                //}
                //lastUpdate = DateTime.UtcNow.Ticks;
                //Console.WriteLine(count++);



                //System.Diagnostics.Debug.WriteLine($"Time: {lastUpdate}");

                //return Task.CompletedTask;
            //}
            //catch(Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}
        }
    }
}
