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
    public class GeometryService : IGeometryService
    {
        IRepository<Domain.Models.Map.Entity> _repository;
        IRepository<Domain.Models.Map.Map> _repositoryMap;
        IRepository<Domain.Models.Map.Layer> _repositoryLayer;
        public GeometryService(IRepository<Domain.Models.Map.Entity> repository,
            IRepository<Domain.Models.Map.Map> repositoryMap,
            IRepository<Domain.Models.Map.Layer> repositoryLayer)
        {
            _repository = repository;
            _repositoryMap = repositoryMap;
            _repositoryLayer = repositoryLayer;
        }

        public async Task Insert(Domain.Models.Map.Entity entity)
        {
            if (entity == null)
                throw new Exception();
            await _repository.InsertAsync(entity);
        }

        public async Task Insert(List<Domain.Models.Map.Entity> entities)
        {
            if (entities == null)
                throw new Exception();
            await _repository.InsertAsync(entities);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public async Task Update(Domain.Models.Map.Entity entity)
        {
            if (entity == null)
                throw new NullReferenceException("map cannot be null");
            await _repository.UpdateAsync(entity);
        }

        public async Task Delete(Domain.Models.Map.Entity entity)
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

        public Domain.Models.Map.Entity GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Domain.Models.Map.Entity> GetByLayerId(string id)
        {
            return _repository.Table.Where(x => x.LayerId == id).ToList();
        }

        public List<Domain.Models.Map.Entity> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<Domain.Models.Map.Entity> GetAsQueryable()
        {
            return _repository.Table;
        }

        //public Domain.Models.Map.Entity IncludeLayers(Domain.Models.Map.Entity entity)
        //{
        //    var layers = _repositoryLayer.Table.Where(a => a.MapId == map.Id).ToList();
        //    if (layers == null || layers.Count == 0) return map;
        //    map.Layers = layers;
        //    return map;
        //}

        //public List<Domain.Models.Map.Entity> IncludeLayers(List<Domain.Models.Map.Entity> maps)
        //{
        //    var mapIds = maps.Select(a => a.Id).ToList();
        //    var layers = _repositoryLayer.Table.Where(a => mapIds.Contains(a.MapId)).ToList();
        //    if (layers == null || layers.Count == 0) return maps;
        //    foreach(var map in maps)
        //    {
        //        map.Layers = layers.Where(a => a.MapId == map.Id).ToList();
        //    }
        //    return maps;
        //}
    }
}
