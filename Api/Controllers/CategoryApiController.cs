using Buisness.Interface.Abstract.ApiAbstract;
using DataModel.CategoryViewModels;
using DataModel.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class CategoryApiController : ApiController
    {
        private readonly IAllApiServices<CategoryListModel> _allServices;
        public CategoryApiController(IAllApiServices<CategoryListModel> allServices)
        {
            _allServices = allServices;
        }
        [HttpGet]
        [Route("api/CategoryApi/Categories")]
        public IHttpActionResult Categories()
        {
            var result = _allServices.GetList();
            return Json(result);
        }

    }
}
