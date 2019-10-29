using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CQRSDemo.Data.EF.Repositories
{
    public interface IEntityRepository<T>
    {
        List<T> GetAll();

        List<T> FindAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        T GetById(object id);
        T Insert(T t);
        T Update(T t);
        bool Delete(object id);
        IQueryable<T> GetQueryable();

        void SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
