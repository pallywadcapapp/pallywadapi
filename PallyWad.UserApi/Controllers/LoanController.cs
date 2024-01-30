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
        public LoanController(ILoanSetupService loanSetupService)
        {
            _loanSetupService = loanSetupService;
        }
        #region
        [HttpGet]
        public IActionResult Get()
        {
            var result = _loanSetupService.GetAllLoanSetups()
                .OrderByDescending(u => u.Id);
            return Ok(result);
        }
        #endregion
    }
}
