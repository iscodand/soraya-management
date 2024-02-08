using Application.DTOs.Email;

namespace Application.Contracts.Services
{
    public interface IEmailService
    {
        public Task SendMailAsync(SendMailRequest request);
    }
}