﻿using PallyWad.Domain;
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
    public class LoanRepaymentRepository : RepositoryBase<LoanRepayment, AppDbContext>, ILoanRepaymentRepository
    {
        public LoanRepaymentRepository(AppDbContext databaseFactory) : base(databaseFactory)
        {
        }

    }

    public interface ILoanRepaymentRepository : IRepository<LoanRepayment>
    {

    }
}
