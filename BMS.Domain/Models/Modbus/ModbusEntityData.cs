namespace BMS.Domain.Models.Modbus
{
    public class ModbusEntityData
    {
        public List<bool?> BitData { get; set; } = new List<bool?>();
        public List<ushort?> WordData { get; set; } = new List<ushort?>();
    }
}
