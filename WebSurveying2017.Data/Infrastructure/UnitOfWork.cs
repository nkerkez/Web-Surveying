using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDbFactory _dbFactory;
        private WebSurveyingContext context;

        private WebSurveyingContext _context
        {
            get
            {
                return context ?? (context = _dbFactory.Init());
            }
        }

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public void Commit()
        {

            _context.Commit();
        }
    }
}

