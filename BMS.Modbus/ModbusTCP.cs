using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BMS.Domain.Models.Modbus;
using MongoDB.Bson;
using NModbus;
using NModbus.Extensions.Enron;
using NModbus.Utility;

namespace BMS.Modbus
{
    public class ModbusTCP : IModbus
    {
        public IPAddress ServerAddress { get; set; }
        public string Name { get; set; }
        public int ServerPort { get; set; }
        public byte SlaveId { get; set; }
        public string? ModbusEntityId { get; set; }
        private bool _isStarted { get; set; }
        public bool IsStarted { get { return _isStarted; } }
        private TcpClient Client { get; set; }
        private IModbusMaster? Master { get; set; }

        private long _updateTime;
        public long UpdateTime
        {
            get { return _updateTime; }
            set
            {
                if (value < 0)
                    _updateTime = 100;
                else
                    _updateTime = value;
            }
        }

        private IPEndPoint Address { get; set; }

        public ModbusTCP(IPAddress address, int port, byte slaveId = 0, long update = 100)
        {
            ServerAddress = address;
            SlaveId = slaveId;
            Address = new IPEndPoint(address, port);
            ServerPort = port;
            Client = null;
            _updateTime = update;
        }

        public ModbusTCP(string address, int port, byte slaveId = 0, long update = 100)
        {
            ServerAddress = IPAddress.Parse(address);
            SlaveId = slaveId;
            Address = new IPEndPoint(ServerAddress, port);
            ServerPort = port;
            Client = null;
            _updateTime = update;
        }

        ~ModbusTCP()
        {
            if (Client != null)
            {
                Client.Close();
                Client.Dispose();
            }
        }

        public void Start()
        {
            try
            {
                Disconnect();
                Client = new TcpClient();
                Client.Connect(Address);
                _isStarted = true;
                var factory = new ModbusFactory();
                Master = factory.CreateMaster(Client);
            }
            catch(Exception ex)
            {

            }
        }

        public void Disconnect()
        {
            if (!_isStarted) { return; }
            if (Client != null)
            {
                Client.Close();
                Client.Dispose();
                _isStarted = false;
            }
            if (Master != null)
                Master.Dispose();
        }

        private List<bool> ReadDiscreteInputs(ushort start, ushort numberOfInput)
        {
            if (Master == null) return null;
            bool[] inputs = Master.ReadInputs(SlaveId, start, numberOfInput);
            return new List<bool>(inputs);
        }
        private List<bool> ReadCoils(ushort start, ushort numberOfInput)
        {
            if (Master == null) return null;
            bool[] inputs = Master.ReadCoils(SlaveId, start, numberOfInput);
            return new List<bool>(inputs);
        }

        private List<ushort> ReadInputRegisters(ushort start, ushort numberOfInput)
        {
            if (Master == null) return null;
            ushort[] inputs = Master.ReadInputRegisters(SlaveId, start, numberOfInput);
            return new List<ushort>(inputs);
        }

        private List<ushort> ReadHoldingRegisters(ushort start, ushort numberOfInput)
        {
            if (Master == null) return null;
            ushort[] inputs = Master.ReadHoldingRegisters(SlaveId, start, numberOfInput);
            return new List<ushort>(inputs);
        }

        private void WriteCoils(ushort start, List<bool> data)
        {
            if (Master == null) return;
            Master.WriteMultipleCoils(SlaveId, start, data.ToArray());
        }

        private void WriteHoldingRegisters(ushort start, List<ushort> data)
        {
            if (Master == null) return;
            Master.WriteMultipleRegisters(SlaveId, start, data.ToArray());
        }

        public void ReadData(ModbusData data)
        {
            if (data == null) return;
            if (Master == null) return;
            switch (data.Type)
            {
                case ModbusDataType.Coil:
                    var coils = ReadCoils(data.StartAddress, data.Count);
                    data.BitData = coils.Select(a => (bool?)a).ToList();
                    break;
                case ModbusDataType.DiscreteInput:
                    var discerteInputs = ReadDiscreteInputs(data.StartAddress, data.Count);
                    data.BitData = discerteInputs.Select(a => (bool?)a).ToList();
                    break;
                case ModbusDataType.InputRegister:
                    var inputRegisters = ReadInputRegisters(data.StartAddress, data.Count);
                    data.WordData = inputRegisters.Select(a => (ushort?)a).ToList();
                    break;
                case ModbusDataType.HoldingRegister:
                    var holdingRegisters = ReadHoldingRegisters(data.StartAddress, data.Count);
                    data.WordData = holdingRegisters.Select(a => (ushort?)a).ToList();
                    break;
            }
        }

        public void WriteData(ModbusData data)
        {
            if (data == null) return;
            if (Master == null) return;
            switch (data.Type)
            {
                case ModbusDataType.Coil:
                    if (data.BitData == null || data.Count != data.BitData.Count) return;
                    WriteCoils(data.StartAddress, data.BitData.Select(a => a.HasValue && a.Value).ToList());
                    break;
                case ModbusDataType.HoldingRegister:
                    if (data.WordData == null || data.Count != data.WordData.Count) return;
                    WriteHoldingRegisters(data.StartAddress, data.WordData.Select(a => a.HasValue ? a.Value : (ushort)0).ToList());
                    break;
            }
        }
    }
}
