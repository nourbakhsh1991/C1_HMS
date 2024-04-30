using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;
using MongoDB.Bson.Serialization.Attributes;

namespace BMS.Domain.Models.Modbus
{
    public class ModbusEntity : BaseDBModel, IBaseModel<MModbusEntity>
    {
        public string Name { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Active { get; set; }
        public bool IsConnected { get; set; }
        public MModbusEntity GetM()
        {
            return new MModbusEntity
            {
                Id = Id,
                Metadata = Metadata.GetDotNetObject(),
                ServerAddress = ServerAddress,
                ServerPort = ServerPort,
                UpdateTime = UpdateTime.Ticks,
                CreatedTime = CreatedTime.Ticks,
                IsConnected = IsConnected,
                Name = Name,
                Active = Active
            };
        }
    }
}
