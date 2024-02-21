using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;
using System.IO;
using System;

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
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userCollateralService.ListAllUserCollateral(memberId);
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

        [HttpGet, Route("Uploads")]
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return BadRequest("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "FileUploads", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        [HttpGet, Route("FileUploads")]
        public async Task<IActionResult> FileDownload(string filepath)
        {
            try { 
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

        [HttpGet, Route("Files")]
        public async Task<IActionResult> GetFileDownload(string id)
        {
            if (id == null)
                return BadRequest("filename not present");
            var coll = _appUploadedFilesService.ListAllSetupAppUploadedFiles()
                .Where(u=>u.comment == id && u.transOwner == "collateral");

            if (coll == null)
                return NotFound("collateral file not found");


            //var filepath = coll.fileurl;

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //                filepath);

            //var memory = new MemoryStream();
            //using (var stream = new FileStream(path, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            //return File(memory, GetContentType(path), Path.GetFileName(path));
            return Ok(coll);
        }
        #endregion

        #region Post

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
            if (files == null)
                return BadRequest();

            int collateralId = saveUserCollateralUploads(collateral.Id.ToString(), collateral.name, userCollateral.estimatedValue, memberId, userCollateral.otherdetails);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    string dataFileName = Path.GetFileName(formFile.FileName);
                    string extension = Path.GetExtension(dataFileName);


                    string[] allowedExtsnions = new string[] { ".png", ".jpg", ".jpeg", ".pdf" };
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
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName), collateralId.ToString());
                        }
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filenames, status, id = collateralId });
        }
       
        #endregion

        #region Put
        #endregion

        #region helper
        void saveUpload(string filename, string path, string Id, string filetype, string collateralId)
        {
            var newAppUpload = new AppUploadedFiles()
            {
                created_date = DateTime.Now,
                comment = collateralId,
                filename = filename,
                fileurl = path,
                uploaderId = Id,
                type = filetype,
                year = DateTime.Now.Year,
                transOwner = "collateral"
            };
            _appUploadedFilesService.AddAppUploadedFiles(newAppUpload);
        }

        int saveUserCollateralUploads(string collateralNo, string collateralName, double value, string memberId, string otherdetails)
        {
            var userCollateral = new UserCollateral()
            {
                created_date = DateTime.Now,
                colleteralId = collateralNo,
                estimatedValue = value,
                approvedValue = 0,
                loanRefId = "",
                otherdetails = otherdetails,
                name = collateralName,
                status = true,
                url = "-",
                userId = memberId,
                verificationStatus = false,
                updated_date = DateTime.Now,
            };
            _userCollateralService.AddUserCollateral(userCollateral);
            return userCollateral.Id;
        }
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


        public class UserCollateralFileDto
        {
            public string collateralRefId { get; set; }
            //public string userId { get; set; }
            //public string name { get; set; }
            //public string url { get; set; }
            //public string collateralNo { get; set; }
            public double estimatedValue { get; set; }
            //public bool status { get; set; }
            public List<IFormFile> file { get; set; }
            public string otherdetails { get; set; }
        }
        #endregion
    }
}
