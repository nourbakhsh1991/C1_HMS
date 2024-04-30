using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Helpers
{
    public static class StrokeHelper
    {
        public static int GetLineWidth(ACadSharp.Entities.Entity entity)
        {
            if (entity == null) return 5;
            if ((int)entity.LineWeight >= 0)
                return (int)entity.LineWeight;
            if (entity.LineWeight == ACadSharp.LineweightType.Default)
                return 5;
            if (entity.LineWeight == ACadSharp.LineweightType.ByLayer)
                return GetLineWidth(entity.Layer);
            if (entity.LineWeight == ACadSharp.LineweightType.ByBlock)
                return -1;
            return 5;
        }

        public static int GetLineWidth(ACadSharp.Tables.Layer entity)
        {
            if (entity == null) return 5;
            if ((int)entity.LineWeight >= 0)
                return (int)entity.LineWeight;
            if (entity.LineWeight == ACadSharp.LineweightType.Default)
                return 5;
            return 5;
        }
    }
}
