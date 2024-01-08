using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IUserDocumentService _userDocumentService;
        public DocumentsController(ILogger<DocumentsController> logger, IUserDocumentService userDocumentService)
        {
            _logger = logger;
            _userDocumentService = userDocumentService;
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

        #region Post


        #endregion

        #region Put
        #endregion
    }
}
