using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
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
            var result = _loanSetupService.GetAllLoanSetups()
                .OrderByDescending(u=>u.Id);
            return Ok(result);
        }

        [HttpGet("byid")]
        public IActionResult GetLoanById(int id)
        {
            var result = _loanSetupService.GetLoanSetup(id);
            return Ok(result);
        }

        [HttpGet("bycode")]
        public IActionResult GetLoanById(string loancode)
        {
            var result = _loanSetupService.GetLoanSetup(loancode);
            return Ok(result);
        }
        #endregion

        #region post

        [HttpPost]
        public IActionResult Post(LoanSetupDto _loanSetup)
        {
            try
            {
                if (_loanSetup != null)
                {
                    var loanSetup = _mapper.Map<LoanSetup>(_loanSetup);
                    var ldoc = _mapper.Map<List<LoanDocument>>(_loanSetup.LoanDocumentRefId);
                    var lcoll = _mapper.Map<List<LoanCollateral>>(_loanSetup.LoanCollateralRefId);

                    loanSetup.LoanDocuments = ldoc;
                    loanSetup.LoanCollaterals = lcoll;
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
        [HttpPut("loandesc")]
        public IActionResult Put(LoanSetupDto _loanSetup, int id)
        {
            try
            {
                if (_loanSetup != null)
                {
                    var loanSetup = _mapper.Map<LoanSetup>(_loanSetup);
                    var ldoc = _mapper.Map<List<LoanDocument>>(_loanSetup.LoanDocumentRefId);
                    var lcoll = _mapper.Map<List<LoanCollateral>>(_loanSetup.LoanCollateralRefId);

                    loanSetup.LoanDocuments = ldoc;
                    loanSetup.LoanCollaterals = lcoll;

                    var loan = _loanSetupService.GetLoanSetup(id);
                    loan.loandesc = _loanSetup.loandesc;
                    _loanSetupService.UpdateLoanSetup(loan);
                    return Ok(new { status = "success", message = $"Loan  {loanSetup.loancode} Updated Successfully" });
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
    }
}
