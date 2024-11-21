using Buisness.Interface.Abstract.WebAbstract;
using DataAccess;
using DataAccess.Context;
using DataModel.ShopCartModels;
using DataModel.UserContactViewModels;
using Entity;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        private readonly IUserInfoService<UserInfoListModel> _userInfoService;
        public ContactController (IUserInfoService<UserInfoListModel> userInfoService)
        {
            _userInfoService = userInfoService;
        }
        public ContactController() { }
        // GET: Contact
        [HttpGet]
        public ActionResult CheckOut()
        {
            var cart = Session["Cart"] as Cart ?? new Cart();
            var user = Session["User"] as User;
            ViewBag.Cart = cart;
            //var userContact = unitOfWork.GetRepository<UserContact>().GetById(user.Id);
            //if(userContact != null)
            //{
            //    return View(userContact);
            //}           
            return View();
        }
        [HttpGet]
        public ActionResult Confirmation()
        {
            var cart = Session["Cart"] as Cart ?? new Cart();
            return View(cart);
        }

    }
}