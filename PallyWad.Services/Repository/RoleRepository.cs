using Microsoft.AspNetCore.Identity;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Repository
{
    [TransientRegistration]
    public class RoleRepository : RepositoryBase<IdentityRole, AppIdentityDbContext>, IRoleRepository //RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(AppIdentityDbContext databaseFactory)
            : base(databaseFactory)
        {
        }

    }

    public interface IRoleRepository : IRepository<IdentityRole>
    {

    }
}
