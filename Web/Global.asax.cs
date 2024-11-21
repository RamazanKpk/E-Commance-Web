using Buisness.Interface.Abstract.WebAbstract;
using Buisness.Interface.Service.BrandServices;
using Buisness.Interface.Service.CategoryServices;
using Buisness.Interface.Service.WebServices;
using Buisness.Interface.Service.WebServices.UserFavoriteProductServices;
using Buisness.Interface.Service.WebServices.UserInfoServices;
using DataModel.BrandViewModels;
using DataModel.CategoryViewModels;
using DataModel.ProductViewModels;
using DataModel.UserContactViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);



            var configre = new UnityContainer();
            configre.RegisterType<IAllWebService<ProductListModel>, ProductService>();
            configre.RegisterType<IAllWebService<BrandListModel>, BrandService>();
            configre.RegisterType<IAllWebService<CategoryListModel>, CategoryService>();
            configre.RegisterType<IUserInfoService<UserInfoListModel>, UserInfoService>();
            configre.RegisterType<IProductService, ProductService>();
            configre.RegisterType<IFavoriteProductService, UserFavoriteProductService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(configre));
        }
    }
}
