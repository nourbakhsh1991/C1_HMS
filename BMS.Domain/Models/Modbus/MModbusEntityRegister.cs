using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;

namespace BMS.Domain.Models.Modbus
{
    public class MModbusEntityRegister : MBaseDBModel, IMModel<ModbusEntityRegister>
    {
        public MModbusEntityRegister()
        {
            Data = new List<KeyValuePair<long, ModbusEntityData>?>();
            for (var i = 0; i < 20; i++)
                Data.Add(null);
        }

        public MModbusEntity? Modbus { get; set; }
        public List<KeyValuePair<long, ModbusEntityData>?> Data { get; set; } 
        public int DataLength { get; set; }
        public int DataIndex { get; set; }
        public int Type { get; set; }
        public ushort StartAddress { get; set; }
        public ushort Count { get; set; }

        public ModbusEntityRegister GetBase()
        {
            return new ModbusEntityRegister
            {
                Id = Id,
                Metadata = Metadata.GetBsonObject(),
                Count = Count,
                Data = Data,
                Type = Type,
                StartAddress = StartAddress,
                DataIndex = DataIndex,
                DataLength = DataLength,
                Modbus = Modbus.GetBase(),
                ModbusId = Modbus.Id
            };
        }
    }
}
