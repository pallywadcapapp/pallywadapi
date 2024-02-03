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
        private readonly IAppUploadedFilesService _appUploadedFilesService;
        public CollateralController(IUserCollateralService userCollateralService, ICollateralService collateralService,
            IAppUploadedFilesService appUploadedFilesService)
        {
            _userCollateralService = userCollateralService;
            _collateralService = collateralService;
            _appUploadedFilesService = appUploadedFilesService;
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


        [HttpGet, Route("Files")]
        public async Task<IActionResult> GetFileDownload(string id)
        {
            if (id == null)
                return BadRequest("filename not present");
            var coll = _appUploadedFilesService.ListAllSetupAppUploadedFiles()
                .Where(u => u.comment == id && u.transOwner == "collateral");

            if (coll == null)
                return NotFound("collateral file not found");


            return Ok(coll);
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
