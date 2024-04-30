using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Map.Extensions
{
    public static class MapExtension
    {
        public static Domain.Models.Map.Map IncludeLayers(this Domain.Models.Map.Map map, Map.Interfaces.IMapService mapService)
        {
            return mapService.IncludeLayers(map);
        }
        public static List<Domain.Models.Map.Map> IncludeLayers(this List<Domain.Models.Map.Map> map, Map.Interfaces.IMapService mapService)
        {
            return mapService.IncludeLayers(map);
        }

    }
}
