using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace PallyWad.Accounting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsOfAccController : ControllerBase
    {
        private readonly ILogger<ChartsOfAccController> _logger;
        private readonly IGlAccountBaseService _glAccountBaseService;
        private readonly IGlAccountTier2Service _glAccountTier2Service;
        private readonly IGlAccountTier3Service _glAccountTier3Service;
        private readonly IGlAccountService _glAccountService;
        private readonly IAccTypeService _accTypeService;
        private readonly IAccountBaseService _accountBaseService;

        public ChartsOfAccController(ILogger<ChartsOfAccController> logger, IGlAccountBaseService glAccountBaseService,
            IGlAccountTier2Service glAccountTier2Service, IAccTypeService accTypeService, IGlAccountTier3Service glAccountTier3Service,
            IGlAccountService glAccountService, IAccountBaseService accountBaseService)
        {
            _logger = logger;
            _glAccountBaseService = glAccountBaseService;
            _glAccountTier2Service = glAccountTier2Service;
            _accTypeService = accTypeService;
            _glAccountTier3Service = glAccountTier3Service;
            _glAccountService = glAccountService;
            _accountBaseService = accountBaseService;
        }

        #region Get

        [HttpGet("trz")]
        public IActionResult trz(int i)
        {
            var result = String.Format("{0,6:00}", i);
            return Ok(result);
        }

        [HttpGet]
        [Route("acctype")]
        public IActionResult GetAccTyleList()
        {
            var result = _accTypeService.GetAllAccType();
            return Ok(result);
        }
        [HttpGet]
        [Route("ChartAcc")]
        public IActionResult GetChartAcc()
        {
            var result = _glAccountService.GetAllGlAccounts();
            return Ok(result);
        }

        [HttpGet]
        [Route("ChartAcc3")]
        public IActionResult GetChartAcc3()
        {
            var result = _glAccountTier3Service.GetAllGlAccounts();
            return Ok(result);
        }

        [HttpGet]
        [Route("ChartAcc2")]
        public IActionResult GetChartAcc2()
        {
            var result = _glAccountTier2Service.GetAllGlAccounts();
            return Ok(result);
        }

        [HttpGet]
        [Route("ChartAcc1")]
        public IActionResult GetChartAcc1()
        {
            var result = _glAccountBaseService.GetAllGlAccounts();
            return Ok(result);
        }

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

        [HttpGet]
        [Route("Tier2ChartAcc")]
        public IActionResult GetTier2ChartAcc(string id)
        {
            var result = _glAccountTier2Service.GetAllGlAccounts().Where(u => u.glaccta == id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Tier3ChartAcc")]
        public IActionResult Tier3GetChartAcc(string id)
        {
            var result = _glAccountTier3Service.GetAllGlAccounts().Where(u => (u.glaccta + u.glacctb) == id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Tier4ChartAcc")]
        public IActionResult Tier4GetChartAcc(string id)
        {
            var result = _glAccountService.GetAllGlAccounts().Where(u => (u.glaccta + u.glacctb + u.glacctc) == id);
            return Ok(result);
        }

        [HttpGet]
        [Route("AccListBS")]
        public IActionResult ListAccBaseDto()
        {
            var temp = _accountBaseService.GetAccountBaseName();
            List<BSTValeuDto> list = new List<BSTValeuDto>();
            foreach (var p in temp)
            {
                BSTValeuDto bSTValeuDto = new BSTValeuDto();
                bSTValeuDto.text = p;
                bSTValeuDto.value = p;
                list.Add(bSTValeuDto);
            }
            return Ok(list);

        }

        [HttpGet]
        [Route("InternalControl")]
        public IActionResult GetInternalControl()
        {
            var result = _glAccountService.GetAllGlAccounts().Where(x => x.internalind == true);
            return Ok(result);
        }

        #endregion

        #region Post

        [HttpPost]
        [Route("ChartBase")]
        public IActionResult PostChartAccBase(string name, string acctype)
        {
            try
            {
                if (name != null)
                {
                    var id = 1;
                    var top = _glAccountBaseService.GetAllGlAccounts().OrderByDescending(x=>x.Id).FirstOrDefault();
                    if(top != null)
                    {
                        int accno = int.Parse(top.glaccta);
                        id = accno + 1;
                    }
                    else
                    {

                    }
                    var accbase = new GLAccountBase() {
                        acctlevel = 1,
                        glaccta = String.Format("{0,2:00}", id), //"0" + id.ToString(), 
                        accountno = String.Format("{0,2:00}", id), //"0" + id.ToString(), 
                        shortdesc = name, 
                        fulldesc = name, 
                        accttype = acctype
                    };
                    _glAccountBaseService.AddGlAccount(accbase);
                    return Ok(new { status = "success", message = $"GL account Base {accbase.shortdesc} Created Successfully" });
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
        public IActionResult PostTier2ChartAcc(string name, string gla)
        {
            try
            {
                if (name != null)
                {
                    var id = 1;
                    var top = _glAccountTier2Service.GetAllGlAccounts()
                        .Where(u=>u.glaccta == gla)
                        .OrderByDescending(x => x.Id).FirstOrDefault();
                    if (top != null)
                    {
                        //id = top.Id + 1;
                        int accno = int.Parse(top.glacctb);
                        id = accno + 1;
                    }
                    else
                    {

                    }
                    var gl = _glAccountBaseService.GetGlAccountByAcc(gla);
                    var gLAccountB = new GLAccountB()
                    {
                        glaccta = gl.accountno,
                        acctlevel = 2,
                        glacctb = String.Format("{0,2:00}", id),//"0" + id.ToString(),
                        accountno = gl.accountno + String.Format("{0,2:00}", id),//"0" + id.ToString(),
                        shortdesc = name,
                        fulldesc = name,
                        accttype = gl.shortdesc
                    };
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



        [HttpPost]
        [Route("glc")]
        public IActionResult PostGLC(GLAccountC gl)
        {
            var accname = gl.shortdesc.ToUpper();
            var id = 1;
            var top = _glAccountTier3Service
                .GetAllGlAccounts()
                .Where(u => u.glacctb == gl.glacctb)
                        .OrderByDescending(x => x.Id).FirstOrDefault();

            if (top != null)
            {
                //id = top.Id + 1;
                int iaccno = int.Parse(top.glacctc);
                id = iaccno + 1;
            }
            else
            {

            }
            var interim = _glAccountTier2Service.GetGlAccount($"{gl.glaccta}{gl.glacctb}");
            gl.shortdesc = accname;
            gl.fulldesc = accname;
            var accno = gl.glaccta + gl.glacctb + gl.glacctc;
            gl.glacctc = String.Format("{0,2:00}", id);//"0" + id.ToString(),
            gl.accountno = interim.accountno + String.Format("{0,2:00}", id);//"0" + id.ToString()
            _glAccountTier3Service.AddGlAccount(gl);
            return Ok(gl);
        }



        [HttpPost]
        [Route("gld")]
        public IActionResult PostGLD(GLAccount gl)
        {
            var accname = gl.shortdesc.ToUpper();
            /*var chtacc = _glAccountService.GetAllGlAccountByDesc();
            int ch = 0;
            if (chtacc != null)
            {
                ch = int.Parse(chtacc);
            }
            var accnoId = ch + 1;*/
            var id = 1;
            var top = _glAccountService
                .GetAllGlAccounts()
                .Where(u => u.glacctc == gl.glacctc)
                        .OrderByDescending(x => x.Id).FirstOrDefault();

            if (top != null)
            {
                //id = top.Id + 1;
                int iaccno = int.Parse(top.glacctd);
                id = iaccno + 1;
            }
            else
            {

            }
            var interim = _glAccountTier3Service.GetGlAccount($"{gl.glaccta}{gl.glacctb}{gl.glacctc}");
            //gl.glacctd = String.Format("{0,2:00}", accnoId);
            gl.shortdesc = accname;
            gl.fulldesc = accname;
            /*var accno = gl.glaccta + gl.glacctb + gl.glacctc + accnoId;
            gl.accountno = accno;
            var accno = gl.glaccta + gl.glacctb + gl.glacctc + gl.glacctd;*/

            gl.glacctd = String.Format("{0,2:00}", id);//"0" + id.ToString(),
            gl.accountno = interim.accountno + String.Format("{0,2:00}", id);//"0" + id.ToString()
            _glAccountService.AddGlAccount(gl);
            return Ok(gl);
        }
        #endregion
    }
}
