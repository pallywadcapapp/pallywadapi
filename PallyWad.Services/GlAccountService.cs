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
    public class GlAccountService : BaseService, IGlAccountService
    {
        private readonly IGlAccountsRepository _glAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GlAccountService(IGlAccountsRepository glAccountRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _glAccountRepository = glAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddGlAccount(GLAccount glAccount)
        {
            _glAccountRepository.Add(glAccount);
            Save();
        }

        public List<GLAccount> GetAllGlAccounts()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _glAccountRepository.FindAll().ToList();
            return result;
        }

        public GLAccount GetGlAccount(string id)
        {
            return _glAccountRepository.Get(x => x.accountno == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateGlAccount(GLAccount glAccount)
        {
            _glAccountRepository.Update(glAccount);
            Save();
        }

        public string GetAllGlAccountByDesc()
        {
            var result = _glAccountRepository.Query<string>("GetNewAccountId").FirstOrDefault();
            return result;
        }

        public GLAccount GetAccByName(string name)
        {

            var result = _glAccountRepository.FindAll()
            .Where(u => u.shortdesc.Contains(name)).FirstOrDefault();
            return result;
        }

        public GLAccount GetChartByGL(string glcode)
        {
            return _glAccountRepository.Get(x => x.accountno == glcode);
        }
    }

    public interface IGlAccountService
    {
        void AddGlAccount(GLAccount GlAccount);
        GLAccount GetAccByName(string name);
        string GetAllGlAccountByDesc();
        List<GLAccount> GetAllGlAccounts();
        GLAccount GetChartByGL(string glcode);
        GLAccount GetGlAccount(string id);
        void Save();
        void UpdateGlAccount(GLAccount GlAccount);
    }
}
