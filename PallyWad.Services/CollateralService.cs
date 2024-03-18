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
    public partial class CollateralService : ICollateralService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICollateralRepository _collateralRepository;

        public CollateralService(ICollateralRepository collateralRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _collateralRepository = collateralRepository;
        }
        public void AddCollateral(Collateral collateral)
        {
            _collateralRepository.Add(collateral);
            Save();
        }

        public void DeleteCollateral(Collateral collateral)
        {
            _collateralRepository?.Delete(collateral);
            Save();
        }

        public List<string> GetAllCollateral()
        {
            return ListAllCollateral().Select(u => u.name).ToList();
        }

        public Collateral GetCollateralById(int id)
        {
            return ListAllCollateral().Where(u => u.Id == id).FirstOrDefault();
        }

        public Collateral GetCollateralByName(string name)
        {
            return ListAllCollateral().Where(u => u.name == name).FirstOrDefault();
        }

        public List<Collateral> ListAllCollateral()
        {
            var result = _collateralRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCollateral(Collateral collateral)
        {
            _collateralRepository.Update(collateral);
            Save();
        }
    }

    public interface ICollateralService
    {
        void Save();
        List<Collateral> ListAllCollateral();
        List<string> GetAllCollateral();
        void AddCollateral(Collateral Collateral);
        void DeleteCollateral(Collateral Collateral);
        void UpdateCollateral(Collateral Collateral);
        Collateral GetCollateralByName(string name);
        Collateral GetCollateralById(int id);
    }
}
