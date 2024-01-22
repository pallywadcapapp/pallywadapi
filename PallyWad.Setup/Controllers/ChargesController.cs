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
    public class ChargesController : ControllerBase
    {
        private readonly IChargesService _chargesService;
        private readonly ILogger<ChargesController> _logger;
        private readonly IMapper _mapper;
        public ChargesController(IChargesService chargesService, ILogger<ChargesController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _chargesService = chargesService;
        }


        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _chargesService.GetAllCharges();
            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var result = _chargesService.ListAllCharges();
            //var result = _mapper.Map<SetupDto>(Documents);
            return Ok(result);
        }

        #endregion

        #region Post
        [HttpPost(nameof(Charges))]
        public IActionResult Post(ChargesDto events)
        {

            try
            {
                if (events != null)
                {
                    var charges = _mapper.Map<Charges>(events);
                    _chargesService.AddCharge(charges);
                    return Ok(new { status = "success", message = $"Charges {charges.shortname} Created Successfully" });
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
                    var charges = _mapper.Map<Charges>(events);
                    _chargesService.UpdateCharge(charges);
                    return Ok(new { status = "success", message = $"Charges {charges.shortname} Updated Successfully" });
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
