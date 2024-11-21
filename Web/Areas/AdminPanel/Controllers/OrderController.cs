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
    public class OrderController : Controller
    {
        // GET: AdminPanel/Order
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<Order>().GetAll();
            return View(result);
        }
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.GetRepository<Order>().Add(order);
        //        unitOfWork.Complate();
        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<Order>().GetById(Id);
            return View(result);
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<Order>().GetById(Id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(Order order)
        {
            try
            {
                unitOfWork.GetRepository<Order>().Update(order);
                unitOfWork.Complate();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(order);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                Order order = unitOfWork.GetRepository<Order>().GetById(id);
                IEnumerable<Order> orders = new List<Order>() { order };
                unitOfWork.GetRepository<Order>().DeleteRange(orders);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}