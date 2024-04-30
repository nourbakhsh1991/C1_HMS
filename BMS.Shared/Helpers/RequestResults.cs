namespace BMS.Shared.Helpers
{
    public static class RequestResults
    {
        public static RequestResult Successful => new RequestResult()
        {
            Code = 200,
            Message = "موفقیت آمیز"
        };
        public static RequestResult NotFound => new RequestResult()
        {
            Code = 404,
            Message = "پیدا نشد"
        };
        public static RequestResult BadRequest => new RequestResult()
        {
            Code = 400,
            Message = "درخواست نامعتبر"
        };
        public static RequestResult InternalServerError => new RequestResult()
        {
            Code = 500,
            Message = "خطای پیش بینی نشده"
        };
        public static RequestResult Created => new RequestResult()
        {
            Code = 201,
            Message = "با موفقیت ایجاد شد"
        };
        public static RequestResult Unauthorized => new RequestResult()
        {
            Code = 401,
            Message = "دسترسی غیر مجاز"
        };
    }
}
