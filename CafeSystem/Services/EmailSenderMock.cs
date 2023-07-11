using Microsoft.AspNetCore.Identity.UI.Services;

namespace CafeSystem.Services;

public class EmailSenderMock : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}