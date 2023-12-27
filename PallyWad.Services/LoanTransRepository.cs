using PallyWad.Domain;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    [TransientRegistration]
    public class LoanTransRepository : RepositoryBase<LoanTrans, AppDbContext>, ILoanTransRepository
    {
        public LoanTransRepository(AppDbContext databaseFactory) : base(databaseFactory)
        {
        }

    }

    public interface ILoanTransRepository : IRepository<LoanTrans>
    {

    }
}
