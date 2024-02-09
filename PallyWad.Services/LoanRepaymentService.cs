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
    public class LoanRepaymentService : BaseService, ILoanRepaymentService
    {
        private readonly ILoanRepaymentRepository _loanRepaymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoanRepaymentService(ILoanRepaymentRepository loanRepaymentRepository, IUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor)
        {
            _loanRepaymentRepository = loanRepaymentRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddLoanRepayment(LoanRepayment loanRepayment)
        {
            _loanRepaymentRepository.Add(loanRepayment);
            Save();
        }

        public List<LoanRepayment> GetAllLoanRepayments()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _loanRepaymentRepository.FindAll().ToList();
            return result;
        }

        public LoanRepayment GetLoanRepayment(string id)
        {
            var _result = _loanRepaymentRepository.FindAll().Where(u => u.loanrefnumber == id)
                .OrderByDescending(u => u.Id);
                //.OrderByDescending(u => u.transdate);
            var result = _result.FirstOrDefault();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateLoanRepayment(LoanRepayment loanRepayment)
        {
            _loanRepaymentRepository.Update(loanRepayment);
            Save();
        }
    }

    public interface ILoanRepaymentService
    {
        void AddLoanRepayment(LoanRepayment LoanRepayment);
        List<LoanRepayment> GetAllLoanRepayments();
        LoanRepayment GetLoanRepayment(string id);
        void Save();
        void UpdateLoanRepayment(LoanRepayment LoanRepayment);
    }
}
