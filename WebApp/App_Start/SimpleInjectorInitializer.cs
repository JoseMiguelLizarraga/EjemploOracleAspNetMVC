[assembly: WebActivator.PostApplicationStartMethod(typeof(WebApp.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace WebApp.App_Start
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Mvc;
    using DataAccess;
    using Services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            // Inyeccion de dependencias

            //container.Register<Entities, DbContext>(Lifestyle.Scoped);

            container.Register<ICategoriaService, CategoriaService>(Lifestyle.Scoped);
            container.Register<IProductoService, ProductoService>(Lifestyle.Scoped);
        }
    }
}