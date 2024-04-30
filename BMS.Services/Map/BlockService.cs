using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACadSharp.Entities;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Map.Interfaces;

namespace BMS.Services.Map
{
    public class BlockService : IBlockService
    {
        IRepository<Domain.Models.Map.Block> _repository;
        IRepository<Domain.Models.Map.Map> _repositoryMap;
        IRepository<Domain.Models.Map.Layer> _repositoryLayer;
        public BlockService(IRepository<Domain.Models.Map.Block> repository,
            IRepository<Domain.Models.Map.Map> repositoryMap,
            IRepository<Domain.Models.Map.Layer> repositoryLayer)
        {
            _repository = repository;
            _repositoryMap = repositoryMap;
            _repositoryLayer = repositoryLayer;
        }

        public async Task Insert(Domain.Models.Map.Block entity)
        {
            if (entity == null)
                throw new Exception();
            await _repository.InsertAsync(entity);
        }

        public async Task Insert(List<Domain.Models.Map.Block> entities)
        {
            if (entities == null)
                throw new Exception();
            await _repository.InsertAsync(entities);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public async Task Update(Domain.Models.Map.Block entity)
        {
            if (entity == null)
                throw new NullReferenceException("map cannot be null");
            await _repository.UpdateAsync(entity);
        }

        public async Task Delete(Domain.Models.Map.Block entity)
        {
            if (entity == null)
                throw new NullReferenceException("map cannot be null");
            await _repository.DeleteAsync(entity);
        }

        public async Task Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException("Id cannot be null");
            var entity = GetById(Id);
            await _repository.DeleteAsync(entity);
        }

        public Domain.Models.Map.Block GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Domain.Models.Map.Block> GetByName(string name)
        {
            return _repository.Table.Where(x => x.Name == name).ToList();
        }

        public List<Domain.Models.Map.Block> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<Domain.Models.Map.Block> GetAsQueryable()
        {
            return _repository.Table;
        }
    }
}
