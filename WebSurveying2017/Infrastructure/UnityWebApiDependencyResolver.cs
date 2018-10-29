using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;
namespace WebSurveying2017.Infrastructure
{
    public class UnityWebApiDependencyResolver : IDependencyResolver
    {
        private readonly Microsoft.Practices.Unity.IUnityContainer _container;

        public UnityWebApiDependencyResolver(Microsoft.Practices.Unity.IUnityContainer container)
        {
            this._container = container;
        }

        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            return new UnityWebApiDependencyResolver(child);
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}