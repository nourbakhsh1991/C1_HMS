using BMS.Domain.Models.Modbus;

namespace BMS.Services.Modbus.Extentions
{
    public static class ModbusExtentions
    {
        public static ModbusEntityHistory? IncludeModbus(this ModbusEntityHistory history,
                                                        Interfaces.IModbusHistoryService historyService)
        {
            return historyService.IncludeModbus(history);
        }

        public static List<ModbusEntityHistory>? IncludeModbus(this List<ModbusEntityHistory> histories,
                                                              Interfaces.IModbusHistoryService historyService)
        {
            return historyService.IncludeModbus(histories);
        }
        public static ModbusEntityRegister? IncludeModbus(this ModbusEntityRegister register,
                                                        Interfaces.IModbusRegisterService registerService)
        {
            return registerService.IncludeModbus(register);
        }

        public static List<ModbusEntityRegister>? IncludeModbus(this List<ModbusEntityRegister> registers,
                                                              Interfaces.IModbusRegisterService registerService)
        {
            return registerService.IncludeModbus(registers);
        }
    }
}
