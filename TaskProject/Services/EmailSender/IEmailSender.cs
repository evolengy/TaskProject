using System.Threading.Tasks;

namespace TaskProject.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailToUserAsync(string userEmail, string subject, string message);

        Task SendEmailToOwnerAsync(string userEmail, string subject, string message);
    }
}
