

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebSurveying2017.Data.Infrastructure;

namespace WebSurveying2017.Service.Infrastructure
{
    public abstract class ServiceBase<T> : IService<T> where T : class
    {
        private readonly IRepository<T> repository;
        private readonly IUnitOfWork unitOfWork;

        public ServiceBase(IRepository<T> repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Implementation
        public virtual IEnumerable<T> GetAll()
        {
            var result = repository.GetAll();
            return result;
        }
        
        public virtual T Get(int id)
        {
            var result = repository.GetById(id);
            return result;
        }
        
        public virtual void Create(T entity)
        {
            repository.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            repository.Delete(entity);
        }

        public virtual void Update(T entity)
        {
            repository.Update(entity);
        }

        public void Update(T entity, T newEntity)
        {
            repository.Update(entity, newEntity);
        }
        public virtual void Delete(int id)
        {
            repository.Delete(repository.GetById(id));
        }

        public virtual void Save()
        {
            unitOfWork.Commit();
        }

        public virtual int GetCount()
        {
            return repository.GetCount();
        }

        public IEnumerable<T> GetForPage(int page, int size)
        {
            return repository.GetForPage(page, size);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return repository.GetMany(where);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
             repository.Delete(where);
        }

        public IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where)
        {
            return repository.GetManyAsNoTracking(where);
        }

        public void Attach(T entity)
        {
            repository.Attach(entity);
        }




        #endregion

    }
}