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
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(IUnitOfWork unitOfWork, ICompanyRepository companyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
        }
        public void Addcompany(Company company)
        {
            _companyRepository.Add(company);
            Save();
        }

        public List<string> GetAllCompany()
        {
            var result = _companyRepository.FindAll().Select(u => u.name).ToList();
            return result;
        }

        public Company GetCompany()
        {
            var result = _companyRepository.FindAll().FirstOrDefault();
            return result;
        }

        public List<Company> ListAllCompany()
        {
            var result = _companyRepository.FindAll().ToList();
            return result;
        }

        public IEnumerable<Company> ListCompany()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _companyRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Updatecompany(Company company)
        {
            _companyRepository.Update(company);
            Save();
        }
    }

    public interface ICompanyService
    {
        IEnumerable<Company> ListCompany();
        Company GetCompany();
        void Addcompany(Company company);
        void Save();
        void Updatecompany(Company company);
        List<string> GetAllCompany();
        List<Company> ListAllCompany();
    }
}
