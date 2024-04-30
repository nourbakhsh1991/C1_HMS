using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;

namespace BMS.Domain.Models.Modbus
{
    public class MModbusEntityHistory : MBaseDBModel, IMModel<ModbusEntityHistory>
    {
        public MModbusEntity Modbus { get; set; }
        public List<KeyValuePair<long, ModbusEntityData>?> Data { get; set; }
        public long CreatedTime { get; set; }
        public int Type { get; set; }
        public ushort StartAddress { get; set; }
        public ushort Count { get; set; }

        public ModbusEntityHistory GetBase()
        {
            return new ModbusEntityHistory
            {
                Id = Id,
                Metadata = Metadata.GetBsonObject(),
                Count = Count,
                Data = Data,
                CreatedTime = CreatedTime != -1 ? new DateTime(CreatedTime) : new DateTime(),
                Type = Type,
                StartAddress = StartAddress,
                Modbus = Modbus.GetBase(),
                ModbusId = Modbus.Id
            };
        }
    }
}
