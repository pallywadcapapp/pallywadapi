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
    public class GlAccountBaseService : BaseService, IGlAccountBaseService
    {
        private readonly IGlAccountBaseRepository _glAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GlAccountBaseService(IGlAccountBaseRepository glAccountRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _glAccountRepository = glAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddGlAccount(GLAccountBase glAccount)
        {
            _glAccountRepository.Add(glAccount);
            Save();
        }

        public void AddGlAccount(GLAccountBase glAccount, string _tenantId)
        {
            _glAccountRepository.Add(glAccount);
            Save();
        }

        public List<GLAccountBase> GetAllGlAccounts()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _glAccountRepository.FindAll().ToList();
            return result;
        }

        public List<GLAccountBase> GetAllGlAccounts(string _tenantId)
        {

            var result = _glAccountRepository.FindAll().ToList();
            return result;
        }

        public GLAccountBase GetGlAccount(string id)
        {
            return _glAccountRepository.Get(x => x.accountno == id);
        }

        public GLAccountBase GetGlAccountByName(string name)
        {
            return _glAccountRepository.Get(x => x.shortdesc == name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateGlAccount(GLAccountBase glAccount)
        {
            _glAccountRepository.Update(glAccount);
            Save();
        }
    }

    public interface IGlAccountBaseService
    {
        void AddGlAccount(GLAccountBase GlAccount);
        List<GLAccountBase> GetAllGlAccounts();
        GLAccountBase GetGlAccountByName(string name);
        GLAccountBase GetGlAccount(string id);
        void Save();
        void UpdateGlAccount(GLAccountBase GlAccount);
    }
}
