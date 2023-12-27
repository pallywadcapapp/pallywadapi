using Dapper;
using Microsoft.AspNetCore.Http;
using PallyWad.Domain;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    [TransientRegistration]
    public class LoanTransService : BaseService, ILoanTransService
    {
        private readonly ILoanTransRepository _loanTransRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoanTransService(ILoanTransRepository loanTransRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _loanTransRepository = loanTransRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddLoanTrans(LoanTrans loanTrans)
        {
            
            _loanTransRepository.Add(loanTrans);
            Save();
        }

        public List<LoanTrans> GetAllLoanTrans()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _loanTransRepository.FindAll().ToList();
            return result;
            //return _LoanTransRepository.Query<LoanTranss>("ListAllLoanTrans", parameters);
        }

        public List<LoanTrans> ListLoanHistory(string memberId)
        {
            return _loanTransRepository.FindAll().Where(x => x.memberid == memberId)
            //.Where(u=>u.Repay == 1)
            //.OrderByDescending(x=>x.Transdate)
            .ToList();
        }

        public List<LoanTransExt> ListOverdueLoans(DateTime date)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@transdate", date);
            return _loanTransRepository.Query<LoanTransExt>("ListOverdueLoans", parameters);
        }

        public List<LoanTransExt> ListLoansExpiration(DateTime date, int duration)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@transdate", date);
            parameters.Add("@duration", duration);
            return _loanTransRepository.Query<LoanTransExt>("ListLoanExpirationForecast", parameters);
        }

        public List<LoanTrans> ListLoanDeductibleTrans()
        {
            return _loanTransRepository.FindAll()
            .Where(u => u.repay == 1)
            .OrderByDescending(x => x.transdate).ToList();
        }

        public LoanTrans GetLoanDeductibleTransByMemberId(string id)
        {
            return _loanTransRepository.FindAll().Where(x => x.memberid == id)
            .Where(u => u.repay == 1)
            .OrderByDescending(x => x.transdate).FirstOrDefault();
        }

        public List<LoanTrans> ListLoanDeductibleTransByMemberId(string id)
        {
            return _loanTransRepository.FindAll().Where(x => x.memberid == id)
            .Where(u => u.repay == 1 && u.repaystartdate <= DateTime.Now)
            .OrderBy(x => x.repayOrder)
            //.OrderByDescending(x=>x.Repayamount)
            .OrderBy(x => x.transdate)

            .ToList();
        }

        public LoanTrans GetLoanTrans(string id)
        {
            return _loanTransRepository.Get(x => x.loancode == id);
        }

        public LoanTrans GetLoanTransByRef(string id)
        {
          
            return _loanTransRepository.Get(x => x.loanrefnumber == id);
        }

        public LoanTrans GetLoanTransByMemberId(string id)
        {
            return _loanTransRepository.FindAll().Where(x => x.memberid == id).OrderByDescending(x => x.transdate).FirstOrDefault();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateLoanTrans(LoanTrans loanTrans)
        {
            
            _loanTransRepository.Update(loanTrans);
            Save();
        }
    }

    public interface ILoanTransService
    {
        void AddLoanTrans(LoanTrans LoanTrans);
        List<LoanTrans> GetAllLoanTrans();
        LoanTrans GetLoanTransByMemberId(string id);
        List<LoanTrans> ListLoanDeductibleTransByMemberId(string id);
        List<LoanTrans> ListLoanHistory(string memberId);
        LoanTrans GetLoanTrans(string id);
        LoanTrans GetLoanTransByRef(string id);
        void Save();
        void UpdateLoanTrans(LoanTrans LoanTrans);
        LoanTrans GetLoanDeductibleTransByMemberId(string id);
        List<LoanTrans> ListLoanDeductibleTrans();
        List<LoanTransExt> ListOverdueLoans(DateTime date);
        List<LoanTransExt> ListLoansExpiration(DateTime date, int duration);
    }
}
