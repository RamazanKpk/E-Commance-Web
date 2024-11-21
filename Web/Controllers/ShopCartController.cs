using Buisness.Interface.Abstract.WebAbstract;
using DataModel.ProductViewModels;
using DataModel.ShopCartModels;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ShopCartController : Controller
    {

       private readonly IAllWebService<ProductListModel> _allWebServices;
        public ShopCartController(IAllWebService<ProductListModel> allWebServices)
        {
            _allWebServices = allWebServices;
        }
        public ShopCartController() { }
        // GET: ShopCart
        public ActionResult ShopCart()
        {
            var cart  = Session["Cart"] as Cart ?? new Cart();
            return View(cart);
        }
        [HttpPost]
        public ActionResult AddCart(int Id)
        {
            var products = _allWebServices.GetApiToWeb();
            var product = products.FirstOrDefault(x => x.Id == Id);
            
            if (product != null)
            {
                var cart = Session["Cart"] as Cart ?? new Cart();
                var cartItem = cart.Items.FirstOrDefault(x => x.Id == product.Id);
                if(cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.Items.Add(new ShopCartListModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        ImageUrl = product.Images.FirstOrDefault().ImageUrl,
                        Price = product.SalePrice,
                        Quantity = 1,
                    });
                }

                Session["Cart"] = cart;
                return Json(new { success = true, message = "Ürün sepete eklendi" });
            }
            else
            {
                return Json(new { success = false, message = "Product not found" });
            }
            
        }
        [HttpPost]
        public ActionResult DeleteCart(int Id)
        {
            var cart = Session["Cart"] as Cart ?? new Cart();
            var deleteCart = cart.Items.FirstOrDefault(x => x.Id == Id);
            cart.Items.Remove(deleteCart);
            return Json(new { success = false, message = "Product has been removed" });
        }
    }
}