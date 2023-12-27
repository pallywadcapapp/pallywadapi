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
    public class GlAccountTier2Service : BaseService, IGlAccountTier2Service
    {
        private readonly IGlAccountTier2Repository _glAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GlAccountTier2Service(IGlAccountTier2Repository glAccountRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _glAccountRepository = glAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddGlAccount(GLAccountB glAccount)
        {
            _glAccountRepository.Add(glAccount);
            Save();
        }

        public List<GLAccountB> GetAllGlAccounts()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _glAccountRepository.FindAll().ToList();
            return result;
        }

        public GLAccountB GetGlAccount(string id)
        {
            return _glAccountRepository.Get(x => x.accountno == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateGlAccount(GLAccountB glAccount)
        {
            _glAccountRepository.Update(glAccount);
            Save();
        }
    }

    public interface IGlAccountTier2Service
    {
        void AddGlAccount(GLAccountB GlAccount);
        List<GLAccountB> GetAllGlAccounts();
        GLAccountB GetGlAccount(string id);
        void Save();
        void UpdateGlAccount(GLAccountB GlAccount);
    }
}
