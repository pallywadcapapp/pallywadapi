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
    public class BankDepositService : BaseService, IBankDepositService
    {
        private readonly IBankDepositRepository _bankRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public BankDepositService(IBankDepositRepository bankRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _bankRepository = bankRepository;
        }
        public void AddBankDeposit(BankDeposit bank)
        {
            _bankRepository.Add(bank);
            Save();
        }

        public BankDeposit GetBankDeposits(string id)
        {
            return _bankRepository.Get(x => x.depositId == id);
        }

        public BankDeposit GetBankDeposits(int id)
        {
            return _bankRepository.Get(x => x.Id == id);
        }

        public List<BankDeposit> ListBankDeposits()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _bankRepository.FindAll().ToList();
            return result;
        }

        public List<BankDeposit> ListMemberBankDeposits(string memberId)
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

        public void UpdateBankDeposit(BankDeposit bank)
        {
            _bankRepository.Update(bank);
            Save();
        }
    }

    public interface IBankDepositService
    {
        List<BankDeposit> ListBankDeposits();
        List<BankDeposit> ListMemberBankDeposits(string memberId);
        BankDeposit GetBankDeposits(string id);
        BankDeposit GetBankDeposits(int id);
        void AddBankDeposit(BankDeposit bank);
        void Save();
        void UpdateBankDeposit(BankDeposit bank);
    }
}
