using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        WebSurveyingContext dbContext;

        public WebSurveyingContext Init()
        {
            return dbContext ?? (dbContext = new WebSurveyingContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

    }
}
