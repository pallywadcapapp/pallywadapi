using Amazon.Runtime.Documents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<DocumentsController> _logger;
        private readonly IUserDocumentService _userDocumentService;
        private readonly IAppUploadedFilesService _appUploadedFilesService;
        public DocumentsController(ILogger<DocumentsController> logger, IConfiguration configuration,
            IUserDocumentService userDocumentService, IAppUploadedFilesService appUploadedFilesService)
        {
            _logger = logger;
            _userDocumentService = userDocumentService;
            _config = configuration;
            _appUploadedFilesService = appUploadedFilesService;
        }

        #region Get
        [HttpGet("all")]
        [Authorize]
        public IActionResult GetAllDocuments()
        {
            var result = _userDocumentService.ListAllUserDocument();
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userDocumentService.ListAllUserDocument(memberId);
            return Ok(result);
        }

        [HttpGet("doctype")]
        [Authorize]
        public IActionResult Get(string documentRefId)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userDocumentService.ListAllUserDocument(memberId, documentRefId);
            return Ok(result);
        }
        #endregion

        [HttpPost, Route("UploadFile")]
        public async Task<IActionResult> OnPostUploadAsync( UserDocumentFileDto userDocument)
        {
            List<IFormFile> files = userDocument.file;
            long size = files.Sum(f => f.Length);
            List<string> filenames = new List<string>();
            List<string> status = new List<string>();
            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    string dataFileName = Path.GetFileName(formFile.FileName);
                    string extension = Path.GetExtension(dataFileName);


                    string[] allowedExtsnions = new string[] { ".png", ".jpg", "jpeg" };
                    if (!allowedExtsnions.Contains(extension))
                    {
                        status.Add("Invalid File " + dataFileName + " for upload");
                    }
                    else
                    {

                        var filename = Path.GetRandomFileName() + Path.GetExtension(formFile.FileName);
                        var filePath = Path.Combine(_config["StoredFilesPath"], filename);
                        filenames.Add(filename);
                        status.Add("File " + dataFileName + " valid for upload");
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName));
                            saveUserDocumentUploads(userDocument.documentNo, filename, userDocument.documentRefId, userDocument.expiryDate, filePath, memberId);
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

        void saveUserDocumentUploads(string docNo, string filename,string doctype, DateTime expiryDate, string path, string memberId)
        {
            var userDocument = new UserDocument()
            {
                created_date = DateTime.Now,
                documentNo = docNo,
                documentRefId = doctype,
                expiryDate = expiryDate,
                name = filename,
                status = false,
                url = path,
                userId = memberId
            };
            _userDocumentService.AddUserDocument(userDocument);
        }
        #endregion
    }

    public class UserDocumentFileDto
    {
        public string documentRefId { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string documentNo { get; set; }
        public DateTime expiryDate { get; set; }
        public bool status { get; set; }
        public List<IFormFile> file { get; set; }
    }

}
