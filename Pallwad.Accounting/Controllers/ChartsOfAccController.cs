using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace Pallwad.Accounting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsOfAccController : ControllerBase
    {
        private readonly ILogger<ChartsOfAccController> _logger;
        private readonly IGlAccountBaseService _glAccountBaseService;
        private readonly IGlAccountTier2Service _glAccountTier2Service;

        public ChartsOfAccController(ILogger<ChartsOfAccController> logger, IGlAccountBaseService glAccountBaseService,
            IGlAccountTier2Service glAccountTier2Service)
        {
            _logger = logger;
            _glAccountBaseService = glAccountBaseService;
            _glAccountTier2Service = glAccountTier2Service;

        }

        #region Get

        [HttpGet]
        [Route("ChartBase")]
        public IActionResult GetChartAccBase()
        {
            var result = _glAccountBaseService.GetAllGlAccounts();
            return Ok(result);
        }

        [HttpGet]
        [Route("Tier2Chart")]
        public IActionResult GetTier2ChartAcc()
        {
            var result = _glAccountTier2Service.GetAllGlAccounts();
            return Ok(result);
        }

        #endregion

        #region Post

        [HttpPost]
        [Route("ChartBase")]
        public IActionResult PostChartAccBase(GLAccountBase gLAccountBase)
        {
            try
            {
                if (gLAccountBase != null)
                {
                    //var collateral = _mapper.Map<Collateral>(events);
                    _glAccountBaseService.AddGlAccount(gLAccountBase);
                    return Ok(new { status = "success", message = $"GL account Base {gLAccountBase.shortdesc} Created Successfully" });
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

        [HttpPost]
        [Route("Tier2Chart")]
        public IActionResult PostTier2ChartAcc(GLAccountB gLAccountB)
        {
            try
            {
                if (gLAccountB != null)
                {
                    //var collateral = _mapper.Map<Collateral>(events);
                    _glAccountTier2Service.AddGlAccount(gLAccountB);
                    return Ok(new { status = "success", message = $"Tier 2 GL account {gLAccountB.shortdesc} Created Successfully" });
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
