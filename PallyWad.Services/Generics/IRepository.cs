using Microsoft.EntityFrameworkCore.Query;
using PallyWad.Services.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Generics
{
    public interface IRepository<T> : IDisposable where T : class//, IEntity
    {
        //AppTenantInfo GetTenant(string tenantUrl);
        IQueryable<T> FindAll();
        T FindById(string id);
        T FindById(long id);
        IEnumerable<T> GetAll();
        IQueryable<T> Query(string sql, params object[] parameters);
        List<T> Query<T>(string storedProcName);
        List<T> Query<T>(string storedProcName, object parameters);
        List<T> QueryTableView<T>(string query);

        T Search(params object[] keyValues);
        T Get(Expression<Func<T, bool>> where);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          string includeProperties = "");

        T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true);

        IPaginate<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true) where TResult : class;

        void Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);


        void Delete(T entity);
        void Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);


        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
    }
}
