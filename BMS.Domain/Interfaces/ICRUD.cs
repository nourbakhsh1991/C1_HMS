using BMS.Domain.BaseModels;

namespace BMS.Domain.Interfaces
{
    public interface ICRUD<T> where T : BaseDBModel
    {
        Task<T> Add(T model);
        Task<T> Edit(T model);
        Task<bool> Remove(T model);

        T GetById(string id);
        List<T> GetByName(string name);
        List<T> GetAll();
    }
}
