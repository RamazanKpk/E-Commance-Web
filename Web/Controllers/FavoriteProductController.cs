using Buisness.Interface.Abstract.WebAbstract;
using DataAccess;
using DataAccess.Context;
using DataModel.BrandViewModels;
using DataModel.ProductViewModels;
using Entity;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class FavoriteProductController : Controller
    {
        // GET: FavoriteProduct
        private readonly IFavoriteProductService _favoriteProductService;
        private IAllWebService<BrandListModel> _brandAllWebServices;
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public FavoriteProductController(IAllWebService<BrandListModel> brandAllWebServices, 
            IFavoriteProductService favoriteProductService)
        {
            _favoriteProductService = favoriteProductService;
            _brandAllWebServices = brandAllWebServices;
        }
        [HttpGet]
        public ActionResult FavoriteProduct()
        {
            var user = Session["User"] as User;
            ViewBag.Brand = _brandAllWebServices.GetApiToWeb();
            if (user == null)
            {
                TempData["Message"] = "Lüthen Giriş Yapınız";
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                var products = _favoriteProductService.GetByuserId(user.Id);
                return View(products);
            }
        }
        [HttpPost]
        public ActionResult AddFavoriteProduct(int Id)
        {
            var user = Session["User"] as User;
            if (user == null)
            {
                return Json(new { success = false, message = "Lütfen giriş yapın" });
            }
            else
            {
                 _favoriteProductService.AddFavoriteProducut(Id, user.Id);
                return Json(new { success = true, message = "Ürün sepete eklendi" });
            }
        }
        [HttpPost]
        public ActionResult RemoveFavoriteProduct(int Id)
        {
            var deletId = unitOfWork.GetRepository<UserFavoriteProduct>().GetAll().Where(x => x.ProductId == Id).FirstOrDefault();
            unitOfWork.GetRepository<UserFavoriteProduct>().Delete(deletId.Id);
            unitOfWork.Complate();
            return Json(new { success = true, message = "Favori Ürün sepete kaldırıldı" });
        }
    }
}