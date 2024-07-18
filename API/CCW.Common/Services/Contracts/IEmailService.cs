using CCW.Common.Models;

namespace CCW.Common.Services.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string licensingEmail);
    }
}
