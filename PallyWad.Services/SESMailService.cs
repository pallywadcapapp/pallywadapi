using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Options;
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
    public class SESMailService : ISESMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IAmazonSimpleEmailService _mailService;
        public SESMailService(IOptions<MailSettings> mailSettings,
            IAmazonSimpleEmailService mailService)
        {
            _mailSettings = mailSettings.Value;
            _mailService = mailService;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var mailBody = new Body(new Content(mailRequest.Body));
            var message = new Message(new Content(mailRequest.Subject), mailBody);
            var destination = new Destination(new List<string> { mailRequest.ToEmail! });
            var request = new SendEmailRequest(_mailSettings.Mail, destination, message);
            await _mailService.SendEmailAsync(request);
        }
    }


    public interface ISESMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
