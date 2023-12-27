using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Paginate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace PallyWad.Services.Generics
{
    public abstract class RepositoryBase<T, TContext> where T : class where TContext : DbContext
    {
        protected readonly DbSet<T> _dbSet;
        //protected AppIdentityDbContext RepositoryContext { get; set; }
        protected TContext RepositoryContext { get; set; }

        private readonly IDbConnection _connection;
        public RepositoryBase(TContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
            _dbSet = RepositoryContext.Set<T>();
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(new DirectoryInfo(Environment.CurrentDirectory).FullName)//System.AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("Default");

            _connection = new SqlConnection(connstr);

        }

        /*public virtual AppTenantInfo GetTenant(string tenantUrl)
        {
            //return this.RepositoryContext.TenantInfo.Where(u => u.TenantUrl == tenantUrl).FirstOrDefault();
            //return null;
            return _connection.Query<AppTenantInfo>("GetTenants", null, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }*/
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public virtual T FindById(long id)
        {
            return this.RepositoryContext.Set<T>().Find(id);
        }

        public virtual T FindById(string id)
        {
            return this.RepositoryContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.RepositoryContext.Set<T>().ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return this.RepositoryContext.Set<T>().Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = this.RepositoryContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }
        //public virtual IQueryable<T> Query(string sql, params object[] parameters) => this.RepositoryContext.Set<T>().FromSql(sql, parameters);  for .netcore 2.2

        public virtual IQueryable<T> Query(string sql, params object[] parameters) => this.RepositoryContext.Set<T>().FromSqlInterpolated($"{sql} {parameters}");

        public virtual List<T> Query<T>(string storedProcName)
        {
            return Query<T>(storedProcName, null);
        }

        public virtual List<T> Query<T>(string storedProcName, object parameters)
        {
            return _connection.Query<T>(storedProcName, parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        public virtual List<T> QueryTableView<T>(string query)
        {
            return _connection.Query<T>(query).ToList();
        }
        public T Search(params object[] keyValues) => this.RepositoryContext.Set<T>().Find(keyValues);
        public T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = this.RepositoryContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).FirstOrDefault();
            return query.FirstOrDefault();
        }

        public IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int index = 0,
           int size = 20, bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }


        public IPaginate<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0, int size = 20, bool disableTracking = true) where TResult : class
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).Select(selector).ToPaginate(index, size)
                : query.Select(selector).ToPaginate(index, size);
        }

        public void Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Add(params T[] entities)
        {
            this.RepositoryContext.Set<T>().AddRange(entities);
        }
        public void Add(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }
        public void Update(params T[] entities)
        {
            this.RepositoryContext.Set<T>().UpdateRange(entities);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (var dbObj in entities.Select(entity => this.RepositoryContext.Entry(entity)))
            {
                dbObj.State = EntityState.Modified;
            }
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void Delete(int id)
        {
            var entity = this.RepositoryContext.Set<T>().Find(id);
            if (entity == null) return;

            Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var dbObj in entities.Select(entity => this.RepositoryContext.Entry(entity)))
            {
                dbObj.State = EntityState.Deleted;
            }
        }

        public void Delete(IEnumerable<int> ids)
        {
            var entities = ids.Select(id => this.RepositoryContext.Set<T>().Find(id)).ToList();
            Delete(entities);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = this.RepositoryContext.Set<T>().Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                this.RepositoryContext.Set<T>().Remove(obj);
        }
        public void Delete(object id)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = this.RepositoryContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                this.RepositoryContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = this.RepositoryContext.Set<T>().Find(id);
                if (entity != null) Delete(entity);
            }
        }

        public void Delete(params T[] entities)
        {
            this.RepositoryContext.Set<T>().RemoveRange(entities);
        }
        public void Dispose()
        {
            this.RepositoryContext?.Dispose();
        }
    }
}
