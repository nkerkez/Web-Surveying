using System;
using System.Collections.Generic;

using System.Linq;


using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using Z.EntityFramework.Plus;

namespace WebSurveying2017.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetRoots();
        IEnumerable<int> GetAllSub(int id, List<int> retVal);
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Category> GetRoots()
        {

            
            List<Category> roots =  dbSet.AsNoTracking().Where(c => c.ParentCategory == null).ToList();
            List<Category> retVal = new List<Category>();
            foreach (var root in roots)
            {
                this.PopulateChildren(root);
                retVal.Add(root);
            }
            return retVal;
         }


        private void PopulateChildren(Category parent)
        {
            parent.SubCategories = dbSet.AsNoTracking().Where(c => c.CategoryId == parent.Id).ToList();
            foreach (var sub in parent.SubCategories.ToList())
                this.PopulateChildren(sub);
        }
        
       
        public override Category GetById(int id)
        {
            
            return dbSet.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<int> GetAllSub(int id, List<int> retVal)
        {
            List<int> sub = base.dbSet.AsNoTracking().Where(c => c.CategoryId == id).Select(c => c.Id).ToList();
            retVal.AddRange(sub);
            sub.ForEach(c =>
            {
                this.GetAllSub(c, retVal);
            });
            return retVal;
        }
    }
}
