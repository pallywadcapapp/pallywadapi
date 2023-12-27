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
    public class MembersAccountService : BaseService, IMembersAccountService
    {
        private readonly IMembersAccountRepository _membersAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MembersAccountService(IMembersAccountRepository membersAccountRepository, IUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor)
        {
            _membersAccountRepository = membersAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddMembersAccount(MemberAccount membersAccount)
        {
            _membersAccountRepository.Add(membersAccount);
            Save();
        }

        public List<MemberAccount> ListAllMembersAccounts()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _membersAccountRepository.FindAll().ToList();
            return result;
            //return _membersAccountRepository.Query<Tblmemberaccount>("ListAllMembersAccount", parameters);
        }

        public List<MemberAccount> ListMembersAccounts(string memberId)
        {

            var result = _membersAccountRepository.FindAll().Where(x => x.memberid == memberId).ToList();
            return result;
        }
        public List<string> ListMembersAccountsOnly(string memberId)
        {

            var result = _membersAccountRepository.FindAll().Where(x => x.memberid == memberId)
                .Select(u => u.accountno)
                .ToList();
            return result;
        }

        public MemberAccount GetMembersAccount(int id)
        {
            return _membersAccountRepository.Get(x => x.Id == id);
        }
        public MemberAccount GetLoanTransMemberAcc(string memberId, string loanref)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            parameters.Add("@refno", loanref);
            var result = _membersAccountRepository.Query<MemberAccount>("GetLoanTransMemberAcc", parameters).FirstOrDefault();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateMembersAccount(MemberAccount membersAccount)
        {
            _membersAccountRepository.Update(membersAccount);
            Save();
        }
    }

    public interface IMembersAccountService
    {
        void AddMembersAccount(MemberAccount membersAccount);
        List<MemberAccount> ListAllMembersAccounts();
        List<MemberAccount> ListMembersAccounts(string memberId);
        List<string> ListMembersAccountsOnly(string memberId);
        MemberAccount GetMembersAccount(int id);
        MemberAccount GetLoanTransMemberAcc(string memberId, string loanref);
        void Save();
        void UpdateMembersAccount(MemberAccount membersAccount);
    }
}
