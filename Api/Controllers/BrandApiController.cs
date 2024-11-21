using Buisness.Interface.Abstract.ApiAbstract;
using DataModel.BrandViewModels;
using DataModel.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class BrandApiController : ApiController
    {
        private IAllApiServices<BrandListModel> _allApiServices;
        public BrandApiController(IAllApiServices<BrandListModel> allApiServices) 
        {
            _allApiServices = allApiServices;
        }
        [HttpGet]
        [Route("api/BrandApi/Brnads")]
        public IHttpActionResult Categories()
        {
            var result = _allApiServices.GetList();
            return Json(result);
        }
    }
}
