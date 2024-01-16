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
    public class ProductRepository : RepositoryBase<Product, SetupDbContext>, IProductRepository
    {
        public ProductRepository(SetupDbContext databaseFactory) : base(databaseFactory)
        {
        }

    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}
