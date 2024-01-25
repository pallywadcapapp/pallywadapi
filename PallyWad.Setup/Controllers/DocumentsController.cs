using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain.Dto;
using PallyWad.Domain;
using PallyWad.Services;
using AutoMapper;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {

        private readonly ILogger<DocumentsController> _logger;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;

        public DocumentsController(ILogger<DocumentsController> logger, IDocumentService documentService, IMapper mapper)
        {
            _logger = logger;
            _documentService = documentService;
            _mapper = mapper;
        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _documentService.GetAllDocument();
            return Ok(result);
        }

        [HttpGet]
        [Route("type")]
        public IActionResult GetDocType()
        {
            string[] result = ["Address","Identity"];
            //var result = _mapper.Map<SetupDto>(Documents);
            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var result = _documentService.ListAllDocument();
            //var result = _mapper.Map<SetupDto>(Documents);
            return Ok(result);
        }

        #endregion

        #region Post
        [HttpPost(nameof(Document))]
        public IActionResult Post(SetupDto events)
        {

            try
            {
                if (events != null)
                {
                    var document = _mapper.Map<Document>(events);
                    _documentService.AddDocument(document);
                    return Ok(new { status = "success", message = $"Document {document.name} Created Successfully" });
                }
                else
                {
                    return BadRequest(new { status = "error", message = "parameter is empty" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        #endregion

        #region Put
        [HttpPut(nameof(Document))]
        public IActionResult Put(SetupDto events)
        {
            try
            {
                if (events != null)
                {
                    var document = _mapper.Map<Document>(events);
                    _documentService.UpdateDocument(document);
                    return Ok(new { status = "success", message = $"Document {document.name} Updated Successfully" });
                }
                else
                {
                    return BadRequest(new { status = "error", message = "parameter is empty" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        #endregion
    }

}
