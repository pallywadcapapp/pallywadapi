using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Auth.Services;
using PallyWad.Domain.Entities;
using PallyWad.Services;

namespace PallyWad.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMailService _mailService;
        public ValuesController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost]
        [Route("sendmail")]
        public async Task<IActionResult> SendMail(MailRequest request)
        {
            await _mailService.SendEmailAsync(request);
            return Ok();

        }
    }
}
