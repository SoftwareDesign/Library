using System.Web.Http;
using Microsoft.Practices.Unity;
using MiniOa.Books;
using MiniOa.BookService;
using MiniOa.Utilities;

namespace MiniOa
{
    public class DependencyInjectionConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IBookList, BookList>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        } 
    }
}