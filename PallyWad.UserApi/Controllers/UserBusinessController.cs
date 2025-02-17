﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
using PallyWad.Services;

namespace PallyWad.UserApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserBusinessController : ControllerBase
	{
        private readonly IBusinessInformationService _businessInformation;
		private readonly IMapper _mapper;
		public UserBusinessController(IBusinessInformationService businessInformation, IMapper mapper)
        {
			_businessInformation = businessInformation;
			_mapper = mapper;

		}


		#region Get
		[HttpGet("all")]
		[Authorize]
		public IActionResult GetAlBusiness()
		{
			var result = _businessInformation.ListBusinessInformations();
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		public IActionResult Get()
		{
			var princ = HttpContext.User;
			var memberId = princ.Identity?.Name;
			var result = _businessInformation.GetBusinessInformations(memberId);
			return Ok(result);
		}

		#endregion

		#region Post

		[HttpPost]
		[Authorize]
		public IActionResult Post(UserBusinessInfoDto info)
		{
			var princ = HttpContext.User;
			var memberId = princ.Identity?.Name;
			var _info = _mapper.Map<BusinessInformation>(info);
			_info.memberId = memberId;
			_businessInformation.AddBusinessInformation(_info);
			return Ok(_info);
		}
		#endregion

		#region Put

		[HttpPut]
		[Authorize]
		public IActionResult Put(UserBusinessInfoDto info)
		{
			var princ = HttpContext.User;
			var memberId = princ.Identity?.Name;
			//var _info = _mapper.Map<BusinessInformation>(info);
			var result = _businessInformation.GetBusinessInformations(memberId);
			result.address = info.address;
			result.name = info.name;
			result.cacno = info.cacno;
			result.landmark = info.landmark;
			result.type = info.type;
			result.business_email = info.business_email;
			result.business_phoneNo = info.business_phoneNo;
			result.sector = info.sector;
			result.services = info.services;
			result.tin = info.tin;
			_businessInformation.UpdateBusinessInformation(result);
			return Ok(result);
		}
		#endregion
	}
}
