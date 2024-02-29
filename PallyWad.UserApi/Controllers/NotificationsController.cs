using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationService;
        public NotificationsController(INotificationsService notificationService)
        {
            _notificationService = notificationService;
        }

        #region Get
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _notificationService.ListAllNotification(memberId)
                .OrderByDescending(u=>u.Id);
            return Ok(result);

        }

        [Authorize]
        [HttpGet("unread")]
        public IActionResult GetUnread()
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _notificationService.ListAllNotification(memberId)
                .Where(u=>u.readStatus ==  false)
                .OrderByDescending(u => u.Id);
            return Ok(result);

        }

        [Authorize]
        [HttpGet("byId")]
        public IActionResult GetById(int id)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var result = _notificationService.GetNotification(id);
            return Ok(result);

        }
        #endregion

        #region put

        [Authorize]
        [HttpPut("read")]
        public IActionResult Put(int id)
        {
            var princ = HttpContext.User;
            var memberId = princ.Identity?.Name;
            var not = _notificationService.GetNotification(id);
            if(not.memberId == memberId)
            not.readStatus = true;
            _notificationService.UpdateNotification(not);
            return Ok(not);
        }
        #endregion
    }
}
