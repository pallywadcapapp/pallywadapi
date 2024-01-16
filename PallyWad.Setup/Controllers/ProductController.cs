using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IProductService _productService;
        public ProductController(ILogger<ProductController> logger, IHttpContextAccessor contextAccessor, IProductService productService)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            _productService = productService;

        }

        #region Get
        [HttpGet]
        public IActionResult Get() { 
            var result = _productService.ListAllProducts();
            return Ok(result);
        }

        [HttpGet("name")]
        public IActionResult Get(string name)
        {
            var result = _productService.GetProduct(name);
            return Ok(result);
        }

        [HttpGet("list")]
        public IActionResult GetProductList()
        {
            var result = _productService.ListAllProducts().Select(u=>u.Name);
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost]

        public IActionResult Post(Product product)
        {
            try
            {
                if(product == null)
                {
                    return BadRequest(new Response { Status = "error", Message = "No record" });
                }
                else
                {
                    _productService.AddProduct(product);
                    return Ok(new Response { Status = "success", Message = product });
                }
            }catch (Exception ex)
            {
                return BadRequest(new Response { Status = "error", Message = ex.Message });
            }
        }
        #endregion
    }
}
