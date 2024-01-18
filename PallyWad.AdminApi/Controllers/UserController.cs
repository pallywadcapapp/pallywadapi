using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            
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

    }
}
