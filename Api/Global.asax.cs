using Autofac;
using Autofac.Integration.WebApi;
using Buisness.Interface.Abstract.ApiAbstract;
using Buisness.Interface.Service.ApiServices;
using DataModel.BrandViewModels;
using DataModel.CategoryViewModels;
using DataModel.ProductViewModels;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Api
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            config.EnableCors();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterType<ProductApiService>().As<IAllApiServices<ProductListModel>>();
            builder.RegisterType<ProductApiService>().As<IProductApiService<ProductListModel>>();
            builder.RegisterType<CategoryApiService>().As<IAllApiServices<CategoryListModel>>();
            builder.RegisterType<BrandApiService>().As<IAllApiServices<BrandListModel>>();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}