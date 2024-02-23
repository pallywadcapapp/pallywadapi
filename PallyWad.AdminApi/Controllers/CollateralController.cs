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

        [HttpGet("loadedCollateral")]
        public IActionResult GetUAdminloadedCollateral(string loanId)
        {
            var result = _appUploadedFilesService.ListAllSetupAppUploadedFiles().Where(u => u.comment == loanId);
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

        [HttpGet, Route("FileUploads")]
        public async Task<IActionResult> FileDownload(string filepath)
        {
            try
            {
                if (filepath == null)
                    return BadRequest("filename not present");

                var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                                filepath);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch
            {
                var path = Path.Combine(
                              Directory.GetCurrentDirectory(),
                              "NC", "R.png");
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
        }

        #endregion

        #region Post
        #endregion

        #region Put
        #endregion

        #region Helper

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion
    }
}
