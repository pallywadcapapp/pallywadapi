using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PallyWad.Application;
using PallyWad.Domain.Entities;
using PallyWad.Domain;
using PallyWad.Services;

namespace PallyWad.Notifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JobsController> _logger;
        private readonly ILoanRepaymentService _loanRepaymentService;
        private readonly IMailService _mailService;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly ILoanRequestService _loanRequestService;
        private readonly ILoanTransService _loanTransService;
        public JobsController(ILoanRepaymentService loanRepaymentService, ILogger<JobsController> logger, IHttpContextAccessor contextAccessor,
            IConfiguration configuration, IMailService mailService, ISmtpConfigService smtpConfigService, ICompanyService companyService, 
            IUserService userService, ILoanRequestService loanRequestService, ILoanTransService loanTransService)
        {
            _loanRepaymentService = loanRepaymentService;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _mailService = mailService;
            _smtpConfigService = smtpConfigService;
            _companyService = companyService;
            _userService = userService;
            _loanRequestService = loanRequestService;
            _loanTransService = loanTransService;
        }

        [HttpGet("DueInterest")]
        public async Task<IActionResult> GetDueInterest() { 
            var date = DateTime.Now.Date;
            var prevDate = date.AddDays(-5);
            var nextDate = date.AddDays(5);
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.repaymentDate >= date && u.repaymentDate <= nextDate && u.interestbalance > 0);
            var company = _companyService.GetCompany();
            
            foreach (var item in result)
            {
                var loan = _loanRequestService.GetAllLoanRequests().Where(u=>u.loanId == item.loanrefnumber).FirstOrDefault();
                var user = _userService.GetUserByEmail(item.memberid);
                var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";
                var mailReq = new MailRequest()
                {
                    Body = "",
                    ToEmail = item.memberid,
                    Subject = "Loan Interest Payment Reminder"
                };
                await SendLoanMail(mailReq, item, loan, fullname, company, " \"loanInterestDue.html\"");
            }
            return Ok(result);
        }

        [HttpGet("ElapsedInterest")]
        public async Task<IActionResult> GetElapsedInterest()
        {
            var date = DateTime.Now.Date;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.repaymentDate < date && u.interestbalance > 0);
            var company = _companyService.GetCompany();

            foreach (var item in result)
            {
                var loan = _loanRequestService.GetAllLoanRequests().Where(u => u.loanId == item.loanrefnumber).FirstOrDefault();
                var user = _userService.GetUserByEmail(item.memberid);
                var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";
                var mailReq = new MailRequest()
                {
                    Body = "",
                    ToEmail = item.memberid,
                    Subject = "Late Interest Payment Notice"
                };
                await SendLoanMail(mailReq, item, loan, fullname, company, "loanInterestElapsed.html");
            }
            return Ok(result);
            //
        }

        //[HttpGet("CapitalRepayReminder")]
        //public async Task<IActionResult> GetCapitalRepayReminder()
        //{
        //    var date = DateTime.Now.Date;
        //    var maturityDate = date.AddDays(14);
        //    var result = _loanTransService.GetAllLoanTrans()
        //        .Where(u => u.re < date && u.interestbalance > 0);
        //    var company = _companyService.GetCompany();


        //}

        #region helper

        internal async Task SendLoanMail(MailRequest request, LoanRepayment lr, LoanRequest loan, string fullname, Company _company, string filename)
        {

            var mailkey = _configuration.GetValue<string>("AppSettings:AWSMail");
            var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == mailkey).FirstOrDefault();
            var company = _configuration.GetValue<string>("AppSettings:companyName");
            if (mailConfig == null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Check email configuration!" });
            }
            else
            {
                var urllink = "<a href=\"https://pallywad.com/tos\">Terms and Conditions</a>";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", filename);
                string emailTemplateText = System.IO.File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, fullname,
                    AppCurrFormatter.GetFormattedCurrency(loan.amount, 2, "HA-LATN-NG"), lr.interestRate,
                    AppCurrFormatter.GetFormattedCurrency(lr.interestbalance, 2, "HA-LATN-NG"),
                    AppCurrFormatter.GetFormattedCurrency(lr.loanamount, 2, "HA-LATN-NG"),
                    loan.requestDate.Date.AddMonths(loan.duration?? 0),lr.repaymentDate,
                    _company.address, _company.phoneno);
                //DateTime.Today.Date.ToShortDateString());

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;

                var body = emailBodyBuilder.ToMessageBody();
                await _mailService.SendEmailAsync(request, mailConfig, company, body);
            }

        }

        #endregion
    }
}
