using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskProject.Models;
using TaskProject.Services.EmailSender;

namespace TaskProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            // Добавляем первоначальные значения в базу данных 
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var emailSender = services.GetRequiredService<IEmailSender>();

                    context.Initialize();

                    // Отправляем пользователям список задач, которые скоро закончатся
                    //RecurringJob.AddOrUpdate(
                    //    () => new BackgroundEmailSender(context,emailSender).CheckUserGoalsAsync(),
	                // Cron.MinuteInterval(15));
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ошибка при доступе к базе данных.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //Устанавливаем наименьший уровень логов
                //.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .UseIISIntegration()
                .Build();
    }
}
