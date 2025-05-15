using Core.Api.Models.Options;
using Core.Models;
using Identity.Domain.Abstractions.Managers;
using MailKit.Net.Smtp;
using MassTransit.Monitoring.Performance;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.Encodings.Web;

namespace Identity.Presentation.Managers;

public sealed class EmailManager : IEmailManager
{
    private readonly UrlsOption _urlsOptions;

    public EmailManager(IOptions<UrlsOption> urlsOptions)
    {
        _urlsOptions = urlsOptions.Value;
    }

    public async Task SendArticleApprovedEmailAsync(string email, string username, string articleTitle)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Floyk documentation", "viktorpipidzule@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = "Одобрение статьи";
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"<p>Здравствуй, {username}. Твоя статья '{articleTitle}' была одобрена, теперь ты можешь ее опубликовать и писать новые статьи.</p><p>Спасибо за написанную статью!</p>"
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("viktorpipidzule@gmail.com", Secure.EmailCode);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }

    public async Task SendRegistrationCompleteEmailAsync(string email, string token, Guid userId)
    {
        var emailMessage = new MimeMessage();

        var callbackUrl = _urlsOptions.ClientUrl + $"/registration-confirm?token={token}&userId={userId}";

        emailMessage.From.Add(new MailboxAddress("Floyk documentation", "viktorpipidzule@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = "Подтверждение почты";
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"Добро пожаловать на наш сервис. Осталось подтвердить почту по <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>этой ссылке</a>.<p>Спасибо за регистраццию!</p>"
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("viktorpipidzule@gmail.com", Secure.EmailCode);
            var tmp = await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}