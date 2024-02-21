using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Services;

namespace PallyWad.Setup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //var result = new Company() {
            //    name = "PallyWad Capital",
            //    address = "120 Ikate Street",
            //    email = "info@pallywad.com",
            //    bank = "Polaris Bank",
            //    accountno = "0011322456",
            //    phoneno = "08021455678"
            //};
            var result = _companyService.GetCompany();
            return Ok(result);
        }
    }
}
