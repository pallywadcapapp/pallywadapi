using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PallyWad.Auth.Models;
using PallyWad.Auth.Services;
using PallyWad.Domain;
using PallyWad.Domain.Entities;
using PallyWad.Services;
using System.Net.Mail;

namespace PallyWad.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly ISmtpConfigService _smtpConfigService;
        public ValuesController(IMailService mailService, IConfiguration configuration, ISmtpConfigService smtpConfigService)
        {
            _mailService = mailService;
            _configuration = configuration;
            _smtpConfigService = smtpConfigService;
        }
        [HttpPost]
        [Route("sendmail")]
        public async Task<IActionResult> SendMail(MailRequest request)
        {
            await _mailService.SendEmailAsync(request);
            return Ok();

        }

        [HttpPost]
        [Route("sendconfmail")]
        public async Task<IActionResult> SendConfMail(MailRequest request)
        {

            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _configuration.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }

            string filePath = Directory.GetCurrentDirectory() + "\\Templates\\verifyemail.html";
            string emailTemplateText = System.IO.File.ReadAllText(filePath);
            emailTemplateText = string.Format(emailTemplateText, "", "", DateTime.Today.Date.ToShortDateString());

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = emailTemplateText;
            emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

            var body = emailBodyBuilder.ToMessageBody();
            await _mailService.SendEmailAsync(request,mailConfig, company, body);
            return Ok();

        }
    }
}
