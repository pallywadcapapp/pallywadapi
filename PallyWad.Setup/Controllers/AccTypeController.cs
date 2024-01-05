using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccTypeController : ControllerBase
    {

        private readonly ILogger<AccTypeController> _logger;
        private readonly IAccTypeService _AccTypeService;
        private readonly IMapper _mapper;

        public AccTypeController(ILogger<AccTypeController> logger, IAccTypeService AccTypeService, IMapper mapper)
        {
            _logger = logger;
            _AccTypeService = AccTypeService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var result = _AccTypeService.GetAllAccType();
            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var result = _AccTypeService.ListAllAccType();
            //var result = _mapper.Map<SetupDto>(AccTypes);
            return Ok(result);
        }

        [HttpPost(nameof(AccType))]
        public IActionResult Post(SetupDto events)
        {

            try
            {
                if (events != null)
                {
                    var AccType = _mapper.Map<AccType>(events);
                    _AccTypeService.AddAccType(AccType);
                    return Ok(new { status = "success", message = $"Account Type {AccType.name} Created Successfully" });
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

        [HttpPut(nameof(AccType))]
        public IActionResult Put(SetupDto events)
        {
            try
            {
                if (events != null)
                {
                    var AccType = _mapper.Map<AccType>(events);
                    _AccTypeService.UpdateAccType(AccType);
                    return Ok(new { status = "success", message = $"Account Type {AccType.name} Updated Successfully" });
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
    }
}
