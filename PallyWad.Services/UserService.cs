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
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddUser(AppIdentityUser user)
        {
            _userRepository.Add(user);
            Save();
        }

        public AppIdentityUser GetUser(string id)
        {
            return ListUsers().Where(u => u.UserName == id).FirstOrDefault();
        }

        public AppIdentityUser GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<AppIdentityUser> ListUsers()
        {
            var result = _userRepository.FindAll().ToList();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUser(AppIdentityUser user)
        {
            _userRepository.Update(user);
            Save();
        }
    }

    public interface IUserService
    {
        List<AppIdentityUser> ListUsers();
        //List<User> ListUsers(string memberId);
        AppIdentityUser GetUser(string id);
        AppIdentityUser GetUser(int id);
        void AddUser(AppIdentityUser user);
        void Save();
        void UpdateUser(AppIdentityUser user);
    }
}
