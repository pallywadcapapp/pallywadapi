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
    public class LoanSetupRepository : RepositoryBase<LoanSetup, SetupDbContext>, ILoanSetupRepository
    {
        public LoanSetupRepository(SetupDbContext databaseFactory) : base(databaseFactory)
        {
        }

    }

    public interface ILoanSetupRepository : IRepository<LoanSetup>
    {

    }
}
