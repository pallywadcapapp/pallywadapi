using Amazon;
using AutoMapper;
//using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Asn1.X509;
using PallyWad.Application;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Domain.Entities;
using PallyWad.Services;
using System.Security.Claims;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRequestController : ControllerBase
    {
        private readonly ILogger<LoanRequestController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILoanRequestService _loanRequestService;
        private readonly ILoanSetupService _loanSetupService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;

        public LoanRequestController(ILogger<LoanRequestController> logger, IHttpContextAccessor contextAccessor,
            ILoanRequestService loanRequestService, IMapper mapper, ILoanSetupService loanSetupService, IConfiguration configuration,
            ISmtpConfigService smtpConfigService,IMailService mailService, IUserService userService)
        {

            _logger = logger;
            _contextAccessor = contextAccessor;
            _loanRequestService = loanRequestService;
            _mapper = mapper;
            _loanSetupService = loanSetupService;
            _configuration = configuration;
            _smtpConfigService = smtpConfigService;
            _mailService = mailService;
            _userService = userService;
        }

        #region Get
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;
            var result = _loanRequestService.GetLoanRequests(memberId).OrderByDescending(u => u.requestDate);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("loandetail")]
        public IActionResult GetLoanDetail(string loadId)
        {
            var _result = _loanRequestService.GetAllLoanRequests();
            var result = _result.Where(u => u.loanId == loadId).FirstOrDefault();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("iseligible")]
        public IActionResult GetLoanRequestEligibility()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;
            var user = _userService.GetUser(memberId);
            if(user == null)
            {
                return Ok(false);
            }
            else
            {
                if (user.PhoneNumberConfirmed == false) { }
            }
            return Ok(user);
        }

        #endregion

        #region Post
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(LoanRequestDto _loanRequest)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            string fullname = "";

            var user = _userService.GetUser(memberId);

            //var identity = princ.Identity as ClaimsIdentity;
            //if (identity != null)
            //{
            //    IEnumerable<Claim> claims = identity.Claims;
            //    // or
            //    var lastname = identity.FindFirst("lastname").Value;
            //    var firstname = identity.FindFirst("firstname").Value;
            //    var othernames = identity.FindFirst("othernames").Value;

            //    fullname = $"{lastname}, {firstname} {othernames}";

            //}

            if(user == null )
            {
                return BadRequest(new Response { Status = "error", Message = "Invalid Token" });
            }

            fullname = $"{user.lastname}, {user.firstname} {user.othernames}";


            var loanRequest = _mapper.Map<LoanRequest>(_loanRequest);
            var ltype = _loanSetupService.GetLoanSetup(loanRequest.loancode);
            if(ltype != null) { 
            loanRequest.loanId = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            loanRequest.memberId = memberId;
            loanRequest.processState = "Pending";
            loanRequest.status = "Pending";
            loanRequest.requestDate = DateTime.Now;
            loanRequest.category = ltype.category;
            loanRequest.postedBy = memberId;
                loanRequest.collateralId = _loanRequest.collateralRefId;

            _loanRequestService.AddLoanRequest(loanRequest);
            var mailReq = new MailRequest()
            {
                Body = "",
                ToEmail = memberId,
                Subject = "Loan Request"
            };
            await SendConfMail(mailReq, loanRequest, fullname);
            return Ok(new Response { Status = "success", Message = loanRequest });
            }
            else
            {
                return BadRequest(new Response()
                {
                    Status = "error",
                    Message = "Invalid loan type "
                });
            }
        }
        #endregion

        #region Helper

        internal async Task SendConfMail(MailRequest request, LoanRequest lr, string fullname)
        {

            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _configuration.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            else { 

            string filePath = Directory.GetCurrentDirectory() + "\\Templates\\loanrequest.html";
            string emailTemplateText = System.IO.File.ReadAllText(filePath);
            emailTemplateText = string.Format(emailTemplateText, fullname,
                AppCurrFormatter.GetFormattedCurrency(lr.amount, 2, "HA-LATN-NG"),
                DateTime.Today.Date.ToShortDateString());

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = emailTemplateText;
            emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

            var body = emailBodyBuilder.ToMessageBody();
            await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }
        #endregion
    }
}
