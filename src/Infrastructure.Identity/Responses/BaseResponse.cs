namespace Infrastructure.Identity.Responses
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool IsSuccess { get; set; }
    }
}