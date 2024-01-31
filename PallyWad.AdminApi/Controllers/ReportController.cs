using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ReportController(ILogger<ReportController> logger, IHttpContextAccessor contextAccessor,
            ILoanRequestService loanRequestService)
        {

            _logger = logger;
            _contextAccessor = contextAccessor;
            _loanRequestService = loanRequestService;
        }

        #region Get
        [HttpGet("LoanRequest")]
        public IActionResult GetLoanRequest()
        {
            var result = _loanRequestService.GetAllPendingLoanRequests().Count();
            return Ok(result);
        }
        #endregion
    }
}
