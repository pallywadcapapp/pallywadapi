using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollateralController : ControllerBase
    {
        private readonly IUserCollateralService _userCollateralService;
        private readonly ICollateralService _collateralService;
        public CollateralController(IUserCollateralService userCollateralService, ICollateralService collateralService)
        {
            _userCollateralService = userCollateralService;
            _collateralService = collateralService;

        }

        #region Get

        [HttpGet()]
        public IActionResult Get()
        {
            var result = _userCollateralService.ListAllUserCollateral();
            return Ok(result);
        }

        [HttpGet("Unapproved")]
        public IActionResult GetUnapproved()
        {
            var result = _userCollateralService.ListAllUserCollateral().Where(u => u.status == false);
            return Ok(result);
        }

        [HttpGet("userCollById")]
        public IActionResult GetUserCollById(int Id)
        {
            var result = _userCollateralService.ListAllUserCollateral().Where(u => u.Id == Id).FirstOrDefault();
            return Ok(result);
        }
        #endregion

        #region Post
        #endregion

        #region Put
        #endregion

        #region Helper
        #endregion
    }
}
