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
    public class LoanCollateralService : BaseService, ILoanCollateralService
    {
        private readonly ILoanCollateralRepository _loanCollateralRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoanCollateralService(ILoanCollateralRepository loanCollateralRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _loanCollateralRepository = loanCollateralRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddLoanCollateral(LoanCollateral loanCollateral)
        {
            
            _loanCollateralRepository.Add(loanCollateral);
            Save();
        }

        public List<LoanCollateral> GetAllLoanCollateral()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _loanCollateralRepository.FindAll().ToList();
            return result;
            //return _LoanCollateralRepository.Query<LoanCollateralS>("ListAllLoanCollateral", parameters);
        }

        public List<LoanCollateral> ListCollateralHistory(string memberId)
        {
            return _loanCollateralRepository.FindAll().Where(x => x.memberid == memberId)
            //.Where(u=>u.Repay == 1)
            //.OrderByDescending(x=>x.Transdate)
            .ToList();
        }
        public LoanCollateral GetLoanCollateral(string id)
        {
            return _loanCollateralRepository.Get(x => x.loancode == id);
        }

        public LoanCollateral GetLoanCollateralByRef(string id)
        {
          
            return _loanCollateralRepository.Get(x => x.loanrefnumber == id);
        }

        public LoanCollateral GetLoanCollateralByMemberId(string id)
        {
            return _loanCollateralRepository.FindAll().Where(x => x.memberid == id).OrderByDescending(x => x.transdate).FirstOrDefault();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateLoanCollateral(LoanCollateral loanCollateral)
        {
            
            _loanCollateralRepository.Update(loanCollateral);
            Save();
        }
    }

    public interface ILoanCollateralService
    {
        void AddLoanCollateral(LoanCollateral LoanCollateral);
        List<LoanCollateral> GetAllLoanCollateral();
        LoanCollateral GetLoanCollateralByMemberId(string id);
        List<LoanCollateral> ListCollateralHistory(string memberId);
        LoanCollateral GetLoanCollateral(string id);
        LoanCollateral GetLoanCollateralByRef(string id);
        void Save();
        void UpdateLoanCollateral(LoanCollateral LoanCollateral);
    }
}
