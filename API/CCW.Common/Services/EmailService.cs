using Microsoft.Graph;
using Azure.Identity;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using CCW.Common.Services.Contracts;

namespace CCW.Common.Services;
public class EmailService : IEmailService
{
    private readonly GraphServiceClient _graphClient;

    public EmailService() 
    {
        var clientSecretCredential = new ClientSecretCredential("TENANT_ID", "client_id", "CLIENT_SECRET");

        _graphClient = new GraphServiceClient(clientSecretCredential);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Text,
                Content = body
            },
            ToRecipients = new List<Recipient>
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = toEmail
                    }
                }
            }
        };

        var sendMailBody = new SendMailPostRequestBody
        {
            Message = message,
            SaveToSentItems = true
        };

        await _graphClient.Users["USER_ID"].SendMail.PostAsync(sendMailBody);
    }
}
