using Dapper;
using Microsoft.AspNetCore.Http;
using PallyWad.Domain;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using PallyWad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{

	[TransientRegistration]
	public class BusinessInformationService : BaseService, IBusinessInformationService
	{
		private readonly IBusinessInformationRepository _bankRepository;
		private readonly IUnitOfWork _unitOfWork;
		//private readonly IHttpContextAccessor _httpContextAccessor;

		public BusinessInformationService(IBusinessInformationRepository bankRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_bankRepository = bankRepository;
		}
		public void AddBusinessInformation(BusinessInformation business)
		{
			_bankRepository.Add(business);
			Save();
		}

		public BusinessInformation GetBusinessInformations(string memberId)
		{
			return _bankRepository.Get(x => x.memberId == memberId);
		}

		public BusinessInformation GetBusinessInformations(int id)
		{
			return _bankRepository.Get(x => x.Id == id);
		}

		public List<BusinessInformation> ListBusinessInformations()
		{
			var parameters = new DynamicParameters();
			//parameters.Add("@tenantId", tenantId);

			var result = _bankRepository.FindAll().ToList();
			return result;
		}

		public List<BusinessInformation> ListMemberBusinessInformations(string memberId)
		{
			var parameters = new DynamicParameters();
			//parameters.Add("@tenantId", tenantId);

			var result = _bankRepository.FindAll().Where(x => x.memberId == memberId).ToList();
			return result;
		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void UpdateBusinessInformation(BusinessInformation business)
		{
			_bankRepository.Update(business);
			Save();
		}
	}

	public interface IBusinessInformationService
	{
		List<BusinessInformation> ListBusinessInformations();
		List<BusinessInformation> ListMemberBusinessInformations(string memberId);
		BusinessInformation GetBusinessInformations(string memberId);
		BusinessInformation GetBusinessInformations(int id);
		void AddBusinessInformation(BusinessInformation business);
		void Save();
		void UpdateBusinessInformation(BusinessInformation business);
	}
}
