using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PallyWad.Domain;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using PallyWad.Services.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    [TransientRegistration]
    public class RoleService : BaseService, IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        public RoleService(IUserRepository userRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository
        , IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public void CreateRole(IdentityRole role)
        {
            _roleRepository.Add(role);
            Save();
        }
        public void DeleteRole(IdentityRole role)
        {
            _roleRepository.Delete(role);
            Save();
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            var role = _roleRepository.GetAll();
            return role;
        }

        public IEnumerable<AppsRole> GetAllUsersInRoles()
        {
            //IdentityRole
            var parameters = new DynamicParameters();
            var role = _roleRepository.Query<AppsRole>("GetAllUsersInRole", parameters);// GetAll().Where(u => u.tenantId == tenantId);
            return role;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateRole(IdentityRole role)
        {
            _roleRepository.Update(role);
            Save();
        }

        public AppIdentityUser GetUserFromEmail(string email)
        {
            var user = _userRepository.Get(u => u.UserName == email);
            return user;
        }

        public void DeleteUserFromRole(string username, string role)
        {
            var user = _userRepository.Get(u => u.UserName == username);
            if (user.Id != null)
            {
                //foreach (var roleName in deleteList)
                //{
                //IdentityResult deletionResult = await UserManager.RemoveFromRoleAsync(userId, roleName);
                //}
            }
        }
    }

    public interface IRoleService
    {
        IEnumerable<IdentityRole> GetAllRoles();
        //IEnumerable<Role> GetAllUserRoles(string username);
        //IEnumerable<Role> GetUserRoles(string userId);
        IEnumerable<AppsRole> GetAllUsersInRoles(); //IdentityRole
        void CreateRole(IdentityRole role);
        AppIdentityUser GetUserFromEmail(string email);

        void DeleteRole(IdentityRole role);
        void UpdateRole(IdentityRole role);

        //Role GetRole(int id);

        //bool HasRole(string username,string role);

        void Save();
        void DeleteUserFromRole(string user, string role);
    }
}
