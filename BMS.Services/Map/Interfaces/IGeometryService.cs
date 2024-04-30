using BMS.Domain.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Map.Interfaces
{
    public interface IGeometryService
    {
        Task Delete(Entity entity);
        Task Delete(string Id);
        Task DeleteAll();
        List<Entity> GetAll();
        IQueryable<Entity> GetAsQueryable();
        Entity GetById(string id);
        List<Entity> GetByLayerId(string id);
        Task Insert(Entity entity);
        Task Insert(List<Entity> entities);
        Task Update(Entity entity);
    }
}
