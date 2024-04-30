using BMS.Domain.Interfaces;
using BMS.Domain.Models.Modbus;
using BMS.Services.Modbus.Interfaces;

namespace BMS.Services.Modbus
{
    public class ModbusService : IModbusService
    {
        IRepository<ModbusEntity> repository;

        public ModbusService(IRepository<ModbusEntity> repository)
        {
            this.repository = repository;
        }

        public async Task Insert(ModbusEntity modbus)
        {
            if (modbus == null)
                throw new ArgumentNullException("Modbus can not be null");
            await repository.InsertAsync(modbus);
        }

        public async Task Insert(List<ModbusEntity> modbuses)
        {
            if (modbuses == null)
                throw new ArgumentNullException("Modbus list can not be null");
            await repository.InsertAsync(modbuses);
        }

        public async Task Update(ModbusEntity modbus)
        {
            if (modbus == null)
                throw new ArgumentNullException("Modbus can not be null");
            await repository.UpdateAsync(modbus);
        }

        public async Task Update(List<ModbusEntity> modbuses)
        {
            if (modbuses == null)
                throw new ArgumentNullException("Modbus list can not be null");
            await repository.UpdateAsync(modbuses);
        }

        public async Task Delete(ModbusEntity modbus)
        {
            if (modbus == null)
                throw new ArgumentNullException("Modbus can not be null");
            await repository.DeleteAsync(modbus);
        }

        public async Task Delete(List<ModbusEntity> modbuses)
        {
            if (modbuses == null)
                throw new ArgumentNullException("Modbus list can not be null");
            await repository.DeleteAsync(modbuses);
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Modbus id can not be null or empty");
            var modbus = GetById(id);
            if (modbus == null)
                throw new NullReferenceException("Modbus can not be null");
            await repository.DeleteAsync(modbus);
        }

        public ModbusEntity GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Modbus id can not be null or empty");
            ModbusEntity modbus = repository.GetById(id);
            return modbus;
        }

        public List<ModbusEntity> GetAll()
        {
            return repository.GetAllAsync().Result;
        }

        public IQueryable<ModbusEntity> GetAsQueryable()
        {
            return repository.Table;
        }
    }
}
