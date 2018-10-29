using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Update(T entity, T newEntity);
        void Delete(Expression<Func<T, bool>> where);
        void Delete(T entity);
        void Attach(T entity);
        int GetCount();

        T GetById(int id);

        IEnumerable<T> GetForPage(int page, int size);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where);
        IEnumerable<T> GetManyWithInclude(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
    }
}
