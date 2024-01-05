//using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using PallyWad.Domain;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    public class CacheService : ICollateralCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private IDatabase _db;
        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }
        public IEnumerable<Collateral> GetAll()
        {
            throw new NotImplementedException();
        }

        public Collateral GetData(string key, string id)
        {
            throw new NotImplementedException();
        }

        public bool RemovePost(string key, string id)
        {
            throw new NotImplementedException();
        }

        public Collateral SetPost(string key, Collateral collateral)
        {
            throw new NotImplementedException();
        }
    }
    public interface ICollateralCacheService
    {
        Collateral GetData(string key, string id);
        IEnumerable<Collateral> GetAll();
        Collateral SetPost(string key, Collateral collateral);
        bool RemovePost(string key, string id);
    }
}
