using Amazon.Runtime.Documents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;
using System.Collections.Generic;

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
        private readonly IDocumentService _documentService;
        public DocumentsController(ILogger<DocumentsController> logger, IConfiguration configuration,
            IUserDocumentService userDocumentService, IAppUploadedFilesService appUploadedFilesService, IDocumentService documentService)
        {
            _logger = logger;
            _userDocumentService = userDocumentService;
            _config = configuration;
            _appUploadedFilesService = appUploadedFilesService;
            _documentService = documentService;
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

            //var document = _documentService.GetDocumentByName(userDocument.documentRefId);
            var document = _documentService.GetDocumentById(int.Parse(userDocument.documentRefId));
            if (document == null)
                return BadRequest(new Response { Status = "error", Message = "document type not found" });
            if (memberId == null)
                return Unauthorized(new Response { Status = "error", Message = "Token Expired" });

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    string dataFileName = Path.GetFileName(formFile.FileName);
                    string extension = Path.GetExtension(dataFileName);


                    string[] allowedExtsnions = new string[] { ".png", ".jpg", ".jpeg" };
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
                        /*using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName));
                            saveUserDocumentUploads(userDocument.documentNo, filename, userDocument.documentRefId, userDocument.expiryDate, filePath, memberId);
                        }*/
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                            saveUpload(filename, filePath, memberId, Path.GetExtension(formFile.FileName));
                            saveUserDocumentUploads(userDocument.documentNo, document.type, document.name, userDocument.documentRefId, userDocument.expiryDate, filePath, memberId);
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

        void saveUserDocumentUploads(string docNo, string type, string name,string refId, DateTime expiryDate, string path, string memberId)
        {
            var userDocument = new UserDocument()
            {
                created_date = DateTime.Now,
                documentNo = docNo,
                documentRefId = refId,
                expiryDate = expiryDate,
                name = name,
                doctype = type,
                status = false,
                url = path,
                userId = memberId
            };
            _userDocumentService.AddUserDocument(userDocument);
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
        #endregion
    }

    public class UserDocumentFileDto
    {
        public string documentRefId { get; set; }
        //public string userId { get; set; }
        //public string name { get; set; }
        //public string url { get; set; }
        public string documentNo { get; set; }
        public DateTime expiryDate { get; set; }
        //public bool status { get; set; }
        public List<IFormFile> file { get; set; }
    }

}
