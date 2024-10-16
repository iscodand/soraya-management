namespace Application.Wrappers
{
    public class Response<T> where T : class
    {
        public string Message { get; set; }
        public T Data { get; set; }
        public int Status { get; set; }
        public List<string> Errors { get; set; }
        public bool Succeeded { get; set; }

        public Response()
        {
            
        }

        public Response(string message, T data, int status)
        {
            Message = message;
            Data = data;
            Succeeded = true;
            Errors = null;
            Status = status;
        }

        public Response(string message, List<string> errors, int status)
        {
            Message = message;
            Errors = errors;
            Succeeded = false;
            Errors = errors;
            Status = status;
        }
    }
}