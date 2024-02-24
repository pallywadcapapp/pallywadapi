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
    public class UserBankService : BaseService, IUserBankService
    {
        private readonly IUserBankRepository _userBankRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserBankService(IUserBankRepository userBankRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _userBankRepository = userBankRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddUserBank(UserBank UserBank)
        {
            _userBankRepository.Add(UserBank);
            Save();
        }

        public List<UserBank> GetAllUserBanks()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _userBankRepository.FindAll()
                .ToList();
            return result;
            //return _UserBankRepository.Query<UserBank>("ListAllUserBank", parameters);
        }

        public List<UserBank> GetUserBank(string memberid)
        {
            var result = _userBankRepository.FindAll().Where(x => x.memberId == memberid).ToList();
            return result;
        }
        public UserBank GetDefaultUserBank(string memberid)
        {
            var result = _userBankRepository.FindAll().Where(x => x.memberId == memberid).Where(u=>u.isDefault == true)
                .FirstOrDefault();
            return result;
        }

        public UserBank GetUserBank(int id)
        {
            return _userBankRepository.Get(x => x.Id == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserBank(UserBank UserBank)
        {
            _userBankRepository.Update(UserBank);
            Save();
        }
    }

    public interface IUserBankService
    {
        void AddUserBank(UserBank userBank);
        List<UserBank> GetAllUserBanks();
        List<UserBank> GetUserBank(string id);
        UserBank GetDefaultUserBank(string memberid);
        UserBank GetUserBank(int id);
        void Save();
        void UpdateUserBank(UserBank userBank);
    }
}
