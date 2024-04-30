using BMS.Domain.Models.Modbus;

namespace BMS.Services.Modbus.Interfaces
{
    public interface IModbusRegisterService
    {
        Task Delete(ModbusEntityRegister register);
        Task Delete(List<ModbusEntityRegister> registers);
        Task Delete(string id);
        List<ModbusEntityRegister> GetAll();
        IQueryable<ModbusEntityRegister> GetAsQueryable();
        ModbusEntityRegister GetById(string id);
        List<ModbusEntityRegister> GetByModbusId(string modbusId);
        ModbusEntityRegister? IncludeModbus(ModbusEntityRegister register);
        List<ModbusEntityRegister>? IncludeModbus(List<ModbusEntityRegister> registers);
        Task Insert(ModbusEntityRegister register);
        Task Insert(List<ModbusEntityRegister> registers);
        Task Update(ModbusEntityRegister register);
        Task Update(List<ModbusEntityRegister> registers);
    }
}
