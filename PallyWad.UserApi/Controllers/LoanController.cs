using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        public readonly ILoanSetupService _loanSetupService;
        public readonly ILoanTransService _loanTransService;
        public LoanController(ILoanSetupService loanSetupService, ILoanTransService loanTransService)
        {
            _loanSetupService = loanSetupService;
            _loanTransService = loanTransService;
        }
        #region
        [HttpGet]
        public IActionResult Get()
        {
            var result = _loanSetupService.GetAllLoanSetups()
                .OrderByDescending(u => u.Id);
            return Ok(result);
        }


        [HttpGet]
        [Route("ActiveLoan")]
        public IActionResult GetActiveLoanHistory()
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var loanHistory = _loanTransService.ListLoanHistory(memberid)
                .Where(u => u.repay == 1)
            .OrderByDescending(u => u.transdate);
            return Ok(loanHistory);
        }


        [HttpGet]
        [Route("InactiveLoan")]
        public IActionResult GetInactiveLoanHistory()
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var loanHistory = _loanTransService.ListLoanHistory(memberid)
                .Where(u => u.repay == 2)
            .OrderByDescending(u => u.transdate);
            return Ok(loanHistory);
        }
        #endregion
    }
}
