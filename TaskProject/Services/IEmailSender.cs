using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Services
{
    public interface IEmailSender
    {
        Task SendEmailToUserAsync(string userEmail, string subject, string message);

        Task SendEmailToOwnerAsync(string userEmail, string subject, string message);
    }
}
