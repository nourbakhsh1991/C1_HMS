using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.UserManagement;
using BMS.Services.Map.Interfaces;

namespace BMS.Services.Map
{
    public class MapService : IMapService
    {
        IRepository<Domain.Models.Map.Map> _repository;
        IRepository<Domain.Models.Map.Layer> _repositoryLayer;
        public MapService(IRepository<Domain.Models.Map.Map> repository,
            IRepository<Domain.Models.Map.Layer> repositoryLayer)
        {
            _repository = repository;
            _repositoryLayer = repositoryLayer;
        }

        public async Task Insert(Domain.Models.Map.Map map)
        {
            if (map == null)
                throw new Exception();
            map.Id = null;
            map.CreatedTime = DateTime.UtcNow;
            map.UpdatedTime = DateTime.UtcNow;
            await _repository.InsertAsync(map);
        }

        public async Task Insert(List<Domain.Models.Map.Map> maps)
        {
            if (maps == null)
                throw new Exception();
            foreach (var map in maps)
            {
                map.Id = null;
                map.CreatedTime = DateTime.UtcNow;
                map.UpdatedTime = DateTime.UtcNow;
            }
            await _repository.InsertAsync(maps);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public async Task Update(Domain.Models.Map.Map map)
        {
            if (map == null)
                throw new NullReferenceException("map cannot be null");
            map.UpdatedTime = DateTime.Now;
            await _repository.UpdateAsync(map);
        }

        public async Task Delete(Domain.Models.Map.Map map)
        {
            if (map == null)
                throw new NullReferenceException("map cannot be null");
            await _repository.DeleteAsync(map);
        }

        public async Task Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException("Id cannot be null");
            var map = GetById(Id);
            await _repository.DeleteAsync(map);
        }

        public Domain.Models.Map.Map GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Domain.Models.Map.Map> GetByName(string name)
        {
            return _repository.Table.Where(x => x.Name.Contains(name)).ToList();
        }

        public List<Domain.Models.Map.Map> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<Domain.Models.Map.Map> GetAsQueryable()
        {
            return _repository.Table;
        }

        public Domain.Models.Map.Map IncludeLayers(Domain.Models.Map.Map map)
        {
            var layers = _repositoryLayer.Table.Where(a => a.MapId == map.Id).ToList();
            if (layers == null || layers.Count == 0) return map;
            map.Layers = layers;
            return map;
        }

        public List<Domain.Models.Map.Map> IncludeLayers(List<Domain.Models.Map.Map> maps)
        {
            var mapIds = maps.Select(a => a.Id).ToList();
            var layers = _repositoryLayer.Table.Where(a => mapIds.Contains(a.MapId)).ToList();
            if (layers == null || layers.Count == 0) return maps;
            foreach(var map in maps)
            {
                map.Layers = layers.Where(a => a.MapId == map.Id).ToList();
            }
            return maps;
        }
    }
}
