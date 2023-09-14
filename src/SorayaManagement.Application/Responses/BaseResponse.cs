namespace SorayaManagement.Application.Responses
{
    public class BaseResponse<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public ICollection<T> DataCollection { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}