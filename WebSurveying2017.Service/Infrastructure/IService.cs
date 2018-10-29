using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Service.Infrastructure
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetForPage(int page, int size);
        T Get(int id);
        void Create(T entity);
        void Save();
        void Delete(int id);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void Update(T entity);
        void Attach(T entity);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where);
        void Update(T entity, T newEntity);
        int GetCount();
    }
}
