using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanTasksController : ControllerBase
    {
        private readonly ILoanRepaymentService _loanRepaymentService;
        private readonly IRecurringJobManager _recurringJobManager;
        public LoanTasksController(ILoanRepaymentService loanRepaymentService, IRecurringJobManager recurringJobManager)
        {
            _loanRepaymentService = loanRepaymentService;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        public IActionResult Get() { 
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u=>u.repaymentDate.Date == DateTime.Now.Date);
            return Ok(result);
        }

        [HttpGet("InterestScheduler")]
        public IActionResult GetInterestScheduler() {
            _recurringJobManager.AddOrUpdate("InterestScheduler", () => InterestScheduler(), Cron.Daily);
            return Ok();
        }

        #region Helper

        [ApiExplorerSettings(IgnoreApi = true)]
        public void InterestScheduler()
        {
            var result = _loanRepaymentService.GetAllLoanRepayments()
                .Where(u => u.repaymentDate.Date == DateTime.Now.Date);

            foreach (var p in result)
            {
                var tempInt = p.interestamt;
                var outstanding = p.interestbalance;

                double newInterest = tempInt + outstanding;
                var nextPayDay = DateTime.Now.Date.AddMonths(1);

                p.interestbalance = newInterest;
                
                _loanRepaymentService.UpdateLoanRepayment(p);
            }
        }
        #endregion
    }
}
