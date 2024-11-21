using DataAccess.Context;
using DataAccess;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Drawing2D;
using Buisness.SessionAuth;

namespace Web.Areas.AdminPanel.Controllers
{
    public class BrandController : Controller
    {
        // GET: AdminPanel/Brand
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        [AuthorizationFilter]
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<Brand>().GetAll();
            return View(result);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            if (ModelState.IsValid)
            {
                if(user!= null)
                {
                    brand.CreatedBy = user.Id;
                    brand.CreatedDate = DateTime.Now;
                    brand.ModifiedDate = DateTime.Now;
                    brand.ModifiedBy = user.Id;
                    unitOfWork.GetRepository<Brand>().Add(brand);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            return View(brand);
        }
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<Brand>().GetById(Id);
            return View(result);
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<Brand>().GetById(Id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            var previousRecord = unitOfWork.GetRepository<Brand>().GetById(brand.Id);
            try
            {
                if (user != null)
                {
                    brand.CreatedBy = previousRecord.CreatedBy;
                    brand.CreatedDate = previousRecord.CreatedDate;
                    brand.ModifiedBy = user.Id;
                    brand.ModifiedDate = DateTime.Now;
                    unitOfWork.GetRepository<Brand>().Update(brand);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(brand);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                Brand brand = unitOfWork.GetRepository<Brand>().GetById(id);
                IEnumerable<Brand> brands = new List<Brand>() { brand };
                unitOfWork.GetRepository<Brand>().DeleteRange(brands);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
    }
}