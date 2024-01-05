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
    public class AccTypeService : IAccTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccTypeRepository _accTypeRepository;

        public AccTypeService(IAccTypeRepository accTypeRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _accTypeRepository = accTypeRepository;
        }
        public void AddAccType(AccType accType)
        {
            _accTypeRepository.Add(accType);
            Save();
        }

        public void DeleteAccType(AccType accType)
        {
            _accTypeRepository.Delete(accType);
            Save();
        }

        public List<string> GetAllAccType()
        {
            return ListAllAccType().Select(u => u.name).ToList();
        }

        public AccType GetAccTypeByName(string name)
        {
            return ListAllAccType().Where(u => u.name == name).FirstOrDefault();
        }

        public List<AccType> ListAllAccType()
        {
            var result = _accTypeRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateAccType(AccType accType)
        {
            _accTypeRepository.Update(accType);
            Save();
        }
    }

    public interface IAccTypeService
    {
        void Save();
        List<AccType> ListAllAccType();
        List<string> GetAllAccType();
        void AddAccType(AccType accType);
        void DeleteAccType(AccType accType);
        void UpdateAccType(AccType AccType);
        AccType GetAccTypeByName(string name);
    }
}
