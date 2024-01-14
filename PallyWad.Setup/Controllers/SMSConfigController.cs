using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain.Dto;
using PallyWad.Domain;
using PallyWad.Services;
using AutoMapper;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSConfigController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISMSConfigService _smConfigService;
        private readonly IMapper _mapper;
        public SMSConfigController(ISMSConfigService smsConfigService, IHttpContextAccessor httpContextAccessor,
            ILogger<SMSConfigController> logger, IMapper mapper)
        {
            _smConfigService = smsConfigService;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _smConfigService.GetAllSetupSMSConfig();
            return Ok(result);
        }

        [HttpGet(nameof(Document))]
        public IActionResult GetAll()
        {
            var result = _smConfigService.ListAllSetupSMSConfig();
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
                    var sMSconfig = _mapper.Map<SMSConfig>(config);
                    _smConfigService.AddSetupSMSConfig(sMSconfig);
                    return Ok(new Response { Status = "success", Message = sMSconfig });
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

        #region put

        [HttpPut]
        public IActionResult Put(ConfigDto config)
        {
            try
            {
                if (config != null)
                {
                    var sMSconfig = _mapper.Map<SMSConfig>(config);
                    _smConfigService.UpdateSetupSMSConfig(sMSconfig);
                    return Ok(sMSconfig);
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
