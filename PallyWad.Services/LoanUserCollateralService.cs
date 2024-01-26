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
    public class LoanUserCollateralService : ILoanUserCollateralService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoanUserCollateralRepository _userCollateralRepository;

        public LoanUserCollateralService(ILoanUserCollateralRepository userCollateralRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userCollateralRepository = userCollateralRepository;
        }
        public void AddUserCollateral(LoanUserCollateral userCollateral)
        {
            _userCollateralRepository.Add(userCollateral);
            Save();
        }

        public void DeleteUserCollateral(LoanUserCollateral userCollateral)
        {
            _userCollateralRepository?.Delete(userCollateral);
            Save();
        }

        public List<string> GetAllUserCollateral()
        {
            return ListAllUserCollateral().Select(u => u.userCollateralId).ToList();
        }

        public LoanUserCollateral GetUserCollateralById(int id)
        {
            //return ListAllUserCollateral().Where(u => u.Id == id).FirstOrDefault();
            var result = _userCollateralRepository.FindById(id);
            return result;
        }

        public List<LoanUserCollateral> ListAllUserCollateral()
        {
            var result = _userCollateralRepository.FindAll().ToList();
            return result;
        }

        public List<LoanUserCollateral> ListAllUserCollateral(int loanRequestId)
        {
            var result = _userCollateralRepository.FindAll().Where(u => u.loanRequestId == loanRequestId).ToList();
            return result;
        }

        public List<LoanUserCollateral> ListAllUserCollateral(int loanRequestId, string CollateralRefId)
        {
            var result = _userCollateralRepository.FindAll().Where(u => u.loanRequestId == loanRequestId && u.userCollateralId == CollateralRefId).ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserCollateral(LoanUserCollateral userCollateral)
        {
            _userCollateralRepository.Update(userCollateral);
            Save();
        }
    }

    public interface ILoanUserCollateralService
    {
        void Save();
        List<LoanUserCollateral> ListAllUserCollateral();
        List<LoanUserCollateral> ListAllUserCollateral(int loanRequestId);
        List<LoanUserCollateral> ListAllUserCollateral(int loanRequestId, string CollateralRefId);
        List<string> GetAllUserCollateral();
        void AddUserCollateral(LoanUserCollateral userCollateral);
        void DeleteUserCollateral(LoanUserCollateral userCollateral);
        void UpdateUserCollateral(LoanUserCollateral userCollateral);
        LoanUserCollateral GetUserCollateralById(int id);
        //List<UserCollateral> ListUserCollaterals(string username);
    }
}
