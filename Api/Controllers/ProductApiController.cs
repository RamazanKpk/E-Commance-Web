using Buisness.Interface.Abstract.ApiAbstract;
using DataModel.CategoryViewModels;
using DataModel.ProductViewModels;
using System.Web.Http;

namespace Api.Controllers
{
    public class ProductApiController : ApiController
    {
        private readonly IAllApiServices<ProductListModel> _allServices;
        private readonly IProductApiService<ProductListModel> _productApiService;
        public ProductApiController(IAllApiServices<ProductListModel> allServices, IProductApiService<ProductListModel> productApiService)
        {
            _allServices = allServices;
            _productApiService = productApiService; 
        }
        [HttpGet]
        [Route("api/ProductApi/Products")]
        public IHttpActionResult Products()
        {
            var result = _allServices.GetList();
            return Json(result);
        }
        [HttpPost]
        [Route("api/ProductApi/ProductByCategory")]
        public IHttpActionResult PorductByCategory([FromBody] CategoryListModel model)
        {
            var result = _productApiService.GetProductByCategory(model);
            return Json(result);
        }
    }
}
