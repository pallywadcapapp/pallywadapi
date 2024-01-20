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
    public class InterestController : ControllerBase
    {
        private readonly IInterestService _interestService;
        private readonly ILogger<InterestController> _logger;
        private readonly IMapper _mapper;
        public InterestController(IInterestService interestService, ILogger<InterestController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _interestService = interestService;
        }


        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _interestService.GetAllInterests();
            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var result = _interestService.ListAllInterests();
            //var result = _mapper.Map<SetupDto>(Documents);
            return Ok(result);
        }

        #endregion

        #region Post
        [HttpPost(nameof(Interest))]
        public IActionResult Post(InterestDto events)
        {

            try
            {
                if (events != null)
                {
                    var interest = _mapper.Map<Interest>(events);
                    _interestService.Addinterest(interest);
                    return Ok(new { status = "success", message = $"Interest {interest.shortname} Created Successfully" });
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

        #region Put
        [HttpPut(nameof(Document))]
        public IActionResult Put(InterestDto events)
        {
            try
            {
                if (events != null)
                {
                    var interest = _mapper.Map<Interest>(events);
                    _interestService.Updateinterest(interest);
                    return Ok(new { status = "success", message = $"Interest {interest.shortname} Updated Successfully" });
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
