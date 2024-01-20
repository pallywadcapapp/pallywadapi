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
    public class ChargesService : BaseService, IChargesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChargesRepository _chargesRepository;
        public ChargesService(IUnitOfWork unitOfWork, IChargesRepository chargesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _chargesRepository = chargesRepository;
        }
        public void AddCharge(Charges charge)
        {
            _chargesRepository.Add(charge);
            Save();
        }

        public List<string> GetAllCharges()
        {
            var result = _chargesRepository.FindAll().Select(u=>u.shortname).ToList();
            return result;
        }

        public Charges GetCharges(string id)
        {
            return _chargesRepository.Get(x => x.chargecode == id);
        }

        public List<Charges> ListAllCharges()
        {
            var result = _chargesRepository.FindAll().ToList();
            return result;
        }

        public IEnumerable<Charges> ListCharges()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _chargesRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCharge(Charges charge)
        {
            _chargesRepository.Update(charge);
        }
    }

    public interface IChargesService
    {
        IEnumerable<Charges> ListCharges();
        Charges GetCharges(string id);
        void AddCharge(Charges charge);
        void Save();
        void UpdateCharge(Charges charge);
        List<string> GetAllCharges();
        List<Charges> ListAllCharges();
    }
}
