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

        public LoanCollateral GetLoanCollateral(int id)
        {
            var result = _loanCollateralRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            return result;
        }

        public List<LoanCollateral> ListCollateral(string collateraId)
        {
            var result = _loanCollateralRepository.FindAll().Where(u => u.collateralId == collateraId).ToList();
            return result;
        }

        //public List<LoanCollateral> ListLoan(string loanId)
        //{
        //    var result = _loanCollateralRepository.FindAll().Where(u => u.loanId == loanId).ToList();
        //    return result;
        //}

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
        //List<LoanCollateral> ListLoan(string loanId);
        List<LoanCollateral> ListCollateral(string collateralId);
        LoanCollateral GetLoanCollateral(int id);
        void Save();
        void UpdateLoanCollateral(LoanCollateral LoanCollateral);
    }
}
