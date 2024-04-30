using BMS.Domain.Interfaces;
using BMS.Domain.Models.Modbus;
using BMS.Services.Modbus.Interfaces;
using System.Linq;

namespace BMS.Services.Modbus
{
    public class ModbusRegisterService : IModbusRegisterService
    {
        IRepository<ModbusEntityRegister> repository;
        IRepository<ModbusEntity> repositoryModbus;

        public ModbusRegisterService(IRepository<ModbusEntityRegister> repository,
                                     IRepository<ModbusEntity> repositoryModbus)
        {
            this.repository = repository;
            this.repositoryModbus = repositoryModbus;
        }

        public async Task Insert(ModbusEntityRegister register)
        {
            if (register == null)
                throw new ArgumentNullException("Register can not be null");
            await repository.InsertAsync(register);
        }

        public async Task Insert(List<ModbusEntityRegister> registers)
        {
            if (registers == null)
                throw new ArgumentNullException("Register list can not be null");
            await repository.InsertAsync(registers);
        }

        public async Task Update(ModbusEntityRegister register)
        {
            if (register == null)
                throw new ArgumentNullException("Register can not be null");
            await repository.UpdateAsync(register);
        }

        public async Task Update(List<ModbusEntityRegister> registers)
        {
            if (registers == null)
                throw new ArgumentNullException("Register list can not be null");
            await repository.UpdateAsync(registers);
        }

        public async Task Delete(ModbusEntityRegister register)
        {
            if (register == null)
                throw new ArgumentNullException("Register can not be null");
            await repository.DeleteAsync(register);
        }

        public async Task Delete(List<ModbusEntityRegister> registers)
        {
            if (registers == null)
                throw new ArgumentNullException("Register list can not be null");
            await repository.DeleteAsync(registers);
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Register id can not be null or empty");
            var register = GetById(id);
            if (register == null)
                throw new NullReferenceException("Register can not be null");
            await repository.DeleteAsync(register);
        }

        public ModbusEntityRegister GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Register id can not be null or empty");
            var register = repository.GetById(id);
            return register;
        }

        public List<ModbusEntityRegister> GetAll()
        {
            return repository.GetAllAsync().Result;
        }

        public List<ModbusEntityRegister> GetByModbusId(string modbusId)
        {
            if (string.IsNullOrEmpty(modbusId))
                throw new ArgumentNullException("Modbus id can not be null or empty");
            return repository.GetAllAsync().Result.Where(a => a.ModbusId == modbusId).ToList();
        }

        public IQueryable<ModbusEntityRegister> GetAsQueryable()
        {
            return repository.Table;
        }

        public ModbusEntityRegister? IncludeModbus(ModbusEntityRegister register)
        {
            if (register != null)
            {
                var modbus = repositoryModbus.GetById(register.ModbusId);
                if (modbus != null)
                    register.Modbus = modbus;
            }
            return register;
        }

        public List<ModbusEntityRegister>? IncludeModbus(List<ModbusEntityRegister> registers)
        {
            if (registers != null && registers.Count > 0)
            {
                foreach (var register in registers)
                {
                    var modbus = repositoryModbus.GetById(register.ModbusId);
                    if (modbus != null)
                        register.Modbus = modbus;
                }
            }
            return registers;
        }
    }
}
