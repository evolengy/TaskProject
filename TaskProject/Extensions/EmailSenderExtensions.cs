using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TaskProject.Services;

namespace TaskProject.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailToUserAsync(email, "Подтвердите вашу электронную почту",
                $"Пожалуйста подтвердите ваш аккаунт кликнув на эту ссылку: <a href='{HtmlEncoder.Default.Encode(link)}'>Подтвердить электронную почту</a>");
        }
    }
}
