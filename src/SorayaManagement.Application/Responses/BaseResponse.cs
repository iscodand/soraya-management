namespace SorayaManagement.Application.Responses
{
    // improve this inheritance
    public class BaseResponse<T> : Infrastructure.Identity.Responses.BaseResponse
    {
        public T Data { get; set; }
    }
}