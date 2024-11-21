using Buisness.Interface.Abstract.WebAbstract;
using DataAccess;
using DataModel.BrandViewModels;
using DataModel.CategoryViewModels;
using DataModel.ProductViewModels;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private IAllWebService<ProductListModel> _productAllWebServices;
        private IAllWebService<CategoryListModel> _categoryAllWebServices;
        private IAllWebService<BrandListModel> _brandAllWebServices;
        private IProductService _productService;
        public ProductController(IAllWebService<ProductListModel> productAllWebServices,
            IAllWebService<CategoryListModel> categoryAllWebServices,
            IAllWebService<BrandListModel> brandAllWebServices,
            IProductService productService)
        {
            _brandAllWebServices = brandAllWebServices;
            _productService = productService;
            _productAllWebServices = productAllWebServices;
            _categoryAllWebServices = categoryAllWebServices;
        }
        public ProductController() { }
        // GET: Product
        [HttpGet]
        public ActionResult Shop(int? Id)
        {
            var categories = _categoryAllWebServices.GetApiToWeb();
            var category = categories.Where(c => c.Id == Id).FirstOrDefault();
            ViewBag.Category = categories;
            ViewBag.Brand = _brandAllWebServices.GetApiToWeb();
            var products = _productAllWebServices.GetApiToWeb();
            List<ProductListModel> result;
            
            if (Id != null)
            {
                var existProduct = products.Where(c=> c.CategoryId == Id).ToList();
                if (existProduct.Count >0)
                {
                    result =existProduct;
                }
                else
                {
                    result = _productService.PostProductByCategory(category);
                }
            }
            else
            {
               result = products ;
            }
            return View(result);
            
        }
        [HttpGet]
        public ActionResult ProductDetail(int Id)
        {
            var products = _productAllWebServices.GetApiToWeb();
            var result = products.Where(x => x.Id == Id).FirstOrDefault();
            var brands = _brandAllWebServices.GetApiToWeb();
            ViewBag.Brand = brands;
            ViewBag.Category = _categoryAllWebServices.GetApiToWeb();
            return View(result);
        }
        
    }
}