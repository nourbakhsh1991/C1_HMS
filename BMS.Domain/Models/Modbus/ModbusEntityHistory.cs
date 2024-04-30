using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;
using MongoDB.Bson.Serialization.Attributes;

namespace BMS.Domain.Models.Modbus
{
    public class ModbusEntityHistory : BaseDBModel, IBaseModel<MModbusEntityHistory>
    {
        public string ModbusId { get; set; }
        [BsonIgnore]
        public ModbusEntity Modbus { get; set; }
        public List<KeyValuePair<long, ModbusEntityData>?> Data { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Type { get; set; }
        public ushort StartAddress { get; set; }
        public ushort Count { get; set; }

        public MModbusEntityHistory GetM()
        {
            return new MModbusEntityHistory
            {
                Id = Id,
                Metadata = Metadata.GetDotNetObject(),
                Count = Count,
                Data = Data,
                CreatedTime = CreatedTime.Ticks,
                Type = Type,
                Modbus = Modbus.GetM(),
                StartAddress = StartAddress
            };
        }
    }
}
