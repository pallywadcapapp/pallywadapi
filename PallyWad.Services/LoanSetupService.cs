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
    public class LoanSetupService : BaseService, ILoanSetupService
    {
        private readonly ILoanSetupRepository _loanSetupRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoanSetupService(ILoanSetupRepository loanSetupRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _loanSetupRepository = loanSetupRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddLoanSetup(LoanSetup loanSetup)
        {
            _loanSetupRepository.Add(loanSetup);
            Save();
        }

        public List<LoanSetup> GetAllLoanSetups()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _loanSetupRepository.FindAll().ToList();
            return result;
            //return _loanSetupRepository.Query<LoanSetup>("ListAllLoanSetup", parameters);
        }

        public LoanSetup GetLoanSetup(string id)
        {
            return _loanSetupRepository.Get(x => x.Loancode == id);
        }

        public LoanSetup GetLoanSetup(int id)
        {
            return _loanSetupRepository.Get(x => x.Id == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateLoanSetup(LoanSetup loanSetup)
        {
            _loanSetupRepository.Update(loanSetup);
            Save();
        }
    }

    public interface ILoanSetupService
    {
        void AddLoanSetup(LoanSetup LoanSetup);
        List<LoanSetup> GetAllLoanSetups();
        LoanSetup GetLoanSetup(string id);
        LoanSetup GetLoanSetup(int id);
        void Save();
        void UpdateLoanSetup(LoanSetup LoanSetup);
    }
}
