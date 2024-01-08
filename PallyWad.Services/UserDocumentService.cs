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
    public class UserDocumentService : IUserDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDocumentRepository _userDocumentRepository;

        public UserDocumentService(IUserDocumentRepository userDocumentRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userDocumentRepository = userDocumentRepository;
        }
        public void AddUserDocument(UserDocument userDocument)
        {
            _userDocumentRepository.Add(userDocument);
            Save();
        }

        public void DeleteUserDocument(UserDocument userDocument)
        {
            _userDocumentRepository?.Delete(userDocument);
            Save();
        }

        public List<string> GetAllUserDocument()
        {
            return ListAllUserDocument().Select(u => u.name).ToList();
        }

        public UserDocument GetUserDocumentById(int id)
        {
            //return ListAllUserDocument().Where(u => u.Id == id).FirstOrDefault();
            var result = _userDocumentRepository.FindById(id);
            return result;
        }

        public List<UserDocument> ListAllUserDocument()
        {
            var result = _userDocumentRepository.FindAll().ToList();
            return result;
        }

        public List<UserDocument> ListAllUserDocument(string username)
        {
            var result = _userDocumentRepository.FindAll().Where(u=>u.userId == username).ToList();
            return result;
        }

        public List<UserDocument> ListAllUserDocument(string username, string documentRefId)
        {
            var result = _userDocumentRepository.FindAll().Where(u => u.userId == username && u.documentRefId == documentRefId).ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserDocument(UserDocument userDocument)
        {
            _userDocumentRepository.Update(userDocument);
            Save();
        }
    }

    public interface IUserDocumentService
    {
        void Save();
        List<UserDocument> ListAllUserDocument();
        List<UserDocument> ListAllUserDocument(string username);
        List<UserDocument> ListAllUserDocument(string username, string documentRefId);
        List<string> GetAllUserDocument();
        void AddUserDocument(UserDocument userDocument);
        void DeleteUserDocument(UserDocument userDocument);
        void UpdateUserDocument(UserDocument userDocument);
        UserDocument GetUserDocumentById(int id);
    }
}
