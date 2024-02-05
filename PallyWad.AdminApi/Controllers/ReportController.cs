using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public ReportController(ILogger<ReportController> logger, IHttpContextAccessor contextAccessor,
            ILoanRequestService loanRequestService, ILoanTransService loanTransService, IUserService userService, 
            IGlAccountService   glAccountService, IMembersAccountService membersAccountService,
            IGlAccountTransService glAccountTransService)
        {

            _logger = logger;
            _contextAccessor = contextAccessor;
            _loanRequestService = loanRequestService;
            _loanTransService = loanTransService;
            _userService = userService;
            _glAccountService = glAccountService;
            _membersAccountService = membersAccountService;
            _glAccountTransService = glAccountTransService;
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
        

        #endregion
    }
}
