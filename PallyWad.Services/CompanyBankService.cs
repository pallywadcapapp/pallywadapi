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
    public partial class CompanyBankService : ICompanyBankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyBankRepository _companyBankRepository;

        public CompanyBankService(ICompanyBankRepository companyBankRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _companyBankRepository = companyBankRepository;
        }
        public void AddCompanyBank(CompanyBank CompanyBank)
        {
            _companyBankRepository.Add(CompanyBank);
            Save();
        }

        public void DeleteCompanyBank(CompanyBank CompanyBank)
        {
            _companyBankRepository?.Delete(CompanyBank);
            Save();
        }

        public List<string> GetAllCompanyBank()
        {
            return ListAllCompanyBank().Select(u => u.name).ToList();
        }

        public CompanyBank GetCompanyBankById(int id)
        {
            return ListAllCompanyBank().Where(u => u.Id == id).FirstOrDefault();
        }

        public List<CompanyBank> GetCompanyBankByName(string name)
        {
            return ListAllCompanyBank().Where(u => u.name == name).ToList();
        }

        public List<CompanyBank> ListAllCompanyBank()
        {
            var result = _companyBankRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCompanyBank(CompanyBank CompanyBank)
        {
            _companyBankRepository.Update(CompanyBank);
            Save();
        }
    }

    public interface ICompanyBankService
    {
        void Save();
        List<CompanyBank> ListAllCompanyBank();
        List<string> GetAllCompanyBank();
        void AddCompanyBank(CompanyBank companyBank);
        void DeleteCompanyBank(CompanyBank companyBank);
        void UpdateCompanyBank(CompanyBank companyBank);
        List<CompanyBank> GetCompanyBankByName(string name);
        CompanyBank GetCompanyBankById(int id);
    }
}
