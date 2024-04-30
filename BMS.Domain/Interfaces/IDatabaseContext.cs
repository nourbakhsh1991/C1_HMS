using System.Linq;
using System.Threading.Tasks;

namespace BMS.Domain.Interfaces
{
    public interface IDatabaseContext
    {
        IQueryable<T> Table<T>(string collectionName);
        Task<bool> DatabaseExist(string connectionString);
        Task CreateTable(string name, string collation);
        Task CreateDatabase();
    }
}
