using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBankController : ControllerBase
    {
        private readonly IUserBankService _userBankService;
        private readonly IMapper _mapper;
        public UserBankController(IUserBankService userBankService, IMapper mapper)
        {
            _userBankService = userBankService;
            _mapper = mapper;
        }

        #region Get
        [HttpGet("all")]
        [Authorize]
        public IActionResult GetAlBanks()
        {
            var result = _userBankService.GetAllUserBanks();
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userBankService.GetUserBank(memberId)
                .Where(u=>u.isDelete == false);
            return Ok(result);
        }

        [HttpGet("default")]
        [Authorize]
        public IActionResult GetDefaultBank()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _userBankService.GetDefaultUserBank(memberId);
            return Ok(result);
        }

        #endregion

        #region Post

        [HttpPost]
        [Authorize]
        public IActionResult Post(UserBankDto userBank)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var _userBank = _mapper.Map<UserBank>(userBank);
            _userBank.memberId = memberId;
            _userBankService.AddUserBank(_userBank);
            return Ok(_userBank);
        }
        #endregion

        #region Put

        [HttpPut]
        [Authorize]
        public IActionResult Put(UserBankDto userBank)
        {
            var _userBank = _mapper.Map<UserBank>(userBank);
            _userBankService.UpdateUserBank(_userBank);
            return Ok(_userBank);
        }

        [HttpPut("makedefault")]
        [Authorize]
        public IActionResult Put(int id)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var defaultB = _userBankService.GetDefaultUserBank(memberId);
            
            if(defaultB != null)
            {
                defaultB.isDefault = false;
                _userBankService.UpdateUserBank(defaultB);
            }
            
            var curr = _userBankService.GetUserBank(id);
            curr.isDefault = true;
            _userBankService.UpdateUserBank(curr);
            return Ok(curr);
        }

        #endregion

        #region del


        [HttpDelete("deletebank")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var curr = _userBankService.GetUserBank(id);
            curr.isDelete = true;
            _userBankService.UpdateUserBank(curr);
            return Ok(curr);
        }
        #endregion
    }
}
