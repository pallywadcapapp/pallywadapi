using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [AcceptVerbs("Get")]
        [Route("RolesUsersLists")]
        public IActionResult GetRolesUsersLists()
        {
            /*var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value);
            return Ok(roles);*/
            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var userIdNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var tokenClaims = new JwtSecurityToken(token.Replace("Bearer ", string.Empty));
            var oid = tokenClaims.Payload["username"].ToString();

            //var user = _userManager.GetUserAsync(User).Result;
            //var uid = User?.FindFirst(x => x.Type.Equals("sub"))?.Value;
            //var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); 

            //var u = _userManager.FindByEmailAsync(oid).Result; //.FindByIdAsync(oid).Result;
            var u = _userManager.FindByNameAsync(oid).Result; //.FindByIdAsync(oid).Result;

            //var user = _userManager.FindByIdAsync(userId).Result;

            var roles = _userManager.GetRolesAsync(u).Result;
            //var roles = _userManager.GetRolesAsync(u).Result;

            return Ok(roles);
        }

        [HttpGet]
        [Route("AllActiveUsers")]
        public IActionResult GetActiveUser()
        {
            var result = _userService.GetAllUsers().Where(u => u.EmailConfirmed == true);
            return Ok(result);
        }

        [HttpGet]
        [Route("AllInactiveUsers")]
        public IActionResult GetInactiveUser()
        {
            var result = _userService.GetAllUsers().Where(u => u.EmailConfirmed == false);
            return Ok(result);
        }
        [AcceptVerbs("Get")]
        [Route("AllUsers")]
        public IActionResult GetAllUsers()
        {
            //var result = _repo.GetAllUsers();
            var result = _userService.GetAllUsers();//.Where(u=>u.TenantId == TenantId);
            return Ok(result);
        }

        [AcceptVerbs("Get")]
        [Route("UserById")]
        public IActionResult GetUserById(string email)
        {
            //var result = _repo.GetAllUsers();
            var result = _userService.GetUserByEmail(email);//.Where(u=>u.TenantId == TenantId);
            return Ok(result);
        }

        [AcceptVerbs("Get")]
        [Route("ActiveUsers")]
        public IActionResult GetActiveUsers()
        {
            //var result = _repo.GetAllUsers();
            var result = _userService.GetAllUsers().Where(u => u.EmailConfirmed == true);
            return Ok(result);
        }

        [AcceptVerbs("Get")]
        [Route("InactiveUsers")]
        public IActionResult GetInactiveUsers()
        {
            //var result = _repo.GetAllUsers();
            var result = _userService.GetAllUsers().Where(u => u.EmailConfirmed == false);
            return Ok(result);
        }

    }
}
