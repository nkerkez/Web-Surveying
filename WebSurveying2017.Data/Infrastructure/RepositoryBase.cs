using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Infrastructure
{
    public class RepositoryBase<T> : IRepository<T> where T : class, IModelBase
    {

        private WebSurveyingContext context;
        protected readonly DbSet<T> dbSet;
        private IDbFactory _dbFactory;


        public WebSurveyingContext _context
        {
            get
            {
                return context ?? (context = _dbFactory.Init());
            }
        }

        public RepositoryBase(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
            dbSet = _context.Set<T>();

        }


        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        public virtual IEnumerable<T> GetForPage(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                size = 1;
                page = 10;
            }

            return dbSet.OrderBy(x => x.Id).Skip((page - 1) * size).Take(size);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = dbSet.Where(where);

            foreach (var obj in objects)
            {
                dbSet.Remove(obj);
            }
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Update(T entity, T newEntity)
        {
            _context.Entry(entity).CurrentValues.SetValues(newEntity);

        }

        public virtual void Update(T entity)
        {

            T local = dbSet.Local.SingleOrDefault(x => x.Id == entity.Id);
            if (local == null)
            {
                dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Unchanged;
            }
            else
            {
                _context.Entry(local).State = EntityState.Unchanged;
            }


            _context.Entry(entity).State = EntityState.Modified;

        }

       

        

        public IEnumerable<T> GetManyWithInclude(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet.AsNoTracking().Where(where).AsQueryable();

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        }

        

        public int GetCount()
        {
            return dbSet.Count();
        }

        public IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where)
        {
            var a = dbSet.AsNoTracking().Where(where);
            var b = dbSet.Local;
            var c = _context.ChangeTracker.Entries();
            
            return dbSet.AsNoTracking().Where(where);
        }

        public void Attach(T entity)
        {
            dbSet.Attach(entity);
        }
    }
}
