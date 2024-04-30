using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BMS.Shared.Extentions
{
    public static class JsonExtension
    {

        public static object GetBaseObject(this Newtonsoft.Json.Linq.JToken elm)
        {
            if (elm.Type == Newtonsoft.Json.Linq.JTokenType.Null ||
                elm.Type == Newtonsoft.Json.Linq.JTokenType.Undefined ||
                elm.Type == Newtonsoft.Json.Linq.JTokenType.None) return null;
            if (elm.Type == Newtonsoft.Json.Linq.JTokenType.Integer)
            {
                var mByte = ((byte?)elm);
                if (mByte.HasValue) return mByte.Value;
                var mInt16 = ((short?)elm);
                if (mInt16.HasValue) return mInt16.Value;
                var mInt32 = ((int?)elm);
                if (mInt32.HasValue) return mInt32.Value;
                var mInt64 = ((long?)elm);
                if (mInt64.HasValue) return mInt64.Value;
            }
            if(elm.Type == Newtonsoft.Json.Linq.JTokenType.Float)
            {
                var mDeciaml = ((decimal?)elm);
                if (mDeciaml.HasValue) return mDeciaml.Value;
                var mfloat = ((float?)elm);
                if (mfloat.HasValue) return mfloat.Value;
                var mDouble = ((double?)elm);
                if (mDouble.HasValue) return mDouble.Value;
            }
            if (elm.Type == Newtonsoft.Json.Linq.JTokenType.Array)
            {
                var list = new List<object>();
                for (int i = 0; i < elm.Count(); i++)
                {
                    list.Add(elm[i].GetBaseObject());
                }
                return list;
            }
            return elm.ToString();
        }
    }
}
