using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TaskProject.Services.EmailSender
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly string apikey;
        private readonly string ownerEmail;

        public EmailSender(IConfiguration _configuration)
        {
            configuration = _configuration;
            apikey = configuration.GetSection("SendGrid_API_KEY").Value;
            ownerEmail = configuration.GetSection("Owner_Email").Value;
        }

        public async Task SendEmailToOwnerAsync(string userEmail, string subject, string message)
        {
            var client = new SendGridClient(apikey);
            var mail = new SendGridMessage()
            {
                Subject = subject,
                From = new EmailAddress(userEmail),
                PlainTextContent = message
            };

            mail.AddTo(new EmailAddress(ownerEmail));

            var response = await client.SendEmailAsync(mail);
        }


        public async Task SendEmailToUserAsync(string userEmail, string subject, string message)
        {
            var client = new SendGridClient(apikey);
            var mail = new SendGridMessage()
            {
                Subject = subject,
                From = new EmailAddress(ownerEmail),
                PlainTextContent = message
            };

            mail.AddTo(new EmailAddress(userEmail));

            var response = await client.SendEmailAsync(mail);
        }
    }
}
