using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRepaymentController : ControllerBase
    {
        private readonly ILoanRepaymentService _loanRepaymentService;
        public LoanRepaymentController(ILoanRepaymentService loanRepaymentService)
        {
            _loanRepaymentService = loanRepaymentService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u=>u.memberid == memberid)
                .Take(50)
            .OrderByDescending(u => u.transdate);
            return Ok(result);
        }

        [HttpGet("bydate")]
        public IActionResult GetByDate(DateTime startdate, DateTime enddate)
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.memberid == memberid && (u.transdate >= startdate || u.transdate <= enddate))
                .Take(50)
            .OrderByDescending(u => u.transdate);
            return Ok(result);
        }

        [HttpGet("byloanId")]
        public IActionResult Get(string loanId)
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.memberid == memberid && u.loanrefnumber == loanId)
                .Take(50)
            .OrderByDescending(u => u.transdate);
            return Ok(result);
        }
    }
}
