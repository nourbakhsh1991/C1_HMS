using BMS.Domain.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Map.Interfaces
{
    public interface IBlockService
    {
        Task Delete(Block entity);
        Task Delete(string Id);
        Task DeleteAll();
        List<Block> GetAll();
        IQueryable<Block> GetAsQueryable();
        Block GetById(string id);
        List<Block> GetByName(string name);
        Task Insert(Block entity);
        Task Insert(List<Block> entities);
        Task Update(Block entity);
    }
}
