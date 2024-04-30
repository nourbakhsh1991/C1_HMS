using MongoDB.Bson;

namespace BMS.Domain.BaseModels
{
    public static class UniqueIdentifier
    {
        public static string New => ObjectId.GenerateNewId().ToString();
    }
}
