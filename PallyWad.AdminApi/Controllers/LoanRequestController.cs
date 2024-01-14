using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;
using PallyWad.Services.Repository;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRequestController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<LoanRequestController> _logger;
        private readonly ILoanSetupService _loanSetupService;
        private readonly ILoanRequestService _loanRequestService;

        public LoanRequestController(IHttpContextAccessor contextAccessor, ILogger<LoanRequestController> logger,
            ILoanSetupService loanSetupService, ILoanRequestService loanRequestService)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _loanSetupService = loanSetupService;
            _loanRequestService = loanRequestService;

        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _loanRequestService.GetAllLoanRequests().OrderByDescending(u=>u.requestDate);
            return Ok( new Response { Status = "success", Message = result } );
        }

        [HttpGet("byReqId")]
        public IActionResult Get(int id)
        {
            var result = _loanRequestService.GetLoanRequest(id);
            return Ok(new Response { Status = "success", Message = result });
        }

        #endregion

        #region Post
        #endregion

        #region Put
        #endregion

        #region Helper

        private bool isEligible(string username)
        {
            return false;
        }
        #endregion
    }
}
