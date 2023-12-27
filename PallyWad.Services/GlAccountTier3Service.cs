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
    public class GlAccountTier3Service : BaseService, IGlAccountTier3Service
    {
        private readonly IGlAccountTier3Repository _glAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GlAccountTier3Service(IGlAccountTier3Repository glAccountRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _glAccountRepository = glAccountRepository;
            _unitOfWork = unitOfWork;

        }

        public void AddGlAccount(GLAccountC glAccount)
        {
            _glAccountRepository.Add(glAccount);
            Save();
        }

        public List<GLAccountC> GetAllGlAccounts()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _glAccountRepository.FindAll().ToList();
            return result;
        }



        public string GetAllGlAccountByDesc()
        {
            var result = _glAccountRepository.Query<string>("GetNewAccountCId").FirstOrDefault();
            return result;
        }

        public GLAccountC GetGlAccount(string id)
        {
            return _glAccountRepository.Get(x => x.accountno == id);
        }

        public GLAccountC GetGlAccountByDesc(string id)
        {
            return _glAccountRepository.Get(x => x.fulldesc == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateGlAccount(GLAccountC glAccount)
        {
            _glAccountRepository.Update(glAccount);
            Save();
        }
    }

    public interface IGlAccountTier3Service
    {
        void AddGlAccount(GLAccountC GlAccount);
        List<GLAccountC> GetAllGlAccounts();
        GLAccountC GetGlAccount(string id);
        GLAccountC GetGlAccountByDesc(string id);
        string GetAllGlAccountByDesc();
        void Save();
        void UpdateGlAccount(GLAccountC GlAccount);
    }
}
