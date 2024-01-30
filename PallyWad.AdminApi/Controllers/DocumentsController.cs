using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Services;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IUserDocumentService _userDocumentService;
        private readonly IDocumentService _documentService;
        public DocumentsController(IUserDocumentService userDocumentService, IDocumentService documentService)
        {
            _userDocumentService = userDocumentService;
            _documentService = documentService;

        }

        #region Get

        [HttpGet()]
        public IActionResult Get()
        {
            var result = _userDocumentService.ListAllUserDocument();
            return Ok(result);
        }

        [HttpGet("Unapproved")]
        public IActionResult GetUnapproved() {
            var result = _userDocumentService.ListAllUserDocument().Where(u=>u.status == false);
            return Ok(result);
        }

        [HttpGet("userDocById")]
        public IActionResult GetUserDocById(int Id)
        {
            var result = _userDocumentService.ListAllUserDocument().Where(u => u.Id == Id).FirstOrDefault();
            return Ok(result);
        }
        #endregion

        #region Post
        #endregion

        #region Put
        #endregion

        #region Helper
        #endregion
    }
}
