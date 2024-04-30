using BMS.Domain.Models.Modbus;

namespace BMS.Services.Modbus.Interfaces
{
    public interface IModbusHistoryService
    {
        Task Delete(ModbusEntityHistory history);
        Task Delete(List<ModbusEntityHistory> histories);
        Task Delete(string id);
        List<ModbusEntityHistory> GetAll();
        IQueryable<ModbusEntityHistory> GetAsQueryable();
        ModbusEntityHistory GetById(string id);
        ModbusEntityHistory? IncludeModbus(ModbusEntityHistory history);
        List<ModbusEntityHistory>? IncludeModbus(List<ModbusEntityHistory> histories);
        Task Insert(ModbusEntityHistory history);
        Task Insert(List<ModbusEntityHistory> histories);
        Task Update(ModbusEntityHistory history);
        Task Update(List<ModbusEntityHistory> histories);
    }
}
