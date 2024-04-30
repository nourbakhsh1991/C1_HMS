using BMS.Domain.BaseModels;
using BMS.Domain.Interfaces;
using BMS.Shared.Extentions;

namespace BMS.Domain.Models.Modbus
{
    public class MModbusEntity : MBaseDBModel, IMModel<ModbusEntity>
    {
        public string Name { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public long UpdateTime { get; set; }
        public long CreatedTime { get; set; }
        public bool Active { get; set; }
        public bool IsConnected { get; set; }
        public ModbusEntity GetBase()
        {
            return new ModbusEntity
            {
                Id = Id,
                Metadata = Metadata.GetBsonObject(),
                ServerAddress = ServerAddress,
                ServerPort = ServerPort,
                UpdateTime = UpdateTime != -1 ? new DateTime(UpdateTime) : new DateTime(),
                CreatedTime = CreatedTime != -1 ? new DateTime(CreatedTime) : new DateTime(),
                IsConnected = IsConnected,
                Name = Name,
                Active = Active
            };
        }
    }
}
