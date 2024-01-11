using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmtpController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISmtpConfigService _smtpConfigService;
        private readonly IMapper _mapper;
        public SmtpController(ISmtpConfigService smtpConfigService, IHttpContextAccessor httpContextAccessor,
            ILogger<SmtpController> logger, IMapper mapper)
        {
            _smtpConfigService = smtpConfigService;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _smtpConfigService.GetAllSetupSmtpConfig();
            return Ok(result);
        }

        [HttpGet(nameof(Document))]
        public IActionResult GetAll()
        {
            var result = _smtpConfigService.ListAllSetupSmtpConfig();
            return Ok(result);
        }
        #endregion

        #region Post

        [HttpPost]
        public IActionResult Post(ConfigDto config)
        {
            try
            {
                if (config != null)
                {
                    var smtpconfig = _mapper.Map<SmtpConfig>(config);
                    _smtpConfigService.AddSetupSmtpConfig(smtpconfig);
                    return Ok(new Response({ Status = "success", Message = smtpconfig });
                }
                else
                {
                    return BadRequest("config is empty");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region put

        [HttpPut]
        public IActionResult Put(ConfigDto config)
        {
            try
            {
                if (config != null)
                {
                    var smtpconfig = _mapper.Map<SmtpConfig>(config);
                    _smtpConfigService.UpdateSetupSmtpConfig(smtpconfig);
                    return Ok(smtpconfig);
                }
                else
                {
                    return BadRequest("config is empty");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion
    }
}
