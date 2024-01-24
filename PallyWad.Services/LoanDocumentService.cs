using Dapper;
using Microsoft.AspNetCore.Http;
using PallyWad.Domain;
using PallyWad.Services.Generics;
using PallyWad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    public class LoanDocumentService : BaseService, ILoanDocumentService
    {
        private readonly ILoanDocumentRepository _loanDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoanDocumentService(ILoanDocumentRepository loanDocumentRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _loanDocumentRepository = loanDocumentRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddLoanDocument(LoanDocument loanDocument)
        {

            _loanDocumentRepository.Add(loanDocument);
            Save();
        }

        public List<LoanDocument> GetAllLoanDocument()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _loanDocumentRepository.FindAll().ToList();
            return result;
            //return _LoanDocumentRepository.Query<LoanDocumentS>("ListAllLoanDocument", parameters);
        }

        public LoanDocument GetLoanDocument(int id)
        {
            var result = _loanDocumentRepository.FindAll().Where(u => u.Id == id).FirstOrDefault();
            return result;
        }

        public List<LoanDocument> ListDocument(string documentId)
        {
            var result = _loanDocumentRepository.FindAll().Where(u => u.documentId == documentId).ToList();
            return result;
        }

        //public List<LoanDocument> ListLoan(string loanId)
        //{
        //    var result = _loanDocumentRepository.FindAll().Where(u => u.loanId == loanId).ToList();
        //    return result;
        //}

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateLoanDocument(LoanDocument loanDocument)
        {

            _loanDocumentRepository.Update(loanDocument);
            Save();
        }
    }

    public interface ILoanDocumentService
    {
        void AddLoanDocument(LoanDocument LoanDocument);
        //List<LoanDocument> ListLoan(string loanId);
        List<LoanDocument> ListDocument(string documentId);
        LoanDocument GetLoanDocument(int id);
        void Save();
        void UpdateLoanDocument(LoanDocument LoanDocument);
    }
}
