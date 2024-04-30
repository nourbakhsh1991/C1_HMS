namespace BMS.Services.Map.Interfaces
{
    public interface IMapService
    {
        Task Delete(Domain.Models.Map.Map map);
        Task Delete(string Id);
        Task DeleteAll();
        List<Domain.Models.Map.Map> GetAll();
        IQueryable<Domain.Models.Map.Map> GetAsQueryable();
        Domain.Models.Map.Map GetById(string id);
        List<Domain.Models.Map.Map> GetByName(string name);
        Domain.Models.Map.Map IncludeLayers(Domain.Models.Map.Map map);
        List<Domain.Models.Map.Map> IncludeLayers(List<Domain.Models.Map.Map> maps);
        Task Insert(Domain.Models.Map.Map map);
        Task Insert(List<Domain.Models.Map.Map> maps);
        Task Update(Domain.Models.Map.Map map);
    }
}