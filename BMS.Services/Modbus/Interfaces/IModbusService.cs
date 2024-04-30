using BMS.Domain.Models.Modbus;

namespace BMS.Services.Modbus.Interfaces
{
    public interface IModbusService
    {
        Task Delete(ModbusEntity modbus);
        Task Delete(List<ModbusEntity> modbuses);
        Task Delete(string id);
        List<ModbusEntity> GetAll();
        IQueryable<ModbusEntity> GetAsQueryable();
        ModbusEntity GetById(string id);
        Task Insert(ModbusEntity modbus);
        Task Insert(List<ModbusEntity> modbuses);
        Task Update(ModbusEntity modbus);
        Task Update(List<ModbusEntity> modbuses);
    }
}
