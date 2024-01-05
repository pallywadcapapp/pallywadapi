using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.Setup.Controllers;

[ApiController]
[Route("[controller]")]
public class CollateralController : ControllerBase
{

    private readonly ILogger<CollateralController> _logger;
    private readonly ICollateralService _collateralService;
    private readonly IMapper _mapper;

    public CollateralController(ILogger<CollateralController> logger, ICollateralService collateralService, IMapper mapper)
    {
        _logger = logger;
        _collateralService = collateralService;
        _mapper = mapper;
    }
    [HttpGet]
    public IActionResult Get()
    {
        var result = _collateralService.GetAllCollateral();
        return Ok(result);
    }

    [HttpGet]
    [Route("all")]
    public IActionResult GetAll() {
        var result = _collateralService.ListAllCollateral();
        //var result = _mapper.Map<SetupDto>(collaterals);
        return Ok(result);
    }

    [HttpPost(nameof(Collateral))]
    public IActionResult Post(SetupDto events)
    {

        try
        {
            if (events != null)
            {
                var collateral = _mapper.Map<Collateral>(events);
                _collateralService.AddCollateral(collateral);
                return Ok(new { status = "success", message = $"Collateral {collateral.name} Created Successfully" });
            }
            else
            {
                return BadRequest(new {status ="error", message = "parameter is empty"});
            }
        }
        catch (Exception ex)
        {

            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpPut(nameof(Collateral))]
    public IActionResult Put(SetupDto events) {
        try
        {
            if (events != null)
            {
                var collateral = _mapper.Map<Collateral>(events);
                _collateralService.UpdateCollateral(collateral);
                return Ok(new { status = "success", message = $"Collateral {collateral.name} Updated Successfully" });
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
