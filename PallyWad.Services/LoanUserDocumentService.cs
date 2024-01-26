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
    public class LoanUserDocumentService : ILoanUserDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoanUserDocumentRepository _userDocumentRepository;

        public LoanUserDocumentService(ILoanUserDocumentRepository userDocumentRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userDocumentRepository = userDocumentRepository;
        }
        public void AddUserDocument(LoanUserDocument userDocument)
        {
            _userDocumentRepository.Add(userDocument);
            Save();
        }

        public void DeleteUserDocument(LoanUserDocument userDocument)
        {
            _userDocumentRepository?.Delete(userDocument);
            Save();
        }

        public List<string> GetAllUserDocument()
        {
            return ListAllUserDocument().Select(u => u.userDocumentlId).ToList();
        }

        public LoanUserDocument GetUserDocumentById(int id)
        {
            //return ListAllUserDocument().Where(u => u.Id == id).FirstOrDefault();
            var result = _userDocumentRepository.FindById(id);
            return result;
        }

        public List<LoanUserDocument> ListAllUserDocument()
        {
            var result = _userDocumentRepository.FindAll().ToList();
            return result;
        }

        public List<LoanUserDocument> ListAllUserDocument(int loanRequestId)
        {
            var result = _userDocumentRepository.FindAll().Where(u => u.loanRequestId == loanRequestId).ToList();
            return result;
        }

        public List<LoanUserDocument> ListAllUserDocument(int loanRequestId, string documentRefId)
        {
            var result = _userDocumentRepository.FindAll().Where(u => u.loanRequestId == loanRequestId && u.userDocumentlId == documentRefId).ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserDocument(LoanUserDocument userDocument)
        {
            _userDocumentRepository.Update(userDocument);
            Save();
        }
    }

    public interface ILoanUserDocumentService
    {
        void Save();
        List<LoanUserDocument> ListAllUserDocument();
        List<LoanUserDocument> ListAllUserDocument(int loanRequestId);
        List<LoanUserDocument> ListAllUserDocument(int loanRequestId, string documentRefId);
        List<string> GetAllUserDocument();
        void AddUserDocument(LoanUserDocument userDocument);
        void DeleteUserDocument(LoanUserDocument userDocument);
        void UpdateUserDocument(LoanUserDocument userDocument);
        LoanUserDocument GetUserDocumentById(int id);
        //List<UserDocument> ListUserDocuments(string username);
    }
}
