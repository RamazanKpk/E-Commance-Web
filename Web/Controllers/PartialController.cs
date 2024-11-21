using Buisness.Interface.Abstract.WebAbstract;
using DataModel.CategoryViewModels;
using DataModel.ShopCartModels;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        private readonly IFavoriteProductService _favoriteProductService;
        public PartialController(IFavoriteProductService favoriteProductService) 
        {
            _favoriteProductService = favoriteProductService;
        }
        public PartialController() { }
        public PartialViewResult Header()
        {
            var categories = GetCategory();
            var result = categories.Where(x => x.ParentId == null).ToList();
            var cart = Session["Cart"] as Cart ?? new Cart();
            var user = Session["User"] as User;
            if(user != null)
            {
                ViewBag.FavoriteProduct = _favoriteProductService.GetByuserId(user.Id).Count();
            }
            ViewBag.CartItem = cart.Items.Count;
            return PartialView("Header", categories);
        }
        public List<CategoryListModel> GetCategory()
        {
            List<CategoryListModel> categories = new List<CategoryListModel>();
            string api = ConfigurationManager.AppSettings["GetCategoryEndPoint"].ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    categories = JsonConvert.DeserializeObject<List<CategoryListModel>>(json);
                }
            }
            return categories;
        }
        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            // Sepetteki ürün sayısını hesapla
            var cart = Session["Cart"] as Cart ?? new Cart();
            int cartItemCount = cart.Items.Count;
            return Json(new { count = cartItemCount }, JsonRequestBehavior.AllowGet);
        }
    }
}