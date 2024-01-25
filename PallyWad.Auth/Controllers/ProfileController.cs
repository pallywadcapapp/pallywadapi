using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
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
        private readonly IMapper _mapper;
        public ProfileController(ILogger<ProfileController> logger, IUserProfileService userProfileService, 
            UserManager<AppIdentityUser> userManager, IMapper mapper, IUserDocumentService documentService )
        {
            _logger = logger;
            _profileService = userProfileService;
            _userManager = userManager;
            _mapper = mapper;
            _documentService = documentService;
        }

        #region Get

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
        #endregion

        #region helper
        private bool hasDocument(string username, string doctype)
        {
            var result = _documentService.ListAllUserDocument(username).Where(u=>u.name == doctype).FirstOrDefault();
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
        #endregion
    }
}
