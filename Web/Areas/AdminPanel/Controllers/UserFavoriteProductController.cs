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
    public class UserFavoriteProductController : Controller
    {
        // GET: AdminPanel/UserFavoriteProduct
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<UserFavoriteProduct>().GetAll();
            return View(result);
        }
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(UserFavoriteProduct userFavoriteProduct)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.GetRepository<UserFavoriteProduct>().Add(userFavoriteProduct);
        //        unitOfWork.Complate();
        //        return RedirectToAction("Index");
        //    }
        //    return View(userFavoriteProduct);
        //}
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<UserFavoriteProduct>().GetById(Id);
            return View(result);
        }
        //[HttpGet]
        //public ActionResult Edit(int Id)
        //{
        //    var result = unitOfWork.GetRepository<UserGroup>().GetById(Id);
        //    return View(result);
        //}
        //[HttpPost]
        //public ActionResult Edit(UserFavoriteProduct userFavoriteProduct)
        //{
        //    try
        //    {
        //        unitOfWork.GetRepository<UserFavoriteProduct>().Update(userFavoriteProduct);
        //        unitOfWork.Complate();
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        //    }
        //    return View(userFavoriteProduct);
        //}
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                UserFavoriteProduct userFavoriteProduct = unitOfWork.GetRepository<UserFavoriteProduct>().GetById(id);
                IEnumerable<UserFavoriteProduct> userFavoriteProducts = new List<UserFavoriteProduct>() { userFavoriteProduct };
                unitOfWork.GetRepository<UserFavoriteProduct>().DeleteRange(userFavoriteProducts);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}