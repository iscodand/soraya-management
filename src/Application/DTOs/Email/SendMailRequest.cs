namespace Application.DTOs.Email
{
    public class SendMailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplatePath { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}