using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.BaseModels
{
    public class BaseDBModel
    {
        protected BaseDBModel()
        {
            _id = UniqueIdentifier.New;
            Metadata = new Dictionary<string, object>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id
        {
            get { return _id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _id = UniqueIdentifier.New;
                else
                    _id = value;
            }
        }

        [BsonExtraElements]
        public Dictionary<string, object> Metadata { get; set; }

        private string _id;
    }
}
