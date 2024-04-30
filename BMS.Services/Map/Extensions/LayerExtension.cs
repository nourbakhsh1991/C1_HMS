using BMS.Domain.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Map.Extensions
{
    public static class LayerExtension
    {
        public static Domain.Models.Map.Layer IncludeMap(this Domain.Models.Map.Layer layer, Map.Interfaces.ILayerService layerService)
        {
            return layerService.IncludeMap(layer);
        }
        public static List<Domain.Models.Map.Layer> IncludeMap(this List<Domain.Models.Map.Layer> layer, Map.Interfaces.ILayerService layerService)
        {
            return layerService.IncludeMap(layer);
        }
        
            public static Domain.Models.Map.Layer IncludeEntities(this Domain.Models.Map.Layer layer, Map.Interfaces.ILayerService layerService)
        {
            return layerService.IncludeEntities(layer);
        }
        public static List<Domain.Models.Map.Layer> IncludeEntities(this List<Domain.Models.Map.Layer> layer, Map.Interfaces.ILayerService layerService)
        {
            return layerService.IncludeEntities(layer);
        }

    }
}
