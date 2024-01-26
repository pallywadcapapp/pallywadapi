using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
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
    public class LoanRequestService : BaseService, ILoanRequestService
    {
        private readonly ILoanRequestRepository _loanRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoanRequestService(ILoanRequestRepository loanRequestRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _loanRequestRepository = loanRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddLoanRequest(LoanRequest loanRequest)
        {
            _loanRequestRepository.Add(loanRequest);
            Save();
        }

        public List<LoanRequest> GetAllLoanRequests()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _loanRequestRepository.FindAll().ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }

        public List<LoanRequest> GetAllPendingLoanRequests()
        {

            var result = _loanRequestRepository.FindAll()
                .Where(x =>  x.status == "Pending")
                .Include(u=>u.loanUserDocuments)
                .ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }

        public List<LoanRequest> GetAllCollaterizedLoanRequests()
        {

            var result = _loanRequestRepository.FindAll().Where(x => x.status == "Collaterized").ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }

        public List<LoanRequest> GetAllApprovedLoanRequests()
        {

            var result = _loanRequestRepository.FindAll().Where(x => x.status == "Approved").ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }



        public List<LoanRequest> GetAllProcessedLoanRequests()
        {
            var parameters = new DynamicParameters();

            var result = _loanRequestRepository.FindAll().Where(x => x.status == "Approved" && x.processState == "Processed").ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }

        public List<LoanRequest> GetAllUnProcessedLoanRequests()
        {
            var parameters = new DynamicParameters();

            var result = _loanRequestRepository.FindAll().Where(x => x.status == "Approved" && x.processState != "Processed").ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }
        public List<LoanRequest> GetAllDeclinedLoanRequests()
        {
            var parameters = new DynamicParameters();

            var result = _loanRequestRepository.FindAll().Where(x => x.status == "Declined").ToList();
            return result;
            //return _LoanRequestRepository.Query<LoanRequest>("ListAllLoanRequest", parameters);
        }

        public LoanRequest GetLoanRequest(string id)
        {
            return _loanRequestRepository.Get(x => x.loanId == id);
        }

        public LoanRequest GetLoanRequest(int id)
        {
            return _loanRequestRepository.Get(x => x.Id == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateLoanRequest(LoanRequest loanRequest)
        {
            _loanRequestRepository.Update(loanRequest);
            Save();
        }

        public List<LoanRequest> GetLoanRequests(string memberid)
        {
            var result = _loanRequestRepository.FindAll().Where(u=> u.memberId == memberid).ToList();
            return result;
        }
    }

    public interface ILoanRequestService
    {
        void AddLoanRequest(LoanRequest LoanRequest);
        List<LoanRequest> GetAllLoanRequests();
        LoanRequest GetLoanRequest(string id);
        LoanRequest GetLoanRequest(int id);
        List<LoanRequest> GetLoanRequests(string memberid);
        List<LoanRequest> GetAllApprovedLoanRequests();
        List<LoanRequest> GetAllCollaterizedLoanRequests();
        List<LoanRequest> GetAllDeclinedLoanRequests();
        List<LoanRequest> GetAllPendingLoanRequests();
        List<LoanRequest> GetAllProcessedLoanRequests();
        List<LoanRequest> GetAllUnProcessedLoanRequests();
        void Save();
        void UpdateLoanRequest(LoanRequest LoanRequest);
    }
}
