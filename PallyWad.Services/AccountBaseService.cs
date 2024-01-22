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
    public class AccountBaseService : BaseService, IAccountBaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountBaseRepository _accountBaseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountBaseService(IUnitOfWork unitOfWork, IAccountBaseRepository accountBaseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _accountBaseRepository = accountBaseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddAccBase(AccountBase accountBase)
        {
            _accountBaseRepository.Add(accountBase);
            Save();
        }

        public List<string> GetAccountBaseName()
        {
            var temp = ListAccountBase().Select(u => u.Type).ToList();
            return temp;
        }

        public List<AccountBase> ListAccountBase()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);
            return _accountBaseRepository.GetAll().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateAccBase(AccountBase accountBase)
        {
            _accountBaseRepository.Update(accountBase);
            Save();
        }
    }

    public interface IAccountBaseService
    {
        List<AccountBase> ListAccountBase();
        List<string> GetAccountBaseName();
        void AddAccBase(AccountBase accountBase);
        void UpdateAccBase(AccountBase accountBase);
        void Save();
    }
}
