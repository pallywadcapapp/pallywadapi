using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using PallyWad.Domain;
using PallyWad.Domain.Entities;
using PallyWad.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    [TransientRegistration]
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        //private readonly SmtpConfig _smtpConfig;
        public MailService(IOptions<MailSettings> mailSettings)
            //, IOptions<SmtpConfig> smtpConfig)
        {
            _mailSettings = mailSettings.Value;
            //_smtpConfig = smtpConfig.Value;
        }

        
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailRequest.Body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(MailRequest mailRequest, SmtpConfig mailConfig, string displayname, MimeEntity body)
        {
           
                var emailMessage = new MimeMessage();


                emailMessage.From.Add(new MailboxAddress(displayname, mailConfig.mailfrom));
                emailMessage.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                emailMessage.Subject = mailRequest.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = mailRequest.Body
                };
            
            emailMessage.Body = body; //builder.ToMessageBody();

                using var smtp = new SmtpClient();

                smtp.Connect(mailConfig.smtp, mailConfig.port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailConfig.username, mailConfig.password);
                await smtp.SendAsync(emailMessage);
                smtp.Disconnect(true);
            }
    }


    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendEmailAsync(MailRequest mailRequest, SmtpConfig mailConfig, string displayname, MimeEntity body);
    }

    
}
