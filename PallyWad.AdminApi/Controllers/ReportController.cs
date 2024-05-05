using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PallyWad.Domain.Entities;
using PallyWad.Services;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILoanRequestService _loanRequestService;
        private readonly ILoanTransService _loanTransService;
        private readonly IUserService _userService;
        private readonly IGlAccountService _glAccountService;
        private readonly IMembersAccountService _membersAccountService;
        private readonly IGlAccountTransService _glAccountTransService;
        private readonly ILoanRepaymentService _loanRepaymentService;
        public ReportController(ILogger<ReportController> logger, IHttpContextAccessor contextAccessor,
            ILoanRequestService loanRequestService, ILoanTransService loanTransService, IUserService userService, 
            IGlAccountService   glAccountService, IMembersAccountService membersAccountService,
            IGlAccountTransService glAccountTransService, ILoanRepaymentService loanRepaymentService)
        {

            _logger = logger;
            _contextAccessor = contextAccessor;
            _loanRequestService = loanRequestService;
            _loanTransService = loanTransService;
            _userService = userService;
            _glAccountService = glAccountService;
            _membersAccountService = membersAccountService;
            _glAccountTransService = glAccountTransService;
            _loanRepaymentService = loanRepaymentService;
        }

        #region Get
        [HttpGet("LoanRequest")]
        public IActionResult GetLoanRequest()
        {
            var result = _loanRequestService.GetAllPendingLoanRequests().Count();
            return Ok(result);
        }

        [HttpGet("activeloans")]
        public IActionResult GetActiveloans()
        {
            var result = _loanTransService.GetAllLoanTrans().Count();
            return Ok(result);
        }

        [HttpGet("users")]
        public IActionResult GeTotalUsers()
        {
            var result = _userService.GetAllUsers().Count();
            return Ok(result);
        }

        [HttpGet("loanSummation")]
        public IActionResult GetLoanSummation()
        {
            var memberAcc = _membersAccountService.ListAllMembersAccounts()
                .Select(u=>u.accountno);
            var result = _glAccountTransService.GetAllGlAccounts()
                .Where(u => memberAcc.Contains(u.accountno))
                .Select(u=>(u.creditamt - u.debitamt)).Sum();
                //.Count();
            return Ok(result);
        }

        [HttpGet("DueInterest")]
        public async Task<IActionResult> GetDueInterest()
        {
            var date = DateTime.Now.Date;
            var prevDate = date.AddDays(-5);
            var nextDate = date.AddDays(5);
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.repaymentDate >= date && u.repaymentDate <= nextDate && u.interestbalance > 0);
            //var company = _companyService.GetCompany();

            //foreach (var item in result)
            //{
            //    var loan = _loanRequestService.GetAllLoanRequests().Where(u => u.loanId == item.loanrefnumber).FirstOrDefault();
            //    var user = _userService.GetUserByEmail(item.memberid);
            //    var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";
            //    var mailReq = new MailRequest()
            //    {
            //        Body = "",
            //        ToEmail = item.memberid,
            //        Subject = "Loan Interest Payment Reminder"
            //    };
            //    await SendLoanMail(mailReq, item, loan, fullname, company, " \"loanInterestDue.html\"");
            //}
            return Ok(result);
        }

        [HttpGet("ElapsedInterest")]
        public async Task<IActionResult> GetElapsedInterest()
        {
            var date = DateTime.Now.Date;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.repaymentDate < date && u.interestbalance > 0);
            //var company = _companyService.GetCompany();

            //foreach (var item in result)
            //{
            //    var loan = _loanRequestService.GetAllLoanRequests().Where(u => u.loanId == item.loanrefnumber).FirstOrDefault();
            //    var user = _userService.GetUserByEmail(item.memberid);
            //    var fullname = $"{user.lastname}, {user.firstname} {user.othernames}";
            //    var mailReq = new MailRequest()
            //    {
            //        Body = "",
            //        ToEmail = item.memberid,
            //        Subject = "Late Interest Payment Notice"
            //    };
            //    await SendLoanMail(mailReq, item, loan, fullname, company, "loanInterestElapsed.html");
            //}
            return Ok(result);
            //
        }


        #endregion
    }
}
