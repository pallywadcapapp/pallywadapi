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
        private readonly IMapper _mapper;
        public ProfileController(ILogger<ProfileController> logger, IUserProfileService userProfileService, 
            UserManager<AppIdentityUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _profileService = userProfileService;
            _userManager = userManager;
            _mapper = mapper;
        }

        #region Get

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

        [HttpPost]
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
           
        }
        #endregion

        #region put
        [HttpPut]
        public async Task<IActionResult> Put(UserProfileDto uprofile)
        {
            var user = await _userManager.FindByNameAsync(uprofile.memberid);
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

                IdentityResult result = await _userManager.UpdateAsync(user);
                return Ok(profile);
            }
            else
            {
                return BadRequest(new { status = "error", message = "user does not exist" });
            };
        }
        #endregion
    }
}
