using BMS.Domain.Interfaces;
using BMS.Domain.Models.Map;
using BMS.Services.Map.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Map
{
    public class LayerService : ILayerService
    {
        IRepository<Domain.Models.Map.Layer> _repository;
        IRepository<Domain.Models.Map.Map> _repositoryMap;

        IRepository<Domain.Models.Map.Entity> _repositoryEntity;
        public LayerService(IRepository<Domain.Models.Map.Layer> repository,
            IRepository<Domain.Models.Map.Map> repositoryMap,
            IRepository<Domain.Models.Map.Entity> repositoryEntity)
        {
            _repository = repository;
            _repositoryMap = repositoryMap;
            _repositoryEntity = repositoryEntity;
        }

        public async Task Insert(Domain.Models.Map.Layer layer)
        {
            if (layer == null)
                throw new Exception();
            await _repository.InsertAsync(layer);
        }

        public async Task Insert(List<Domain.Models.Map.Layer> layers)
        {
            if (layers == null)
                throw new Exception();
            await _repository.InsertAsync(layers);
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteManyAsync((attribute) => true);
        }

        public async Task Update(Domain.Models.Map.Layer layer)
        {
            if (layer == null)
                throw new NullReferenceException("layer cannot be null");
            layer.UpdatedTime = DateTime.Now;
            await _repository.UpdateAsync(layer);
        }

        public async Task Update(List<Domain.Models.Map.Layer> layers)
        {
            if (layers == null || layers.Count == 0)
                throw new NullReferenceException("Layers can not be null or empty");
            layers.ForEach(layer => layer.UpdatedTime = DateTime.UtcNow);
            await _repository.UpdateAsync(layers);
        }

        public async Task Delete(Domain.Models.Map.Layer layer)
        {
            if (layer == null)
                throw new NullReferenceException("layer cannot be null");
            await _repository.DeleteAsync(layer);
        }

        public async Task Delete(List<Domain.Models.Map.Layer> layers)
        {
            if (layers == null || layers.Count == 0)
                throw new NullReferenceException("Layers can not be null or empty");
            await _repository.DeleteAsync(layers);
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("Id cannot be null");
            var layer = GetById(id);
            await _repository.DeleteAsync(layer);
        }

        public async Task Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
                throw new NullReferenceException("Id list can not be null or empty");
            var layers = ids.Aggregate(new List<Layer>(), (list, id) =>
            {
                var layer = GetById(id);
                list.Add(layer);
                return list;
            });
            await _repository.DeleteAsync(layers);
        }

        public Domain.Models.Map.Layer GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<Domain.Models.Map.Layer> GetByName(string name)
        {
            return _repository.Table.Where(x => x.Name.Contains(name)).ToList();
        }

        public List<Domain.Models.Map.Layer> GetAll()
        {
            return _repository.GetAllAsync().Result;
        }

        public IQueryable<Domain.Models.Map.Layer> GetAsQueryable()
        {
            return _repository.Table;
        }

        public Domain.Models.Map.Layer IncludeMap(Domain.Models.Map.Layer layer)
        {
            var map = _repositoryMap.Table.FirstOrDefault(a => a.Id == layer.MapId);
            if (map == null) return layer;
            layer.Map = map;
            return layer;
        }

        public Domain.Models.Map.Layer IncludeEntities(Domain.Models.Map.Layer layer)
        {
            var entities = _repositoryEntity.Table.Where(a => a.LayerId == layer.Id).ToList();
            if (entities == null) return layer;
            layer.Entities = entities;
            return layer;
        }
        public List<Domain.Models.Map.Layer> IncludeEntities(List<Domain.Models.Map.Layer> layers)
        {
            var layerIds = layers.Select(a => a.Id).ToList();
            var entities = _repositoryEntity.Table.Where(a => layerIds.Contains(a.LayerId)).ToList();
            if (entities == null || entities.Count == 0) return layers;
            foreach (var layer in layers)
            {
                layer.Entities = entities.Where(a => a.LayerId == layer.Id).ToList();
            }
            return layers;
        }

        public List<Domain.Models.Map.Layer> IncludeMap(List<Domain.Models.Map.Layer> layers)
        {
            var mapIds = layers.Select(a => a.MapId).ToList();
            var maps = _repositoryMap.Table.Where(a => mapIds.Contains(a.Id)).ToList();
            if (maps == null || maps.Count == 0) return layers;
            foreach (var layer in layers)
            {
                layer.Map = maps.FirstOrDefault(a => a.Id == layer.MapId);
            }
            return layers;
        }
    }
}
