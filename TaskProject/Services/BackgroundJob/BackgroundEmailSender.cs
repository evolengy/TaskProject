using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;
using TaskProject.Services.EmailSender;

namespace TaskProject.Services.BackgroundJob
{
    public class BackgroundEmailSender
    {
        private readonly ApplicationDbContext context;
        private readonly IEmailSender emailSender;

        public BackgroundEmailSender(ApplicationDbContext context, IEmailSender emailSender)
        {
            this.context = context;
            this.emailSender = emailSender;
        }

        public async Task CheckUserGoalsAsync()
        {
            var users = await context.Users.Include(g => g.Goals).ToListAsync();

            foreach (var applicationUser in users)
            {
                string message = "";

                foreach (var goals in applicationUser.Goals.Where(g => g.IsComplete == false 
                                                                       && g.GoalEnd.HasValue 
                                                                       && g.GoalEnd.Value <= TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).AddMinutes(15)
                                                                       && g.GoalEnd.Value > TimeZoneInfo.ConvertTimeToUtc(DateTime.Now)))
                {
                    message +=
                        $"Задача «{goals.Name}» - срок выполнения «{goals.GoalEnd.Value.ToString("dd.MM.yyyy HH:mm")}»\r\n";
                }

                if(!string.IsNullOrEmpty(message))
                await emailSender.SendEmailToUserAsync(applicationUser.Email, "Список задач", message);
            }
        }
    }
}
