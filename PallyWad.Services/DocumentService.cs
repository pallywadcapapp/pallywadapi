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
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
        }
        public void AddDocument(Document document)
        {
            _documentRepository.Add(document);
            Save();
        }

        public void DeleteDocument(Document document)
        {
            _documentRepository?.Delete(document);
            Save();
        }

        public List<string> GetAllDocument()
        {
            return ListAllDocument().Select(u => u.name).ToList();
        }

        public Document GetDocumentByName(string name)
        {
            return ListAllDocument().Where(u => u.name == name).FirstOrDefault();
        }

        public List<Document> ListAllDocument()
        {
            var result = _documentRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateDocument(Document Document)
        {
            _documentRepository.Update(Document);
            Save();
        }
    }

    public interface IDocumentService
    {
        void Save();
        List<Document> ListAllDocument();
        List<string> GetAllDocument();
        void AddDocument(Document document);
        void DeleteDocument(Document document);
        void UpdateDocument(Document document);
        Document GetDocumentByName(string name);
    }
}
