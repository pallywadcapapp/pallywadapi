using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyBankController : ControllerBase
    {
        private readonly ICompanyBankService _companyBankService;
        private readonly IMapper _mapper;
        public CompanyBankController(ICompanyBankService companyBankService, IMapper mapper)
        {
            _companyBankService = companyBankService;
            _mapper = mapper;
        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            var result = _companyBankService.ListAllCompanyBank()
                .Where(u => u.isDelete == false).ToList();
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost]
        public IActionResult Post(CompanyBankDto companyBankDto)
        {
            var companyAcc = _mapper.Map<CompanyBank>(companyBankDto);
            companyAcc.isDelete = false;
            _companyBankService.AddCompanyBank(companyAcc);
            return Ok(companyAcc);
        }
        
        #endregion
    }
}
