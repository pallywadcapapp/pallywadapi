using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Infrastructure.Migrations;
using PallyWad.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace PallyWad.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IUserProfileService _profileService;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IUserDocumentService _documentService;
        private readonly IAppUploadedFilesService _appUploadedFilesService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public ProfileController(ILogger<ProfileController> logger, IUserProfileService userProfileService, 
            UserManager<AppIdentityUser> userManager, IMapper mapper, IUserDocumentService documentService,
            IAppUploadedFilesService appUploadedFilesService, IConfiguration config)
        {
            _logger = logger;
            _profileService = userProfileService;
            _userManager = userManager;
            _mapper = mapper;
            _documentService = documentService;
            _appUploadedFilesService = appUploadedFilesService;
            _config = config;
        }

        #region Get

        [Authorize]
        [HttpGet("iseligible")]
        public async Task<IActionResult> GetIsEligible(){//string username) {
            try
            {

            var princ = HttpContext.User;
            var username = princ.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if(user.dob == null)
            {
                return Ok(new Response { Status = "success", Message = "update date of birth profile" });
            }else if(user.sex == null || user.sex == "")
            {
                return Ok(new Response { Status = "success", Message = "update gender profile" });
            }else if (user.address == null || user.address == "")
            {
                return Ok(new Response { Status = "success", Message = "update resident address profile" });
            }
            else if (user.employmentStatus == null || user.employmentStatus == "")
            {
                return Ok(new Response { Status = "success", Message = "update employment status profile" });
            }
            else if (user.PhoneNumber == null || user.PhoneNumber == "")
            {
                return Ok(new Response { Status = "success", Message = "update phone number profile" });
            }
            var isValidID = hasDocument(username, "Identity");
            var isValidAddress = hasDocument(username, "Address");
            if(isValidID == false || isValidAddress == false)
            {
                return Ok(new Response { Status = "success", Message = "upload all required documents profile" });
            }
            return Ok(new Response { Status = "success", Message = true });

            }catch(Exception ex)
            {
                return BadRequest(new Response { Status = "ërror", Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
            //var user = _profileService.GetUserProfile(username);
            var user = await _userManager.FindByNameAsync(username);
            var usermap = _mapper.Map<UserProfileDto>(user);
            return Ok(usermap);
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

        [Authorize(Roles ="Admin")]
        [HttpGet("Members")]
        public IActionResult GetAllUsers()
        {
            var members =_userManager.Users.ToList();
            var usermap = _mapper.Map<List<UserProfileDto>>(members);
            return Ok(usermap);
        }


        #endregion

        #region post

        /*[HttpPost]
        public async Task<IActionResult> Post(UserProfileDto uprofile)
        {
            
            var user = await _userManager.FindByNameAsync(uprofile.memberid);
            var profile = _mapper.Map<UserProfile>(uprofile);
            //profile.AppIdentityUser = user;
            if ((user != null))
            {
                //profile.AppIdentityUser = user;
                _profileService.AddUserProfile(profile);
                return Ok(profile);
            }else {
                return BadRequest(new { status = "error", message = "user does not exist" });
                    };
           
        }*/
        #endregion

        #region put
        [HttpPut]
        public async Task<IActionResult> Put(UserProfileDto uprofile)
        {
            try { 
            var princ = HttpContext.User;
            var memberid = princ.Identity.Name;
            var user = await _userManager.FindByNameAsync(memberid);
            var profile = _mapper.Map<AppIdentityUser>(uprofile);
            //profile.AppIdentityUser = user;
            if ((user != null))
            {
                user.Email = profile.Email;
                user.firstname = profile.firstname;
                user.PhoneNumber = profile.PhoneNumber;
                user.lastname = profile.lastname;
                user.othernames = profile.othernames;
                user.address = profile.address;
                user.bvn = profile.bvn;
                user.dob = profile.dob;
                user.sex = profile.sex;
                user.employmentStatus = profile.employmentStatus;

                IdentityResult result = await _userManager.UpdateAsync(user);
                return Ok(profile);
            }
            else
            {
                return BadRequest(new { status = "error", message = "user does not exist" });
            };
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = "ërror", Message = ex.Message });
            }
        }


        [Authorize]
        [HttpPut, Route("saveProfilePicture")]
        public async Task<IActionResult> OnPostImageUploadAsync(IFormFile file)
        {
            long size = file.Length;
            List<string> filenames = new List<string>();
            List<string> status = new List<string>();
            var princ = HttpContext.User;
            var memberId = princ.Identity.Name;
            if (file.Length > 0)
            {
                //var filePath = Path.GetTempFileName();
                string dataFileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(dataFileName);


                string[] allowedExtsnions = new string[] { ".jpg", ".jpeg", ".png", ".tiff", ".gif" };
                if (!allowedExtsnions.Contains(extension))
                {
                    status.Add("Invalid File " + dataFileName + " for upload");
                }
                else
                {
                    var dirPath = Path.Combine(_config["StoredFilesPath"], _config["ProfileImage"]);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    var filename = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(_config["StoredFilesPath"], _config["ProfileImage"], filename);
                    filenames.Add(filename);
                    status.Add("File " + dataFileName + " valid for upload");
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        saveUpload(filename, filePath, memberId, Path.GetExtension(file.FileName));
                        await UpdateMemberProfileUrl(filePath, memberId);
                        await file.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = 1, size, filenames, status });
        }


        
        #endregion

        #region helper
        private bool hasDocument(string username, string doctype)
        {
            var result = _documentService.ListAllUserDocument(username).Where(u=>u.doctype == doctype).FirstOrDefault();
            if(result == null)
            {
                return false;
            }
            else
            {
                if (result.status == false)
                    return false;
                return true;
            }
        }

        [HttpGet("age")]
        public int Age(DateTime dob)
        {
            var result = CalculateAge(dob);
            return result;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        private string CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            Years, Months, Days, Hours, Seconds);
        }

        async Task UpdateMemberProfileUrl(string filePath, string memberId)
        {
            var member = await _userManager.FindByNameAsync(memberId);
            member.imgUrl = filePath;
            await _userManager.UpdateAsync(member);
        }

        void saveUpload(string filename, string path, string Id, string filetype)
        {
            var newAppUpload = new AppUploadedFiles()
            {
                comment = "",
                filename = filename,
                fileurl = path,
                uploaderId = Id,
                type = filetype,
                year = DateTime.Now.Year,
                created_date = DateTime.Now,
                extractedStatus = true,
                transOwner = "profile"
            };
            _appUploadedFilesService.AddAppUploadedFiles(newAppUpload);
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
}
