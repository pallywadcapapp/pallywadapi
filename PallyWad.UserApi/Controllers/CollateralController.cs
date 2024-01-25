using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollateralController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CollateralController> _logger;
        private readonly IUserCollateralService _userCollateralService;
        private readonly IAppUploadedFilesService _appUploadedFilesService;
        private readonly ICollateralService _collateralService;
        public CollateralController(ILogger<CollateralController> logger, IConfiguration configuration,
            IUserCollateralService userCollateralService, IAppUploadedFilesService appUploadedFilesService, ICollateralService collateralService)
        {
            _logger = logger;
            _userCollateralService = userCollateralService;
            _config = configuration;
            _appUploadedFilesService = appUploadedFilesService;
            _collateralService = collateralService;
        }

        #region Get
        [HttpGet("all")]
        [Authorize]
        public IActionResult GetAllCollaterals()
        {
            var result = _userCollateralService.ListAllUserCollateral();
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userCollateralService.ListAllUserCollateral(memberId);
            return Ok(result);
        }

        [HttpGet("collateraltype")]
        [Authorize]
        public IActionResult Get(string collateralRefId)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userCollateralService.ListAllUserCollateral(memberId, collateralRefId);
            return Ok(result);
        }
        #endregion

        [HttpPost, Route("UploadFile")]
        public async Task<IActionResult> OnPostUploadAsync(UserCollateralFileDto userCollateral)
        {
            List<IFormFile> files = userCollateral.file;
            long size = files.Sum(f => f.Length);
            List<string> filenames = new List<string>();
            List<string> status = new List<string>();

            var princ = HttpContext.User;
            var memberId = princ.Identity.Name; 

            var collateral = _collateralService.GetCollateralByName(userCollateral.collateralRefId);
            if (collateral == null)
                return BadRequest(new Response { Status = "error", Message = "collateral type not found" });
            if (memberId == null)
                return Unauthorized(new Response { Status = "error", Message = "Token Expired" });

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    string dataFileName = Path.GetFileName(formFile.FileName);
                    string extension = Path.GetExtension(dataFileName);


                    string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };
                    if (!allowedExtsnions.Contains(extension))
                    {
                        status.Add("Invalid File " + dataFileName + " for upload");
                    }
                    else
                    {

                        var filename = Path.GetRandomFileName() + Path.GetExtension(formFile.FileName);
                        string dirPath = Path.Combine(_config["StoredFilesPath"]);
                        var filePath = Path.Combine(_config["StoredFilesPath"], filename);
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }

                        filenames.Add(filename);
                        status.Add("File " + dataFileName + " valid for upload");
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName));
                            saveUserCollateralUploads(userCollateral.collateralNo, filename, userCollateral.estimatedValue, filePath, memberId, userCollateral.otherdetails);
                        }
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filenames, status });
        }
        #region Post


        #endregion

        #region Put
        #endregion

        #region helper
        void saveUpload(string filename, string path, string Id, string filetype)
        {
            var newAppUpload = new AppUploadedFiles()
            {
                created_date = DateTime.Now,
                comment = "",
                filename = filename,
                fileurl = path,
                uploaderId = Id,
                type = filetype,
                year = DateTime.Now.Year
            };
            _appUploadedFilesService.AddAppUploadedFiles(newAppUpload);
        }

        void saveUserCollateralUploads(string collateralNo, string filename, double value, string path, string memberId, string otherdetails)
        {
            var userCollateral = new UserCollateral()
            {
                created_date = DateTime.Now,
                colleteralId = collateralNo,
                estimatedValue = value,
                approvedValue = 0,
                loanRefId = "",
                otherdetails = otherdetails,
                name = filename,
                status = true,
                url = path,
                userId = memberId,
                verificationStatus = false,
                updated_date = DateTime.Now,
            };
            _userCollateralService.AddUserCollateral(userCollateral);
        }


        public class UserCollateralFileDto
        {
            public string collateralRefId { get; set; }
            //public string userId { get; set; }
            //public string name { get; set; }
            //public string url { get; set; }
            public string collateralNo { get; set; }
            public double estimatedValue { get; set; }
            public bool status { get; set; }
            public List<IFormFile> file { get; set; }
            public string otherdetails { get; set; }
        }
        #endregion
    }
}
