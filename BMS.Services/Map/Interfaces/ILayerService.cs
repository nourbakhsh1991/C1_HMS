using BMS.Domain.Models.Map;

namespace BMS.Services.Map.Interfaces
{
    public interface ILayerService
    {
        Task Delete(Domain.Models.Map.Layer layer);
        Task Delete(string id);
        Task Delete(List<Layer> layers);
        Task Delete(List<string> ids);
        Task DeleteAll();
        List<Domain.Models.Map.Layer> GetAll();
        IQueryable<Domain.Models.Map.Layer> GetAsQueryable();
        Domain.Models.Map.Layer GetById(string id);
        List<Domain.Models.Map.Layer> GetByName(string name);
        Layer IncludeEntities(Layer layer);
        List<Layer> IncludeEntities(List<Layer> layers);
        Domain.Models.Map.Layer IncludeMap(Domain.Models.Map.Layer layer);
        List<Domain.Models.Map.Layer> IncludeMap(List<Domain.Models.Map.Layer> layers);
        Task Insert(Domain.Models.Map.Layer layer);
        Task Insert(List<Domain.Models.Map.Layer> layers);
        Task Update(Domain.Models.Map.Layer layer);
        Task Update(List<Layer> layers);
    }
}