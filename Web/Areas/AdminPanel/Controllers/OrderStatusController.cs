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
    public class OrderStatusController : Controller
    {
        // GET: AdminPanel/OrderStatus
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<OrderStatus>().GetAll();
            return View(result);
        }
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(OrderStatus orderStatus)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitOfWork.GetRepository<OrderStatus>().Add(orderStatus);
        //        unitOfWork.Complate();
        //        return RedirectToAction("Index");
        //    }
        //    return View(orderStatus);
        //}
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<OrderStatus>().GetById(Id);
            return View(result);
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<OrderStatus>().GetById(Id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(OrderStatus orderStatus)
        {
            try
            {
                unitOfWork.GetRepository<OrderStatus>().Update(orderStatus);
                unitOfWork.Complate();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(orderStatus);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                OrderStatus orderStatus = unitOfWork.GetRepository<OrderStatus>().GetById(id);
                IEnumerable<OrderStatus> orderStatuses = new List<OrderStatus>() { orderStatus };
                unitOfWork.GetRepository<OrderStatus>().DeleteRange(orderStatuses);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}