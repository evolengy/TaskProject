using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {

            string apikey = configuration.GetSection("SendGrid_API_KEY").Value;

            var client = new SendGridClient(apikey);
            var mail = new SendGridMessage()
            {
                Subject = subject,
                From = new EmailAddress(email),
                PlainTextContent = message
            };

            mail.AddTo(new EmailAddress("evpetruhin@gmail.com"));

            var response = await client.SendEmailAsync(mail);
        }
    }
}
