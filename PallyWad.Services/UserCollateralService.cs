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
    public class UserCollateralService : IUserCollateralService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserCollateralRepository _userCollateralRepository;

        public UserCollateralService(IUserCollateralRepository userCollateralRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userCollateralRepository = userCollateralRepository;
        }
        public void AddUserCollateral(UserCollateral userCollateral)
        {
            _userCollateralRepository.Add(userCollateral);
            Save();
        }

        public void DeleteUserCollateral(UserCollateral userCollateral)
        {
            _userCollateralRepository?.Delete(userCollateral);
            Save();
        }

        public List<string> GetAllUserCollateral()
        {
            return ListAllUserCollateral().Select(u => u.name).ToList();
        }

        public UserCollateral GetUserCollateralById(int id)
        {
            //return ListAllUserCollateral().Where(u => u.Id == id).FirstOrDefault();
            var result = _userCollateralRepository.FindById(id);
            return result;
        }

        public List<UserCollateral> ListAllUserCollateral()
        {
            var result = _userCollateralRepository.FindAll().ToList();
            return result;
        }

        public List<UserCollateral> ListAllUserCollateral(string username)
        {
            var result = _userCollateralRepository.FindAll().Where(u => u.userId == username).ToList();
            return result;
        }

        public List<UserCollateral> ListAllUserCollateral(string username, string CollateralRefId)
        {
            var result = _userCollateralRepository.FindAll().Where(u => u.userId == username && u.colleteralId == CollateralRefId).ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserCollateral(UserCollateral userCollateral)
        {
            _userCollateralRepository.Update(userCollateral);
            Save();
        }
    }

    public interface IUserCollateralService
    {
        void Save();
        List<UserCollateral> ListAllUserCollateral();
        List<UserCollateral> ListAllUserCollateral(string username);
        List<UserCollateral> ListAllUserCollateral(string username, string CollateralRefId);
        List<string> GetAllUserCollateral();
        void AddUserCollateral(UserCollateral userCollateral);
        void DeleteUserCollateral(UserCollateral userCollateral);
        void UpdateUserCollateral(UserCollateral userCollateral);
        UserCollateral GetUserCollateralById(int id);
    }
}
