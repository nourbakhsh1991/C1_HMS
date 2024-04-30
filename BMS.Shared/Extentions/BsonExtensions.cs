using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Extentions
{
    public static class BsonExtensions
    {
        public static object GetDotNetValue(this BsonValue value)
        {
            if (value == null) return null;
            if (value.IsDecimal128) return value.ToDecimal();
            if (value.IsBsonArray)
            {
                var list = new List<object>();
                for (int i = 0; i < value.AsBsonArray.Count; i++)
                {
                    list.Add(value.AsBsonArray[i].GetDotNetValue());
                }
                return list;
            }
            return value;
        }

        public static Dictionary<string, object> GetDotNetObject(this Dictionary<string, object> value)
        {
            return value
                .Select(a =>
                {
                    if (a.Value is Newtonsoft.Json.Linq.JToken elm)
                        return new KeyValuePair<string, object>(a.Key, elm.GetBaseObject());
                    return a;
                }).ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<string, object> GetBsonObject(this Dictionary<string, object> value)
        {
            return value
                .ToDictionary(
                    x => x.Key,
                    x => (x.Value is BsonValue bson) ? bson.GetDotNetValue() : x.Value);
        }
    }
}
