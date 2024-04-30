using BMS.Domain.ClientModels;

namespace BMS.Api.Helpers
{
    public static class StringExtentions
    {
        public static QueryParamModel ToQueryParam(this string value)
        {
            try
            {
                var qp = new QueryParamModel();
                if (!string.IsNullOrEmpty(value))
                    qp = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryParamModel>(value);
                if (string.IsNullOrEmpty(qp.sortOrder))
                    qp.sortOrder = "asc";
                return qp;
            }
            catch (Exception ex)
            {
                return new QueryParamModel();
            }
            
        }
    }
}
