using Microsoft.AspNetCore.Identity.UI.Services;

namespace Online_Art_Gallery.Models.Data
{
    public class EmailSender : IEmailSender
    {
        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
