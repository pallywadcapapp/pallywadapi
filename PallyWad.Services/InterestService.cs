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
    public class InterestService : BaseService, IInterestService
    {
        private readonly IInterestRepository _interestRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InterestService(IInterestRepository interestRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _interestRepository = interestRepository;
            _unitOfWork = unitOfWork;
        }

        public void Addinterest(Interest interest)
        {
            _interestRepository.Add(interest);
            Save();
        }

        public List<string> GetAllInterests()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _interestRepository.FindAll().Select(u=>u.shortname).ToList();
            return result;
            //return _interestRepository.Query<Tblinterest>("ListAllinterests", parameters);
        }

        public List<Interest> ListAllInterests()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _interestRepository.FindAll().ToList();
            return result;
            //return _interestRepository.Query<Tblinterest>("ListAllinterests", parameters);
        }

        public Interest Getinterest(string id)
        {
            return _interestRepository.Get(x => x.interestcode == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Updateinterest(Interest interest)
        {
            _interestRepository.Update(interest);
            Save();
        }
    }

    public interface IInterestService
    {
        void Addinterest(Interest interest);
        List<string> GetAllInterests();
        Interest Getinterest(string id);
        List<Interest> ListAllInterests();
        void Save();
        void Updateinterest(Interest interest);
    }
}
