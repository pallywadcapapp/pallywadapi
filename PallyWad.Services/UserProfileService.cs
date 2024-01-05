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
    public class UserProfileService : BaseService, IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserProfileService(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.Add(userProfile);
            Save();
        }

        public UserProfile GetUserProfile(string id)
        {
            return ListUserProfiles().Where(u => u.AppIdentityUser.UserName == id).FirstOrDefault();
        }

        public UserProfile GetUserProfile(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserProfile> ListUserProfiles()
        {
            var result = _userProfileRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.Update(userProfile);
            Save();
        }
    }

    public interface IUserProfileService
    {
        List<UserProfile> ListUserProfiles();
        //List<UserProfile> ListUserProfiles(string memberId);
        UserProfile GetUserProfile(string id);
        UserProfile GetUserProfile(int id);
        void AddUserProfile(UserProfile userProfile);
        void Save();
        void UpdateUserProfile(UserProfile userProfile);
    }
}
