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
    public class GlAccountTier3Repository : RepositoryBase<GLAccountC, AccountDbContext>, IGlAccountTier3Repository
    {
        public GlAccountTier3Repository(AccountDbContext databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IGlAccountTier3Repository : IRepository<GLAccountC>
    {

    }
}
