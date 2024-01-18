using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;
using StackExchange.Redis;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        public RolesController(RoleManager<IdentityRole> roleManager, IHttpContextAccessor contextAccessor, IRoleService roleService,
            IUserService userService)
        {

            _contextAccessor = contextAccessor;
            _roleService = roleService;
            _roleManager = roleManager;
            _userService = userService;

        }

        #region Get
        /// <summary>
        /// Return only list of all roles
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("Get")]
        [Route("AllRolesList")]
        public IActionResult GetAllRolesList()
        {
            //var result = _roleService.GetAllRoles().ToList(); //_repo.GetAllRolesList();
            var result = _roleManager.Roles.ToList();
            return Ok(result);
        }


        [AcceptVerbs("Get")]
        [Route("GetAllUsersInRoles")]
        public IActionResult GetAllUsersInRoles()
        {
            var result = _roleService.GetAllUsersInRoles();
            return Ok(result);
        }
        #endregion

        #region Post

        [HttpPost]
        [Route("CreateRole")]
        public IActionResult CreateRole(string role)
        {
            //_repo = new AuthRepository();
            RoleViewModel rvm = new RoleViewModel() { Description = "Administrator Account", Name = role };
            var _role = new IdentityRole(rvm.Name);
            _role.NormalizedName = rvm.Name.ToLower();

            try
            {
                _roleService.CreateRole(_role); //_repo.CreateRole(rvm);
                //if (!result.Succeeded)
                //{
                //    return BadRequest();
                //}
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }



        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string user, string role)
        {
            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var userId = _userService.GetUserByEmail(user); //_repo.FindUserByEmail(user);
            var result = await _userManager.AddToRoleAsync(userId, role); //await _repo.AddUserToRole(userId.Id, role);

            return Ok(result);
        }
        [HttpPost, HttpGet]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string role, string user)
        {
            //var userId = await _repo.FindUserByEmail(user);
            //var result = await _repo.RemoveUserFromRole(user, role);
            var _userManager = HttpContext.RequestServices
                                        .GetRequiredService<UserManager<AppIdentityUser>>();
            var users = _roleService.GetUserFromEmail(user);
            var result = await _userManager.RemoveFromRoleAsync(users, role);
            return Ok(result);
        }
        #endregion
    }
}
