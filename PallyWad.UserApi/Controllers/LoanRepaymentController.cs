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
        private readonly ILoanTransService _loanTransService;
        public LoanRepaymentController(ILoanRepaymentService loanRepaymentService, ILoanTransService loanTransService)
        {
            _loanRepaymentService = loanRepaymentService;
            _loanTransService = loanTransService;
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

        [HttpGet("details")]
        public IActionResult GetDetails(int id)
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.Id == id)
                .FirstOrDefault();
            return Ok(result);
        }

        [HttpGet("LoanPosition")]
        public IActionResult GetLoanPosition(string loanId)
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var result = _loanRepaymentService.GetAllLoanRepayments()
               .Where(u => u.memberid == memberid && u.loanrefnumber == loanId)
           .OrderByDescending(u => u.Id)
           .FirstOrDefault();
            return Ok(result);
        }

        [HttpGet("LoanOverheads")]
        public IActionResult GetLoanOverheads()
        {
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InNlZ3h5MjcwOEBob3RtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJzZWd4eTI3MDhAaG90bWFpbC5jb20iLCJsYXN0bmFtZSI6Ik9kdXdvbGUiLCJmaXJzdG5hbWUiOiJPbHV3YXNlZ3VuIiwib3RoZXJuYW1lcyI6Ik9sdXdhZ2JlbmdhIiwiYWRkcmVzcyI6IjE4IFVuaXR5IFN0cmVldCBvZmYgb2xvbXUgd2F5Iiwic2V4IjoiTWFsZSIsImRvYiI6IjEvMjQvMTk4NCA5OjQxOjQxIEFNIiwianRpIjoiOWM0YmVkZTgtYTdmNC00MzEzLTk2ZjktYjFiOGUzNDVkOTUyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkFkbWluIiwiUHJvdmlkZXIiXSwiZXhwIjoxNzA5MzA0NDQ0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.6RLdBxLiVOVa7GtAF9Tg80-1iU-XG9xcnyHoLCgjgj4
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var loans = _loanTransService.GetAllLoanTrans()
                .Where(u=>u.memberid == memberid && u.repay == 1);

            double sums = 0;

            foreach(var l in loans)
            {
                var repay = _loanRepaymentService.GetAllLoanRepayments()
                    .Where(u=>u.loanrefnumber == l.loanrefnumber)
                    .OrderByDescending(u=>u.Id) .FirstOrDefault();

                if(repay != null)
                sums += repay.loanamount;
            }
           
            return Ok(sums);
        }

        [HttpGet("totalPayment")]
        public IActionResult GetTotalPayment(string loanId)
        {
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.memberid == memberid && u.loanrefnumber == loanId)
                .Sum(u=>u.loanamount);
            return Ok(result);
        }
    }
}
