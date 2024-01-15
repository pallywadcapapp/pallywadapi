using PallyWad.Domain;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PallyWad.Services.Repository
{
    [TransientRegistration]
    public class LoanCollateralRepository : RepositoryBase<LoanCollateral, AppDbContext>, ILoanCollateralRepository
    {
        public LoanCollateralRepository(AppDbContext databaseFactory) : base(databaseFactory)
        {
        }

    }

    public interface ILoanCollateralRepository : IRepository<LoanCollateral>
    {

    }
}