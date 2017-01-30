
namespace AS.CMS.Domain.Dto
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
