using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;
using MongoDB.Bson.Serialization.Attributes;

namespace BMS.Domain.Models.Modbus
{
    public class ModbusEntityRegister : BaseDBModel, IBaseModel<MModbusEntityRegister>
    {
        public ModbusEntityRegister()
        {
            Data = new List<KeyValuePair<long, ModbusEntityData>?>();
            for (var i = 0; i < 20; i++)
                Data.Add(null);
        }

        public string ModbusId { get; set; }
        [BsonIgnore]
        public ModbusEntity Modbus { get; set; }
        public List<KeyValuePair<long, ModbusEntityData>?> Data { get; set; }
        public int DataLength { get; set; }
        public int DataIndex { get; set; }
        public int Type { get; set; }
        public ushort StartAddress { get; set; }
        public ushort Count { get; set; }

        public MModbusEntityRegister GetM()
        {
            return new MModbusEntityRegister
            {
                Id = Id,
                Metadata = Metadata.GetDotNetObject(),
                Count = Count,
                Data = Data,
                Type = Type,
                StartAddress = StartAddress,
                DataIndex = DataIndex,
                DataLength = DataLength,
                Modbus = Modbus.GetM(),
            };
        }
    }
}
