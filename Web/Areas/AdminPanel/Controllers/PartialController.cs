using Buisness.SessionAuth;
using DataAccess.Context;
using DataAccess;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.AdminPanel.Controllers
{
    public class PartialController : Controller
    {
        // GET: AdminPanel/Partial
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public PartialViewResult Menu()
        {
            var user = SessionManeger.GetUserName();
            ViewBag.User = user;
            return PartialView("_Menu");
        }
        public PartialViewResult SideBar()
        {
            return PartialView("_SideBar");
        }
        public PartialViewResult Footer()
        {
            return PartialView("_Footer");
        }
        [HttpGet]
        public ActionResult UserDetail()
        {
            var user = SessionManeger.GetUserName();
            ViewBag.User = user;
            if (user != null)
            {
                var userDetail = unitOfWork.GetRepository<User>().GetAll().Where(x => x.Name == user).SingleOrDefault();
                return View(userDetail);
            }
            return View();
        }
        [HttpPost]
        public ActionResult UserDetail(User user)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}