using PallyWad.Domain;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Repository
{
    [TransientRegistration]
    public class GlAccountTransRepository : RepositoryBase<GL, AppDbContext>, IGlAccountTransRepository
    {
        public GlAccountTransRepository(AppDbContext databaseFactory) : base(databaseFactory)
        {
        }

    }

    public interface IGlAccountTransRepository : IRepository<GL>
    {

    }
}
