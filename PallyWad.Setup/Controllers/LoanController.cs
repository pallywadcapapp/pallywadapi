using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanSetupService _loanSetupService;
        private readonly IMapper _mapper;

        public LoanController(ILogger<LoanController> logger, ILoanSetupService loanSetupService, IMapper mapper)
        {
            _logger = logger;
            _loanSetupService = loanSetupService;
            _mapper = mapper;
        }

        #region get

        [HttpGet]
        public IActionResult Get()
        {
            var result = _loanSetupService.GetAllLoanSetups();
            return Ok(result);
        }

        [HttpGet("byid")]
        public IActionResult GetLoanById(int id)
        {
            var result = _loanSetupService.GetLoanSetup(id);
            return Ok(result);
        }

        [HttpGet("bycode")]
        public IActionResult GetLoanById(string loanode)
        {
            var result = _loanSetupService.GetLoanSetup(loanode);
            return Ok(result);
        }
        #endregion

        #region post

        [HttpPost]
        public IActionResult Post(LoanSetup loanSetup)
        {
            try
            {
                if (loanSetup != null)
                {
                    //var collateral = _mapper.Map<Collateral>(events);
                    _loanSetupService.AddLoanSetup(loanSetup);
                    return Ok(new { status = "success", message = $"Collateral {loanSetup.loancode} Created Successfully" });
                }
                else
                {
                    return BadRequest(new { status = "error", message = "parameter is empty" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { status = "error", message = ex.Message });
            }
        }
        #endregion

        #region put
        #endregion
    }
}
