using BMS.Domain.Interfaces;
using BMS.Domain.Models.Modbus;
using BMS.Services.Modbus.Interfaces;

namespace BMS.Services.Modbus
{
    public class ModbusHistoryService : IModbusHistoryService
    {
        IRepository<ModbusEntityHistory> repository;
        IRepository<ModbusEntity> repositoryModbus;

        public ModbusHistoryService(IRepository<ModbusEntityHistory> repository,
                                    IRepository<ModbusEntity> repositoryModbus)
        {
            this.repository = repository;
            this.repositoryModbus = repositoryModbus;
        }

        public async Task Insert(ModbusEntityHistory history)
        {
            if (history == null)
                throw new ArgumentNullException("History can not be null");
            await repository.InsertAsync(history);
        }

        public async Task Insert(List<ModbusEntityHistory> histories)
        {
            if (histories == null)
                throw new ArgumentNullException("History list can not be null");
            await repository.InsertAsync(histories);
        }

        public async Task Update(ModbusEntityHistory history)
        {
            if (history == null)
                throw new ArgumentNullException("History can not be null");
            await repository.UpdateAsync(history);
        }

        public async Task Update(List<ModbusEntityHistory> histories)
        {
            if (histories == null)
                throw new ArgumentNullException("History list can not be null");
            await repository.UpdateAsync(histories);
        }

        public async Task Delete(ModbusEntityHistory history)
        {
            if (history == null)
                throw new ArgumentNullException("History can not be null");
            await repository.DeleteAsync(history);
        }

        public async Task Delete(List<ModbusEntityHistory> histories)
        {
            if (histories == null)
                throw new ArgumentNullException("History list can not be null");
            await repository.DeleteAsync(histories);
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("History id can not be null or empty");
            var history = GetById(id);
            if (history == null)
                throw new NullReferenceException("History can not be null");
            await repository.DeleteAsync(history);
        }

        public ModbusEntityHistory GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("History id can not be null or empty");
            var history = repository.GetById(id);
            return history;
        }

        public List<ModbusEntityHistory> GetAll()
        {
            return repository.GetAllAsync().Result;
        }

        public IQueryable<ModbusEntityHistory> GetAsQueryable()
        {
            return repository.Table;
        }

        public ModbusEntityHistory? IncludeModbus(ModbusEntityHistory history)
        {
            if (history != null)
            {
                var modbus = repositoryModbus.GetById(history.ModbusId);
                if (modbus != null)
                    history.Modbus = modbus;
            }
            return history;
        }

        public List<ModbusEntityHistory>? IncludeModbus(List<ModbusEntityHistory> histories)
        {
            if (histories != null && histories.Count > 0)
            {
                foreach (var history in histories)
                {
                    var modbus = repositoryModbus.GetById(history.ModbusId);
                    if (modbus != null)
                        history.Modbus = modbus;
                }
            }
            return histories;
        }
    }
}
