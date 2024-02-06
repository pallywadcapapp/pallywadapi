using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        public CompanyController()
        {
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = new Company() {
                name = "PallyWad Capital",
                address = "120 Ikate Street",
                email = "info@pallywad.com",
                bank = "Polaris Bank",
                accountno = "0011322456",
                phoneno = "08021455678"
            };
            return Ok(result);
        }
    }
}
