using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using System.Web.Http;
using UnityDependencyResolver.Lib;

namespace WebSurveying2017
{
  public static class Bootstrapper
  {
    private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
    {
      var container = new UnityContainer();
      var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
      container.LoadConfiguration(section);
      return container;
    });

    public static IUnityContainer Initialize()
    {
      var container = GetConfiguredContainer();
      // DependencyResolver.SetResolver(new UnityMVCDependencyResolver(container));
      
      var resolver = new UnityWebApiDependencyResolver(GetConfiguredContainer());
      GlobalConfiguration.Configuration.DependencyResolver = resolver;

      return container;
    }

    public static IUnityContainer GetConfiguredContainer()
    {
      return container.Value;
    }
  }
}